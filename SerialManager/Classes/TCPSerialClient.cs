using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SerialManager.Classes
{
    public class TCPSerialClient
    {
        IPAddress address;
        int port;

        public event EventHandler Disconnected;
        public event EventHandler<ClientDataReceivedEventArgs> Message;

        TcpClient clt;
        NetworkStream ns;
        StreamReader sr;

        public TCPSerialClient(IPAddress Address, int Port)
        {
            address = Address;
            port = Port;
        }

        public async Task<bool> Open()
        {
            if (clt != null)
                return false;

            try
            {

                clt = new TcpClient();
                await clt.ConnectAsync(address, port).ConfigureAwait(false);
                ns = clt.GetStream();
                sr = new StreamReader(ns, Encoding.UTF8);
                Task.Run(() => HandleConnection());
            }
            catch
            {
                Dispose();
                return false;
            }

            return true;
        }

        public void Close()
        {
            Dispose();
        }

        public async Task<bool> Send(SerialMessage Message)
        {
            if (clt == null || !clt.Connected)
                return false;

            try
            {
                var eDData = Message.Serialize();
                await ns.WriteAsync(eDData, 0, eDData.Length).ConfigureAwait(false);
                return true;
            }
            catch { return false; }
        }

        private async void HandleConnection()
        {
            

            try
            {
                while (true)
                {
                    string line = await sr.ReadLineAsync().ConfigureAwait(false);

                    if (string.IsNullOrWhiteSpace(line))
                    {
                        if (Disconnected != null)
                            Disconnected(this, EventArgs.Empty);

                        Dispose();
                        return;
                    }
                    else
                    {
                        try
                        {
                            if (Message != null)
                                Message(this, new ClientDataReceivedEventArgs { Message = new SerialMessage(line) });
                        }
                        catch { }
                    }
                }

            }
            catch { }
        }

        public void Dispose()
        {
            try
            {
                sr.Close();
                sr.Dispose();
            }
            catch { }

            try
            {
                ns.Close();
                ns.Dispose();
            }
            catch { }

            try
            {
                clt.Close();
                clt.Dispose();
            }
            catch { }
        }

       
    }

    public class SerialMessage
    {
        public string Command { get { return command; } }
        string command;

        public string[] Parameters { get { return parameters; } }
        string[] parameters;

        public SerialMessage(string Command, params string[] Parameters)
        {
            command = Command;
            parameters = Parameters;
        }

        internal SerialMessage(string Message)
        {
            int index = Message.IndexOf(":");

            if (index == -1)
                command = Message;
            else
            {
                command = Message.Substring(0, index);
                string prms = Unescape(Message.Substring(index + 1));

                switch (command)
                {
                    default:
                        parameters = new string[] { prms };
                        break;

                    case "AvailablePorts":
                        parameters = prms.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        break;
                    case "PortData":
                        index = prms.IndexOf(",");

                        if (index == -1)
                            parameters = new string[] { prms };
                        else
                            parameters = new string[] { prms.Substring(0, index), prms.Substring(index + 1) };
                        break;
                }
            }
        }

        internal byte[] Serialize()
        {
            if (parameters != null && parameters.Length > 0)
                return Encoding.UTF8.GetBytes($"{command}:{Escape(string.Join(",", Parameters))}\r\n");
            else
                return Encoding.UTF8.GetBytes($"{command}:\r\n");
        }

        private static string Escape(string Data)
        {
            return Data.Replace("\n", "·NL%").Replace("\r", "·CR%");
        }

        private static string Unescape(string Data)
        {
            return Data.Replace("·NL%", "\n").Replace("·CR%", "\r");
        }

    }

    public class ClientDataReceivedEventArgs : EventArgs
    {
        public SerialMessage Message { get; set; }
    }
}
