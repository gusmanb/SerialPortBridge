using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TCPSerialClient
{
    public class Client : IDisposable
    {
        IPAddress address;
        int port;

        public event EventHandler Disconnected;
        public event EventHandler<ClientDataReceivedEventArgs> Message;

        TcpClient clt;
        NetworkStream ns;
        StreamReader sr;

        public Client(IPAddress Address, int Port)
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

        public async Task<bool> Send(string Message)
        {
            if (clt == null || !clt.Connected)
                return false;

            try
            {
                var eDData = Encoding.UTF8.GetBytes(Escape(Message) + "\r\n");
                await ns.WriteAsync(eDData, 0, eDData.Length).ConfigureAwait(false);
                return true;
            }
            catch { return false; }
        }

        private static string Escape(string Data)
        {
            return Data.Replace("\n", "·NL%").Replace("\r", "·CR%");
        }

        private static string Unescape(string Data)
        {
            return Data.Replace("·NL%", "\n").Replace("·CR%", "\r");
        }
        
        private async void HandleConnection()
        {
            ns = clt.GetStream();
            sr = new StreamReader(ns, Encoding.UTF8);

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
                                Message(this, new ClientDataReceivedEventArgs { Message = Unescape(line) });
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

    public class ClientDataReceivedEventArgs : EventArgs
    {
        public string Message { get; set; }
    }
}
