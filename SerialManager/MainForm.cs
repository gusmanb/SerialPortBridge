using Com0Com;
using SerialManager.Classes;
using SerialManager.Controls;
using SerialManager.Managers;
using SerialManager.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SerialManager
{
    public partial class MainForm : Form
    {
        BridgeManager bridgeManager;
        Configuration config;
        PortPairManager manager;
        BridgeStatus bridgeSelected;

        public MainForm()
        {
            InitializeComponent();
            config = Configuration.Load();

            ApplyConfig();
            
        }

        private void ApplyConfig()
        {

            txtServer.Text = config.Server?.ToString() ?? "";
            txtPort.Text = config.Port != 0 ? config.Port.ToString() : "";
            txtCom0com.Text = config.Com0comPath ?? "";

            if (bridgeManager != null)
                bridgeManager.Dispose();

            bridgeManager = null;
            manager = null;

            if (config.Server != null)
            {
                bridgeManager = new BridgeManager(config.Server, config.Port);
                manager = new PortPairManager(config.Com0comPath);

                foreach (var bridge in config.Bridges)
                {
                    var brd = bridgeManager.AddBridge(bridge.LocalPort, bridge.RemotePort, bridge.Bauds);
                    brd.Selected += Bridge_Selected;
                    pnlPorts.Controls.Add(brd);
                }

                RefreshPairManager();
                RefreshBridges();

            }
        }

        private void RefreshPairManager()
        {
            RefreshPortPairs();
            RefreshFreePorts();
        }
        
        private void btnRefreshFreePorts_Click(object sender, EventArgs e)
        {
            RefreshFreePorts();
        }

        private void RefreshFreePorts()
        {

            if (manager == null)
            {
                MessageBox.Show("System not configured");
                return;
            }

            var free = manager.FreePorts;
            cbInputPort.DataSource = null;
            cbOutputPort.DataSource = null;
            cbInputPort.DataSource = free.ToArray();
            cbOutputPort.DataSource = free.ToArray();
        }

        private void RefreshPortPairs()
        {
            if (manager == null)
            {
                MessageBox.Show("System not configured");
                return;
            }

            var pairs = manager.PortPairs;
            lstPortPairs.DataSource = null;
            lstPortPairs.DataSource = pairs;
        }

        private void btnDeletePair_Click(object sender, EventArgs e)
        {

            if (manager == null)
            {
                MessageBox.Show("System not configured");
                return;
            }

            if (lstPortPairs.SelectedItem == null)
            {
                MessageBox.Show("Debes seleccionar un par a eliminar");
                return;
            }

            var pair = lstPortPairs.SelectedItem as CrossoverPortPair;
            if(!manager.DeletePair(pair.PairNumber))
            {
                MessageBox.Show("No se pudo eliminar el par, psoiblemente se encuentre en uso");
                return;
            }

            RefreshPairManager();
        }

        private void btnCreatePair_Click(object sender, EventArgs e)
        {
            if (manager == null)
            {
                MessageBox.Show("System not configured");
                return;
            }

            if (cbInputPort.SelectedItem == null || cbOutputPort.SelectedItem == null)
            {
                MessageBox.Show("Select Input/Output ports");
                return;
            }

            string inp = cbInputPort.SelectedItem.ToString();
            string outp = cbOutputPort.SelectedItem.ToString();

            if (inp == outp)
            {
                MessageBox.Show("Input and output port cannot be the same");
                return;
            }

            if (!manager.CreatePair(inp, outp))
            {
                MessageBox.Show("Cannot create pair");
                return;
            }

            RefreshPairManager();
        }

        private async void btnRefreshBridge_Click(object sender, EventArgs e)
        {

            if (bridgeManager == null)
            {
                MessageBox.Show("System not configured");
                return;
            }

            btnRefreshBridge.Enabled = false;
            await RefreshBridges();
            btnRefreshBridge.Enabled = true;
        }

        async Task<bool> RefreshBridges()
        {
            var pairs = manager.PortPairs;

            cbLocal.DataSource = pairs.Where(p => !config.LocalPortUsed(p.PortNameB)).Select(p => p.PortNameB).ToArray();

            TCPSerialClient client = new TCPSerialClient(config.Server, config.Port);
            if (!await client.Open())
                return false;
            else
            {
                TaskCompletionSource<bool> taskSource = new TaskCompletionSource<bool>();

                client.Message += (o, e) => 
                {
                    if(e.Message.Command == "AvailablePorts")
                    {
                        BeginInvoke((MethodInvoker)(() => cbRemote.DataSource = e.Message.Parameters.Where(c => !config.RemotePortUsed(c)).ToArray() ));
                        taskSource.SetResult(true);
                        client.Close();
                        client.Dispose();
                    }
                    else
                    {
                        taskSource.SetResult(false);
                        client.Close();
                        client.Dispose();
                    }
                };

                client.Disconnected += (o, e) =>
                {
                    taskSource.SetResult(false);
                    client.Close();
                    client.Dispose();
                };

                if (!await client.Send(new SerialMessage("ListPorts", null)))
                    return false;

                return await taskSource.Task;

            }
        }

        private void btnAddBridge_Click(object sender, EventArgs e)
        {
            if (cbLocal.SelectedItem == null || cbRemote.SelectedItem == null)
            {
                MessageBox.Show("Local/remote port not selected");
                return;
            }

            int bauds;

            if (string.IsNullOrWhiteSpace(cbBauds.Text) || !int.TryParse(cbBauds.Text, out bauds))
            {
                MessageBox.Show("Incorrect bauds");
                return;
            }

            var bridge = bridgeManager.AddBridge(cbLocal.SelectedItem.ToString(), cbRemote.SelectedItem.ToString(), bauds);

            if (bridge == null)
            {
                MessageBox.Show("Cannot add bridge, refresh available ports and try again");
                return;
            }

            pnlPorts.Controls.Add(bridge);
            bridge.Selected += Bridge_Selected;
            config.AddBridge(cbLocal.SelectedItem.ToString(), cbRemote.SelectedItem.ToString(), bauds);
            config.Save();
            RefreshBridges();
        }

        private void Bridge_Selected(object sender, EventArgs e)
        {
            foreach (var control in pnlPorts.Controls)
            {
                if (control != sender)
                    (control as BridgeStatus).Deselect();
            }

            bridgeSelected = sender as BridgeStatus;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            IPAddress server;
            int port;

            if (!IPAddress.TryParse(txtServer.Text, out server))
            {
                MessageBox.Show("Invalid server address");
                return;
            }

            if (!int.TryParse(txtPort.Text, out port) || port < 1)
            {
                MessageBox.Show("Invalid port");
                return;
            }

            if (!string.IsNullOrWhiteSpace(txtCom0com.Text) && !Directory.Exists(txtCom0com.Text))
            {
                MessageBox.Show("Invalid Com0com path");
                return;
            }

            config.Server = server;
            config.Port = port;
            config.Com0comPath = txtCom0com.Text;
            config.Save();

            ApplyConfig();
        }

        private void btnRemoveBridge_Click(object sender, EventArgs e)
        {
            if(bridgeSelected == null)
            {
                MessageBox.Show("No bridge selected");
                return;
            }

            if (bridgeManager.RemoveBridge(bridgeSelected.PortBName))
            {
                pnlPorts.Controls.Remove(bridgeSelected);
                config.RemoveBridge(bridgeSelected.PortBName);
                config.Save();
                RefreshBridges();
            }
        }
    }
}
