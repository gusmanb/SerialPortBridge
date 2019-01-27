using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SerialLinuxHost
{
    public static class Server
    {
        static TcpListener listener;
        static ConcurrentDictionary<Guid, ConnectedClient> connectedClients = new ConcurrentDictionary<Guid, ConnectedClient>();

        public static event EventHandler<ConnectedClientEventArgs> ClientConnected;
        public static event EventHandler<ConnectedClientEventArgs> ClientDisconnected;
        public static event EventHandler<MessageFromClientEventArgs> ClientMessage;

        public static void Start(IPAddress ListenAddress, int Port)
        {
            if (listener != null)
                return;

            listener = new TcpListener(ListenAddress, Port);
            listener.Start();
            listener.BeginAcceptTcpClient(NewClient, null);
        }

        public static void Stop()
        {
            if (listener != null)
            {
                listener.Stop();
                listener = null;
            }

            foreach (var client in connectedClients)
                DestroyClient(client.Value);

            connectedClients.Clear();
        }

        public static async Task<bool> Send(Guid Client, string Data)
        {
            ConnectedClient client;

            if (!connectedClients.TryGetValue(Client, out client))
                return false;

            try
            {
                var eDData = Encoding.UTF8.GetBytes(Escape(Data) + "\r\n");
                await client.ClientStream.WriteAsync(eDData, 0, eDData.Length).ConfigureAwait(false);
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

        private static void NewClient(IAsyncResult Result)
        {
            try
            {
                var res = listener.EndAcceptTcpClient(Result);

                if (res != null)
                    Task.Run(() => HandleClient(res));
            }
            catch { }

            if (listener == null)
                return;

            try
            {
                listener.BeginAcceptSocket(NewClient, null);
            }
            catch { }
        }

        private static async void HandleClient(TcpClient res)
        {
            ConnectedClient client = new ConnectedClient
            {
                Id = Guid.NewGuid(),
                Client = res,
                ClientStream = res.GetStream(),
            };

            client.ClientReader = new StreamReader(client.ClientStream, Encoding.UTF8);
            
            connectedClients[client.Id] = client;

            if (ClientConnected != null)
                ClientConnected(client, new ConnectedClientEventArgs { ClientId = client.Id, ClientIP = (res.Client.RemoteEndPoint as IPEndPoint).Address });

            try
            {
                while (true)
                {
                    var line = await client.ClientReader.ReadLineAsync().ConfigureAwait(false);

                    if (string.IsNullOrWhiteSpace(line))
                    {
                        if(ClientDisconnected != null)
                            ClientDisconnected(client, new ConnectedClientEventArgs { ClientId = client.Id, ClientIP = (res.Client.RemoteEndPoint as IPEndPoint).Address });

                        DestroyClient(client);
                        return;
                    }
                    else
                    {
                        try
                        {
                            if (ClientMessage != null)
                                ClientMessage(client, new MessageFromClientEventArgs { ClientId = client.Id, ClientIP = (res.Client.RemoteEndPoint as IPEndPoint).Address, Message = Unescape(line) });
                        }
                        catch { }
                    }
                }
            }
            catch
            {
                if (ClientDisconnected != null)
                    ClientDisconnected(client, new ConnectedClientEventArgs { ClientId = client.Id });

                DestroyClient(client);
            }

        }

        private static void DestroyClient(ConnectedClient Client)
        {
            connectedClients.TryRemove(Client.Id, out var dummy);

            try
            {
                Client.ClientReader.Close();
                Client.ClientReader.Dispose();
            }
            catch { }
            
            try
            {
                Client.ClientStream.Close();
                Client.ClientStream.Dispose();
            }
            catch { }

            try
            {
                Client.Client.Close();
                Client.Client.Dispose();
            }
            catch { }
        }

        class ConnectedClient
        {
            public Guid Id { get; set; }
            public TcpClient Client { get; set; }
            public NetworkStream ClientStream { get; set; }
            public StreamReader ClientReader { get; set; }
        }
    }

    public class ConnectedClientEventArgs : EventArgs
    {
        public Guid ClientId { get; set; }
        public IPAddress ClientIP { get; set; }
    }

    public class MessageFromClientEventArgs : ConnectedClientEventArgs
    {
        public string Message { get; set; }
    }
}
