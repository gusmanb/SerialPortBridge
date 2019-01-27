using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TCPSerialClient.Controls;

namespace TCPSerialClient
{
    public partial class MainForm : Form
    {
        PortManager manager;
        bool connecting = false;
        bool connected = false;

        Dictionary<string, PortPair> portControls = new Dictionary<string, PortPair>();

        List<string> availableRemotePorts = new List<string>();
        List<string> availableLocalPorts = new List<string>();

        public MainForm()
        {
            InitializeComponent();
        }

        private async void btnConnect_Click(object sender, EventArgs e)
        {
            if (connecting || connected)
                return;

            gbServer.Enabled = false;
            connecting = true;

            manager = new PortManager(IPAddress.Parse(txtServerAddress.Text), 9025);
            manager.Disconnected += Manager_Disconnected;
            manager.PortReady += Manager_PortReady;
            manager.PortUnready += Manager_PortUnready;
            manager.LocalPortActivity += Manager_LocalPortActivity;
            manager.RemotePortActivity += Manager_RemotePortActivity;
            var res = await manager.Open();
            
            if(!res)
            {
                connecting = false;
                gbServer.Enabled = true;
                return;
            }
            
            availableRemotePorts.AddRange((await manager.ListRemotePorts()).OrderBy(p => p));
            availableLocalPorts.AddRange(manager.ListLocalFreePorts());

            cbInPort.DataSource = availableLocalPorts.ToArray();
            cbOutPort.DataSource = availableLocalPorts.ToArray();
            cbRemotePort.DataSource = availableRemotePorts.ToArray();

            gbAddPort.Enabled = true;
            connected = true;
        }

        private void Manager_RemotePortActivity(object sender, PortEventArgs e)
        {
            if (!portControls.TryGetValue(e.RemotePortName, out var port))
                return;

            BeginInvoke((MethodInvoker)(() => port.ActivityB()));
        }

        private void Manager_LocalPortActivity(object sender, PortEventArgs e)
        {
            if (!portControls.TryGetValue(e.RemotePortName, out var port))
                return;

            BeginInvoke((MethodInvoker)(() => port.ActivityA()));
        }

        private void Manager_PortUnready(object sender, PortEventArgs e)
        {
            if (!portControls.TryGetValue(e.RemotePortName, out var port))
                return;

            BeginInvoke((MethodInvoker)(() => port.Connected = false));
        }

        private void Manager_PortReady(object sender, PortEventArgs e)
        {
            if (!portControls.TryGetValue(e.RemotePortName, out var port))
                return;

            BeginInvoke((MethodInvoker)(() => port.Connected = true));
        }

        private void Manager_Disconnected(object sender, EventArgs e)
        {
            BeginInvoke((MethodInvoker)(() =>
            {
                ClearControls();
                gbServer.Enabled = true;
                gbAddPort.Enabled = false;
                connected = false;
                connecting = false;

            }));

        }

        private void ClearControls()
        {
            foreach (var control in portControls.Values)
            {
                control.Connected = false;
                control.Dispose();
            }
            portControls.Clear();
            pnlPorts.Controls.Clear();
        }

        private void btnAddPort_Click(object sender, EventArgs e)
        {
            if (manager == null || !manager.IsOpen)
                return;

            var ctl = new PortPair { PortAName = cbInPort.Text, PortBName = cbRemotePort.Text };

            portControls.Add(cbRemotePort.Text, ctl);
            pnlPorts.Controls.Add(ctl);
            manager.AddPort(cbInPort.Text, cbOutPort.Text, cbRemotePort.Text, int.Parse(txtBauds.Text));
        }
    }
}
