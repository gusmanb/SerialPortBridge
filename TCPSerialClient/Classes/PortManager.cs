using Com0Com;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TCPSerialClient
{
    public class PortManager : IDisposable
    {
        IPAddress address;
        int port;
        Client clt;
        TaskCompletionSource<string[]> remotePortsTask;

        public bool IsOpen { get; private set; }

        public RemotePort[] PairedPorts
        {
            get
            {
                return remotePorts.Values.Select(r => new RemotePort
                {
                    Ready = r.Open,
                    InputPortName = r.InputPortName,
                    OutputPortName = r.OutputPortName,
                    RemotePortName = r.RemotePortName,
                    RemotePortBauds = r.RemoteBauds

                }).ToArray();
            }
        }

        public event EventHandler Disconnected;

        public event EventHandler<PortEventArgs> PortReady;
        public event EventHandler<PortEventArgs> PortUnready;

        public event EventHandler<PortEventArgs> LocalPortActivity;
        public event EventHandler<PortEventArgs> RemotePortActivity;

        ConcurrentDictionary<string, LocalPortBridge> remotePorts = new ConcurrentDictionary<string, LocalPortBridge>();
        ConcurrentDictionary<string, LocalPortBridge> closedRemotePorts = new ConcurrentDictionary<string, LocalPortBridge>();
        ConcurrentDictionary<string, LocalPortBridge> openingRemotePorts = new ConcurrentDictionary<string, LocalPortBridge>();
        ConcurrentDictionary<string, LocalPortBridge> openRemotePorts = new ConcurrentDictionary<string, LocalPortBridge>();

        public PortManager(IPAddress ServerAddress, int ServerPort)
        {
            address = ServerAddress;
            port = ServerPort;
        }

        public async Task<bool> Open()
        {
            if (clt != null)
                return false;

            clt = new Client(address, port);
            clt.Disconnected += Clt_Disconnected;
            clt.Message += Clt_Message;

            if (!await clt.Open().ConfigureAwait(false))
            {
                Dispose();
                return false;
            }

            foreach (var port in remotePorts)
                closedRemotePorts[port.Key] = port.Value;

            IsOpen = true;

            Task.Run(() => SupervisePorts());

            return true;
        }

        public void Close()
        {
            Dispose();
        }

        public bool AddPort(string InputPort, string OutputPort, string RemotePort, int RemoteBauds)
        {
            var ports = SerialPort.GetPortNames();

            if (ports.Contains(InputPort) || ports.Contains(OutputPort) || remotePorts.ContainsKey(RemotePort))
                return false;

            LocalPortBridge bridge = new LocalPortBridge(InputPort, OutputPort, RemotePort, RemoteBauds);

            bridge.Closed += Bridge_Closed;
            bridge.Data += Bridge_Data;
            remotePorts[RemotePort] = bridge;
            closedRemotePorts[RemotePort] = bridge;

            return true;
        }

        public bool RemovePort(string RemotePort)
        {
            LocalPortBridge bridge;

            if (!remotePorts.TryRemove(RemotePort, out bridge))
                return false;

            closedRemotePorts.TryRemove(RemotePort, out var dummy);
            openingRemotePorts.TryRemove(RemotePort, out dummy);
            openRemotePorts.TryRemove(RemotePort, out dummy);

            bridge.Dispose();
            bridge.Closed -= Bridge_Closed;
            bridge.Data -= Bridge_Data;

            try
            {
                clt.Send($"ClosePort:{RemotePort}");
            }
            catch { }

            return true;
        }

        public Task<string[]> ListRemotePorts()
        {

            if (!IsOpen)
                throw new InvalidOperationException("Not connected");

            if (remotePortsTask != null)
                return remotePortsTask.Task;

            remotePortsTask = new TaskCompletionSource<string[]>();

            clt.Send("ListPorts:");

            return remotePortsTask.Task;
        }

        public string[] ListLocalFreePorts()
        {
            string[] ports = SerialPort.GetPortNames();

            List<string> freePorts = new List<string>();

            var usedCom0Com = LocalPortBridge.manager.GetCrossoverPortPairs();

            for (int buc = 1; buc < 100; buc++)
            {
                string port = "COM" + buc;

                if (ports.Contains(port) || usedCom0Com.Any(p => p.PortNameA == port || p.PortNameB == port))
                    continue;

                freePorts.Add(port);
            }

            return freePorts.ToArray();
        }


        private void Bridge_Data(object sender, PortDataEventArgs e)
        {
            clt.Send($"SendData:{e.RemotePortName},{Convert.ToBase64String(e.Data)}");

            if (RemotePortActivity != null)
                RemotePortActivity(this, new PortEventArgs { RemotePortName = e.RemotePortName });

        }

        private void Bridge_Closed(object sender, PortEventArgs e)
        {
            throw new NotImplementedException();
        }


        private async void SupervisePorts()
        {
            try
            {
                while (IsOpen)
                {
                    await Task.Delay(15000);

                    if (!IsOpen)
                        return;

                    foreach (var port in closedRemotePorts.ToArray())
                    {
                        closedRemotePorts.TryRemove(port.Key, out var dummy);
                         openingRemotePorts[port.Key] = port.Value;
                        await clt.Send($"OpenPort:{port.Value.RemotePortName},{port.Value.RemoteBauds}");
                        
                    }
                }
            }
            catch { }
        }

        private async void Clt_Message(object sender, ClientDataReceivedEventArgs e)
        {
            int pos = e.Message.IndexOf(":");

            if (pos == -1)
            {
                Console.WriteLine("No command, ignoring message");
                return;
            }

            string command = e.Message.Substring(0, pos);
            string data = e.Message.Substring(pos + 1);

            switch (command)
            {
                case "AvailablePorts":

                    string[] ports = data.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                    if (remotePortsTask != null)
                    {
                        remotePortsTask.SetResult(ports);
                        remotePortsTask = null;
                    }

                    break;

                case "OpenError":
                case "BadData":

                    RemoveOpeningPort(data);
                    Debug.WriteLine(e.Message);

                    break;

                case "AlreadyOpen":
                case "PortOpen":

                    await OpenLocalPortBridge(data);

                    break;

                case "PortClosed":

                    CloseLocalPortBridge(data, true);

                    break;

                case "ErrorClosing":

                    Debug.WriteLine(e.Message);

                    break;

                case "NotOpen":

                    Debug.WriteLine(e.Message);

                    break;

                case "DataSent":
                    break;

                case "PortData":

                    int posPort = data.IndexOf(",");

                    if (posPort == -1)
                    {
                        Debug.WriteLine($"No port found on the message: {e.Message}");
                        await clt.Send($"NoPort:{data}");
                        break;
                    }

                    string port = data.Substring(0, posPort);
                    string realData = data.Substring(posPort + 1);

                    SendDataToBridge(port, realData);

                    break;

                case "Unknown":

                    Debug.WriteLine(e.Message);

                    break;

                default:

                    Debug.WriteLine(e.Message);
                    await clt.Send($"Unknown:{command}");
                    break;
                    
            }
        }

        private void RemoveOpeningPort(string Data)
        {
            int pos = Data.IndexOf(",");
            string port = pos == -1 ? Data : Data.Substring(0, pos);

            if (openingRemotePorts.TryRemove(port, out var rp))
                closedRemotePorts[port] = rp;
        }

        private void SendDataToBridge(string RemotePortName, string Data)
        {
            LocalPortBridge bridge;

            if (!remotePorts.TryGetValue(RemotePortName, out bridge))
                return;

            bridge.Send(Convert.FromBase64String(Data));
        }

        private void CloseLocalPortBridge(string RemotePortName, bool NotifyUnready)
        {
            LocalPortBridge bridge;

            if (!remotePorts.TryGetValue(RemotePortName, out bridge))
                return;

            bridge.Dispose();

            if (!openingRemotePorts.TryRemove(RemotePortName, out var dummy) && !openRemotePorts.TryRemove(RemotePortName, out dummy))
                return;

            closedRemotePorts[RemotePortName] = bridge;

            clt.Send($"ClosePort:{RemotePortName}");

            if (NotifyUnready)
            {
                if (PortUnready != null)
                    PortUnready(this, new PortEventArgs { RemotePortName = RemotePortName });
            }
        }

        private async Task OpenLocalPortBridge(string RemotePortName)
        {
            LocalPortBridge bridge;

            if (!openingRemotePorts.TryRemove(RemotePortName, out bridge))
            {
                CloseLocalPortBridge(RemotePortName, false);
                await clt.Send($"ClosePort:{RemotePortName}");
            }
            else
            {
                openRemotePorts[RemotePortName] = bridge;

                if (!await bridge.RemoteOpened())
                    await clt.Send($"ClosePort:{RemotePortName}");
                else if (PortReady != null)
                    PortReady(this, new PortEventArgs { RemotePortName = RemotePortName });
            }
        }

        private void Clt_Disconnected(object sender, EventArgs e)
        {
            IsOpen = false;
            Dispose();

            if (Disconnected != null)
                Disconnected(this, EventArgs.Empty);
        }

        public void Dispose()
        {
            IsOpen = false;

            foreach (var item in openingRemotePorts)
                item.Value.Dispose();

            foreach (var item in openRemotePorts)
                item.Value.Dispose();

            openRemotePorts.Clear();
            openingRemotePorts.Clear();
            closedRemotePorts.Clear();

            try
            {
                clt.Close();
                clt.Dispose();

                clt.Disconnected -= Clt_Disconnected;
                clt.Message -= Clt_Message;
            }
            catch { }

            clt = null;
        }

        class LocalPortBridge : IDisposable
        {
            internal static Com0ComManager manager = new Com0ComManager();

            public string InputPortName { get; private set; }
            public string OutputPortName { get; private set; }
            public string RemotePortName { get; private set; }
            public int RemoteBauds { get; private set; }

            public bool Open { get; private set; }

            public event EventHandler<PortDataEventArgs> Data;
            public event EventHandler<PortEventArgs> Closed;

            SerialPort dataPort;
            CrossoverPortPair pair;

            public LocalPortBridge(string InputPort, string OutputPort, string RemotePort, int RemoteBauds)
            {
                this.InputPortName = InputPort;
                this.OutputPortName = OutputPort;
                this.RemotePortName = RemotePort;
                this.RemoteBauds = RemoteBauds;
            }

            public async Task<bool> RemoteOpened()
            {
                try
                {
                    var pairs = await manager.GetCrossoverPortPairsAsync();

                    foreach (var pair in pairs)
                    {
                        if (pair.PortNameA == InputPortName || pair.PortNameB == OutputPortName)
                            await manager.DeletePortPairAsync(pair.PairNumber);
                    }

                    pair = await manager.CreatePortPairAsync(InputPortName, OutputPortName);

                    if (pair != null)
                    {
                        dataPort = new SerialPort(OutputPortName);
                        dataPort.DataReceived += DataPort_DataReceived;
                        dataPort.Open();
                        Open = true;
                        return true;
                    }
                    else
                        throw new Exception();

                }catch
                {
                    Dispose();
                    return false;
                }

            }

            private void DataPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
            {
                try
                {
                    while (dataPort.BytesToRead > 0)
                    {
                        byte[] data = new byte[dataPort.BytesToRead];
                        dataPort.Read(data, 0, data.Length);

                        if (Data != null)
                            Data(this, new PortDataEventArgs { RemotePortName = RemotePortName, Data = data });
                    }

                }
                catch
                {
                    Dispose();

                    if (Closed != null)
                        Closed(this, new PortEventArgs { RemotePortName = RemotePortName });
                }
            }

            public void RemoteClosed()
            {
                Dispose();
            }

            public bool Send(byte[] Data)
            {
                if (!Open)
                    return false;

                try
                {
                    dataPort.Write(Data, 0, Data.Length);
                }
                catch
                {
                    Dispose();

                    if (Closed != null)
                        Closed(this, new PortEventArgs { RemotePortName = RemotePortName });

                    return false;
                }

                return true;
            }

            public void Dispose()
            {
                Open = false;

                if (pair != null)
                {
                    try
                    {
                        manager.DeletePortPair(pair.PairNumber);
                    }
                    catch { }
                }

                if (dataPort != null)
                {
                    try
                    {
                        dataPort.Close();
                        dataPort.DataReceived -= DataPort_DataReceived;
                    }
                    catch { }

                    dataPort = null;
                }
            }
        }

        class PortDataEventArgs : PortEventArgs
        {
            public byte[] Data { get; set; }
        }
    }

    public class PortEventArgs : EventArgs
    {
        public string RemotePortName { get; set; }
    }

    public class RemotePort
    {
        public bool Ready { get; set; }
        public string RemotePortName { get; set; }
        public int RemotePortBauds { get; set; }
        public string InputPortName { get; set; }
        public string OutputPortName { get; set; }
    }
}
