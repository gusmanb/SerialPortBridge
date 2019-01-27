using System;
using System.Collections.Concurrent;
using System.IO;
using System.IO.Ports;
using System.Net;
using System.Text;

namespace SerialLinuxHost
{
    class Program
    {
        static ConcurrentDictionary<Guid, ConnectedClient> connectedClients = new ConcurrentDictionary<Guid, ConnectedClient>();
        static ConcurrentDictionary<string, OpenPort> openPorts = new ConcurrentDictionary<string, OpenPort>();
        static void Main(string[] args)
        {
        
            Console.WriteLine("Starting server");

            try
            {

                Server.ClientConnected += Server_ClientConnected;
                Server.ClientDisconnected += Server_ClientDisconnected;
                Server.ClientMessage += Server_ClientMessage;

                Server.Start(IPAddress.Any, 9025);
            }
            catch
            {
                Console.WriteLine("Cannot open server! Terminating...");
                return;
            }

            Console.WriteLine("Server running on port 9025, press Q to quit");

            while (Console.ReadKey().Key != ConsoleKey.Q) ;

            Console.WriteLine("Bye!");

        }

        private static async void Server_ClientMessage(object sender, MessageFromClientEventArgs e)
        {
            //Console.WriteLine($"Message received from ID {e.ClientId} IP {e.ClientIP}: {e.Message}");

            ConnectedClient client;

            if (!connectedClients.TryGetValue(e.ClientId, out client))
            {
                Console.WriteLine("Cannot find connected client, cancelling");
                return;
            }

            int pos = e.Message.IndexOf(":");

            if (pos == -1)
            {
                Console.WriteLine("No command, ignoring message");
                return;
            }

            string command = e.Message.Substring(0, pos);
            string data = e.Message.Substring(pos + 1);


            switch(command)
            {
                case "ListPorts":

                    Console.WriteLine("Listing available ports");

                    try
                    {

                        var ports = string.Join(",", Directory.GetFiles("/dev", "tty*")).Replace("/dev/", "");
                        Console.WriteLine($"Available ports: {ports}");

                        if (!await Server.Send(e.ClientId, $"AvailablePorts:{ports}"))
                            Console.WriteLine("Error sending message!");
                        else
                            Console.WriteLine("Message sent");
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine($"Exception! {ex.Message}, {ex.StackTrace}");
                    }

                    break;

                case "OpenPort":

                    int index = data.IndexOf(",");

                    if (index == -1)
                    {
                        Console.WriteLine("Bad port data");
                        Server.Send(e.ClientId, $"BadData:{data}");
                        return;
                    }

                    string portName = data.Substring(0, index);
                    string portBauds = data.Substring(index + 1);

                    int intBauds;

                    if (!int.TryParse(portBauds, out intBauds))
                    {
                        Console.WriteLine("Bad port data");
                        Server.Send(e.ClientId, $"BadData:{data}");
                        return;
                    }

                    if (client.OpenPorts.ContainsKey(portName))
                    {
                        Console.WriteLine($"Client already has port {portName} open");
                        Server.Send(e.ClientId, $"AlreadyOpen:{portName}");
                        return;
                    }

                    Console.WriteLine($"Opening port {portName} at {intBauds} bauds");
                    
                    try
                    {
                        SerialDevice sport = new SerialDevice(portName, intBauds);
                        sport.DataReceived += Port_DataReceived;
                        sport.Closed += Port_Closed;
                        sport.Open();

                        client.OpenPorts[portName] = sport;

                        OpenPort opt = new OpenPort { Client = client, Port = sport, Name = portName };
                        openPorts[portName] = opt;

                        Console.WriteLine($"Port {portName} open");
                        Server.Send(e.ClientId, $"PortOpen:{portName}");
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine($"Error opening port {portName} {ex.Message} {ex.StackTrace}");
                        Server.Send(e.ClientId, $"OpenError:{portName}");
                    }

                    break;

                case "ClosePort":

                    if (!client.OpenPorts.ContainsKey(data))
                    {
                        Console.WriteLine($"Client does not have port {data} open");
                        Server.Send(e.ClientId, $"NotOpen:{data}");
                        break;
                    }

                    SerialDevice pt;

                    if (!client.OpenPorts.TryRemove(data, out pt))
                    {
                        Console.WriteLine($"Cannot close port {data}!");
                        Server.Send(e.ClientId, $"ErrorClosing:{data}");
                        break;
                    }

                    pt.Close();
                    pt.Closed -= Port_Closed;
                    pt.DataReceived -= Port_DataReceived;
                    openPorts.TryRemove(data, out var dummy);

                    Console.WriteLine($"Port {data} closed");
                    Server.Send(e.ClientId, $"PortClosed:{data}");
                    break;

                case "SendData":

                    int posPort = data.IndexOf(",");

                    if (posPort == -1)
                    {
                        Console.WriteLine($"No port found on the message");
                        Server.Send(e.ClientId, $"NoPort:{data}");
                        break;
                    }

                    string port = data.Substring(0, posPort);
                    string realData = data.Substring(posPort + 1);

                    SerialDevice tpt;

                    if (!client.OpenPorts.TryGetValue(port, out tpt))
                    {
                        Console.WriteLine($"Port {port} is not open on this connection");
                        Server.Send(e.ClientId, $"NotOpen:{port}");
                        break;
                    }

                    if(tpt.Write(Convert.FromBase64String(realData)))
                        Server.Send(e.ClientId, $"DataSent:{port}");
                    else
                        Server.Send(e.ClientId, $"SendError:{port}");

                    break;

                default:

                    Console.WriteLine($"Unknown command {command}");
                    Server.Send(e.ClientId, $"Unknown:{command}");
                    break;
            }
        }

