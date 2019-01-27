using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SerialConsole
{
    public partial class Form1 : Form
    {
        StreamReader ClientReader;
        StreamWriter ClientWriter;
        NetworkStream ClientStream;

        public Form1()
        {
            InitializeComponent();
        }
   
        private async void HandleClient(TcpClient res)
        {
            ClientStream = res.GetStream();
            ClientReader = new StreamReader(ClientStream, Encoding.UTF8);
            
            try
            {
                while (true)
                {
                    var line = await ClientReader.ReadLineAsync().ConfigureAwait(false);

                    if (string.IsNullOrWhiteSpace(line))
                    {
                        BeginInvoke((MethodInvoker)(() => { txtOutput.AppendText($"Disconnected\r\n"); }));
                        return;
                    }
                    else
                    {
                        try
                        {
                            BeginInvoke((MethodInvoker)(() => { txtOutput.AppendText($"Received: {line}\r\n"); }));
                        }
                        catch { }
                    }
                }
            }
            catch
            {
                BeginInvoke((MethodInvoker)(() => { txtOutput.AppendText($"Disconnected\r\n"); }));
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            TcpClient client = new TcpClient();
            client.Connect("192.168.10.24", 9025);
            HandleClient(client);
        }

        private void txtInput_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Return)
            {
                e.Handled = true;
                byte[] data = Encoding.UTF8.GetBytes(txtInput.Text + "\r\n");
                ClientStream.Write(data, 0, data.Length);
                txtOutput.AppendText($"Sent: {txtInput.Text}\r\n");
                txtInput.Text = "";
            }
        }
    }
}
