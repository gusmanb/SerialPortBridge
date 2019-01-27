using SerialManager.Classes;
using SerialManager.Controls;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SerialManager.Managers
{
    public class BridgeManager : IDisposable
    {
        ConcurrentDictionary<string, BridgeData> bridges = new ConcurrentDictionary<string, BridgeData>();

        IPAddress server;
        int port;

        public BridgeManager(IPAddress Server, int Port)
        {
            server = Server;
            port = Port;
        }

        public BridgeStatus AddBridge(string LocalPort, string RemotePort, int Bauds)
        {
            if (bridges.ContainsKey(RemotePort))
                return null;

            PortBridge bridge = new PortBridge(RemotePort, LocalPort, Bauds, server, port);
            bridge.StateChanged += Bridge_StateChanged;
            bridge.Activity += Bridge_Activity;

            BridgeStatus status = new BridgeStatus();
            status.PortAName = LocalPort;
            status.PortBName = RemotePort;

            BridgeData data = new BridgeData { Bridge = bridge, Status = status };

            bridges[RemotePort] = data;

            bridge.Start();

            return status;
        }

        public bool RemoveBridge(string RemotePort)
        {
            if (!bridges.TryRemove(RemotePort, out var data))
                return false;

            data.Bridge.Activity -= Bridge_Activity;
            data.Bridge.StateChanged -= Bridge_StateChanged;
            data.Bridge.Dispose();

            return true;
        }

        private void Bridge_Activity(object sender, ActivityEventArgs e)
        {
            BridgeData bridge;

            if (!bridges.TryGetValue(e.RemotePort, out bridge))
                return;


            if (e.Local)
            {
                if (bridges[e.RemotePort].Status.HasActivityA)
                    return;
            }
            else
            {
                if (bridges[e.RemotePort].Status.HasActivityB)
                    return;
            }

            bridge.Status.BeginInvoke((MethodInvoker)(() =>
            {
                if (e.Local)
                    bridges[e.RemotePort].Status.ActivityA();
                else
                    bridges[e.RemotePort].Status.ActivityB();
            }));

        }

        private void Bridge_StateChanged(object sender, StatusEventArgs e)
        {
            BridgeData bridge;

            if (!bridges.TryGetValue(e.RemotePort, out bridge))
                return;

            bridge.Status.BeginInvoke((MethodInvoker)(() =>
            {

                if (e.Status == BridgeState.Ready)
                    bridges[e.RemotePort].Status.Connected = true;
                else
                    bridges[e.RemotePort].Status.Connected = false;
            }));
        }

        public void Dispose()
        {
            foreach (var data in bridges.Values)
            {
                data.Bridge.Dispose();

                data.Status.BeginInvoke((MethodInvoker)(() => 
                {
                    data.Status.Parent.Controls.Remove(data.Status);
                    data.Status.Dispose();
                }));
            }

            bridges.Clear();
        }

        class BridgeData
        {
            public PortBridge Bridge { get; set; }
            public BridgeStatus Status { get; set; }
        }
    }
}