        private static void Port_DataReceived(object sender, SerialDeviceReceiveArgs e)
        {
            var port = sender as SerialDevice;

            string data = Convert.ToBase64String(e.Data);

            //Console.WriteLine($"Received data {data} from port {port.Name}");

            if (openPorts.TryGetValue(port.Name, out var op))
            {
                //Console.WriteLine($"Sending data to client {op.Client.Id}");
                Server.Send(op.Client.Id, $"PortData:{port.Name},{data}");
            }
            
        }

        private static void Port_Closed(object sender, EventArgs e)
        {
            var port = sender as SerialDevice;
            Console.WriteLine($"Port {port.Name} was closed");
            if (openPorts.TryRemove(port.Name, out var connected))
            {
                connected.Client.OpenPorts.TryRemove(port.Name, out var pt);
                Console.WriteLine($"Port {port.Name} disconnected from client ID {connected.Client.Id}");
                Server.Send(connected.Client.Id, $"PortClosed:{port.Name}");
            }
            else
            {
                Console.WriteLine($"Port {port.Name} was not connected to any client!");
            }

        }
        
        private static void Server_ClientDisconnected(object sender, ConnectedClientEventArgs e)
        {
            connectedClients.TryRemove(e.ClientId, out var client);
            Console.WriteLine($"Client with ID {e.ClientId} disconnected from {e.ClientIP}");

            foreach (var port in client.OpenPorts)
            {
                try
                {
                    Console.WriteLine($"Closing port {port.Key}");
                    port.Value.Close();
                    Console.WriteLine($"Port {port.Key} closed");
                }
                catch { Console.WriteLine($"Error closing port {port.Key}"); }
            }
        }

        private static void Server_ClientConnected(object sender, ConnectedClientEventArgs e)
        {
            ConnectedClient c = new ConnectedClient { Id = e.ClientId };
            connectedClients[e.ClientId] = c;
            Console.WriteLine($"Client with ID {e.ClientId} connected from {e.ClientIP}");

        }

        class ConnectedClient
        {
            public Guid Id { get; set; }
            public ConcurrentDictionary<string, SerialDevice> OpenPorts { get; } = new ConcurrentDictionary<string, SerialDevice>();
        }

        class OpenPort
        {
            public string Name { get; set; }
            public SerialDevice Port { get; set; }
            public ConnectedClient Client { get; set; }
        }
    }
}
