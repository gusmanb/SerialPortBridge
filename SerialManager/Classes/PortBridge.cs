using NeoSmart.AsyncLock;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SerialManager.Classes
{
    public class PortBridge : IDisposable
    {
        TCPSerialClient client;
        SerialPort serialPort;

        BridgeState currentState = BridgeState.Idle;

        string localPort;
        string remotePort;
        IPAddress server;
        int port;
        int bauds;

        public BridgeState State { get { return currentState; } }

        public event EventHandler<ActivityEventArgs> Activity;
        public event EventHandler<StatusEventArgs> StateChanged;

        AsyncLock processLocker = new AsyncLock();

        public PortBridge(string RemotePort, string LocalPort, int Bauds, IPAddress Server, int Port)
        {
            remotePort = RemotePort;
            localPort = LocalPort;
            server = Server;
            port = Port;
            bauds = Bauds;
    
        }

        public void Start()
        {
            ProcessMachineState(BridgeState.RestartDelay);
        }

        private void ProcessMachineState(BridgeState State)
        {
            if (currentState == BridgeState.Disposed)
                return;

            Task.Run(async () => 
            {
                using(await processLocker.LockAsync())
                {
                    if (await SetMachineState(State))
                    {
                        if (StateChanged != null)
                            StateChanged(this, new StatusEventArgs { LocalPort = localPort, RemotePort = remotePort, Status = State });

                        switch (currentState)
                        {
                            case BridgeState.Idle:
                                break;
                            case BridgeState.RestartDelay:
                                break;
                            case BridgeState.OpeningLocal:
                                ProcessMachineState(BridgeState.LocalOpen);
                                break;
                            case BridgeState.LocalOpen:
                                ProcessMachineState(BridgeState.Connecting);
                                break;
                            case BridgeState.Connecting:
                                ProcessMachineState(BridgeState.Connected);
                                break;
                            case BridgeState.Connected:
                                ProcessMachineState(BridgeState.OpeningRemote);
                                break;
                            case BridgeState.OpeningRemote:
                                break;
                            case BridgeState.RemoteOpen:
                                ProcessMachineState(BridgeState.Ready);
                                break;
                        }
                    }
                    else
                    {
                        await SetMachineState(BridgeState.RestartDelay);

                        if (StateChanged != null)
                            StateChanged(this, new StatusEventArgs { LocalPort = localPort, RemotePort = remotePort, Status = State });

                        return;
                    }
                }
            });
        }

        private async Task<bool> SetMachineState(BridgeState Status)
        {
            if (Status > currentState)
            {
                BridgeState tmpState = currentState + 1;
                while (tmpState <= Status)
                {
                    if (!await DoState(tmpState))
                    {
                        UndoState(tmpState);
                        return false;
                    }
                    else
                    {
                        currentState = tmpState;
                        tmpState++;
                    }
                }

                return true;
            }
            else if (Status < currentState)
            {
                BridgeState tmpState = currentState;

                while (tmpState >= Status)
                    UndoState(tmpState--);

                ProcessMachineState(Status);

                return true;
            }
            else
                return true;
        }

        private async Task<bool> DoState(BridgeState StateToExecute)
        {
            switch (StateToExecute)
            {
                case BridgeState.Idle:
                    return true;

                case BridgeState.RestartDelay:

                    Task.Run(async () => 
                    {
                        await Task.Delay(10000);

                        if (currentState != BridgeState.RestartDelay)
                            return;

                        ProcessMachineState(BridgeState.OpeningLocal);
                    });

                    return true;

                case BridgeState.OpeningLocal:

                    try
                    {
                        serialPort = new SerialPort(localPort, bauds);
                        serialPort.DataReceived += SerialPort_DataReceived;
                        serialPort.ErrorReceived += SerialPort_ErrorReceived;
                        serialPort.Open();
                        
                        return true;
                    }
                    catch { }

                    return false;

                case BridgeState.LocalOpen:
                    return true;

                case BridgeState.Connecting:

                    try
                    {
                        client = new TCPSerialClient(server, port);
                        client.Message += Client_Message;
                        client.Disconnected += Client_Disconnected;
                        var connected = await client.Open();
                        return connected;
                    }
                    catch { }

                    return false;

                case BridgeState.Connected:

                    return true;

                case BridgeState.OpeningRemote:

                    try
                    {
                        return await client.Send(new SerialMessage("OpenPort", remotePort, bauds.ToString()));
                    }
                    catch { }

                    return false;

                case BridgeState.RemoteOpen:

                    return true;

                case BridgeState.Ready:

                    return true;

                case BridgeState.Disposed:

                    await SetMachineState(BridgeState.Idle);
                    currentState = BridgeState.Disposed;
                    return true;

                default:
                    return false;
            }

            
        }

        private void UndoState(BridgeState State)
        {
            switch (State)
            {
                case BridgeState.Idle:
                case BridgeState.RestartDelay:
                    currentState = BridgeState.Idle;
                    break;
                case BridgeState.OpeningLocal:

                    if (serialPort != null)
                    {
                        serialPort.DataReceived -= SerialPort_DataReceived;
                        serialPort.ErrorReceived -= SerialPort_ErrorReceived;

                        try { serialPort.Close(); } catch { }

                        serialPort = null;
                    }
                    break;

                case BridgeState.LocalOpen:
                    currentState = BridgeState.OpeningLocal;
                    break;

                case BridgeState.Connecting:

                    if(client != null)
                    {
                        client.Message -= Client_Message;
                        client.Disconnected -= Client_Disconnected;

                        try { client.Dispose(); } catch { }

                        client = null;
                    }
                    currentState = BridgeState.LocalOpen;
                    break;

                case BridgeState.Connected:
                    currentState = BridgeState.Connecting;
                    break;

                case BridgeState.OpeningRemote:

                    client.Send(new SerialMessage("ClosePort", remotePort));
                    currentState = BridgeState.Connected;
                    break;

                case BridgeState.RemoteOpen:
                    currentState = BridgeState.OpeningRemote;
                    break;

                case BridgeState.Ready:
                    currentState = BridgeState.RemoteOpen;
                    break;
            }
        }

        private void SerialPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            if (currentState != BridgeState.Ready)
                return;

            ProcessMachineState(BridgeState.RestartDelay);
        }

        private void Client_Disconnected(object sender, EventArgs e)
        {
            if (currentState != BridgeState.Ready)
                return;

            ProcessMachineState(BridgeState.RestartDelay);
        }

        private void Client_Message(object sender, ClientDataReceivedEventArgs e)
        {
            switch (currentState)
            {
                case BridgeState.OpeningRemote:

                    if (e.Message.Command == "PortOpen")
                        ProcessMachineState(BridgeState.RemoteOpen);
                    else
                        ProcessMachineState(BridgeState.RestartDelay);

                    break;

                case BridgeState.Ready:

                    switch (e.Message.Command)
                    {
                        case "DataSent":

                            if (e.Message.Parameters == null || e.Message.Parameters.Length != 1 || e.Message.Parameters[0] != remotePort)
                                ProcessMachineState(BridgeState.RestartDelay);

                            break;

                        case "PortData":

                            if (e.Message.Parameters == null || e.Message.Parameters.Length != 2 || e.Message.Parameters[0] != remotePort)
                                ProcessMachineState(BridgeState.RestartDelay);
                            else
                            {
                                try
                                {
                                    byte[] data = Convert.FromBase64String(e.Message.Parameters[1]);
                                    serialPort.Write(data, 0, data.Length);

                                    if (Activity != null)
                                        Activity(this, new ActivityEventArgs { LocalPort = localPort, RemotePort = remotePort, Local = false });
                                }
                                catch { ProcessMachineState(BridgeState.RestartDelay); }
                            }

                            break;

                        case "PortClosed":

                            if(e.Message.Parameters.Length == 1 && e.Message.Parameters[0] == remotePort)
                                ProcessMachineState(BridgeState.RestartDelay);

                            break;
                    }

                    break;
            }
        }

        private async void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (currentState != BridgeState.Ready)
                return;

            try
            {
                while (currentState == BridgeState.Ready && serialPort.BytesToRead > 0)
                {
                    byte[] data = new byte[serialPort.BytesToRead];
                    serialPort.Read(data, 0, data.Length);

                    if (!await client.Send(new SerialMessage("SendData", remotePort,Convert.ToBase64String(data))))
                    {
                        ProcessMachineState(BridgeState.RestartDelay);
                        return;
                    }

                    if (Activity != null)
                        Activity(this, new ActivityEventArgs { LocalPort = localPort, RemotePort = remotePort, Local = true });

                }
                
            }
            catch
            { ProcessMachineState(BridgeState.RestartDelay); }
        }

        public void Dispose()
        {
            ProcessMachineState(BridgeState.Disposed);
        }
    }

    public class ActivityEventArgs : EventArgs
    {
        public string LocalPort { get; set; }
        public string RemotePort { get; set; }
        public bool Local { get; set; }
    }

    public class StatusEventArgs : EventArgs
    {
        public string LocalPort { get; set; }
        public string RemotePort { get; set; }
        public BridgeState Status { get; set; }
    }

    public enum BridgeState
    {
        Idle,
        RestartDelay,
        OpeningLocal,
        LocalOpen,
        Connecting,
        Connected,
        OpeningRemote,
        RemoteOpen,
        Ready,
        Disposed
    }
}
