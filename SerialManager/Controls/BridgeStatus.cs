using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace SerialManager.Controls
{
    public partial class BridgeStatus : UserControl
    {
        PictureBox[] actA;
        PictureBox[] actB;

        int currentIndexA = -1;
        int currentIndexB = -1;

        bool connected = false;

        public event EventHandler Selected;

        public bool Connected
        {
            get { return connected; }
            set
            {
                if (value == connected)
                    return;

                connected = value;

                StopA();
                StopB();

                if (connected)
                {
                    pnlConnected.Visible = true;
                    pbDisconnected.Visible = false;
                }
                else
                {
                    pnlConnected.Visible = false;
                    pbDisconnected.Visible = true;
                }
            }
        }

        System.Windows.Forms.Timer endActivityA;
        System.Windows.Forms.Timer endActivityB;

        System.Windows.Forms.Timer activityA;
        System.Windows.Forms.Timer activityB;

        public bool HasActivityA { get { return activityA != null; } }
        public bool HasActivityB { get { return activityB != null; } }

        public string PortAName { get { return label1.Text; } set { label1.Text = value; } }
        public string PortBName { get { return label2.Text; } set { label2.Text = value; } }

        object locker = new object();

        public BridgeStatus()
        {
            InitializeComponent();
            actA = new PictureBox[] { pbActPortA1, pbActPortA2, pbActPortA3 };
            actB = new PictureBox[] { pbActPortB1, pbActPortB2, pbActPortB3 };
        }

        public void Deselect()
        {
            BackColor = Color.Transparent;
        }

        public void ActivityA()
        {
            if (!connected)
                return;

            if (endActivityA != null)
            {
                endActivityA.Stop();
                endActivityA.Start();
            }
            else
            {
                endActivityA = new System.Windows.Forms.Timer();
                endActivityA.Interval = 1000;
                endActivityA.Tick += (o, e) =>
                {
                    StopA();
                };

                activityA = new System.Windows.Forms.Timer();
                activityA.Interval = 75;
                activityA.Tick += (o, e) =>
                {
                    currentIndexA++;
                    switch (currentIndexA)
                    {
                        case 0:

                            actA[0].Visible = true;
                            break;

                        case 1:
                        case 2:

                            actA[currentIndexA - 1].Visible = false;
                            actA[currentIndexA].Visible = true;

                            break;

                        case 3:
                            currentIndexA = -1;
                            actA[0].Visible = false;
                            actA[1].Visible = false;
                            actA[2].Visible = false;
                            break;
                    }
                };

                activityA.Start();
                endActivityA.Start();

            }

        }

        private void StopA()
        {
            try
            {
                activityA.Stop();
                endActivityA.Stop();
                currentIndexA = -1;

                actA[0].Visible = false;
                actA[1].Visible = false;
                actA[2].Visible = false;

                endActivityA.Dispose();
                endActivityA = null;
                activityA.Dispose();
                activityA = null;
            }
            catch { }
        }

        public void ActivityB()
        {
            if (!connected)
                return;

            if (endActivityB != null)
            {
                endActivityB.Stop();
                endActivityB.Start();
            }
            else
            {
                endActivityB = new System.Windows.Forms.Timer();
                endActivityB.Interval = 1000;
                endActivityB.Tick += (o, e) =>
                {
                    StopB();
                };

                activityB = new System.Windows.Forms.Timer();
                activityB.Interval = 75;
                activityB.Tick += (o, e) =>
                {
                    currentIndexB++;
                    switch (currentIndexB)
                    {
                        case 0:

                            actB[0].Visible = true;
                            break;

                        case 1:
                        case 2:

                            actB[currentIndexB - 1].Visible = false;
                            actB[currentIndexB].Visible = true;

                            break;

                        case 3:
                            currentIndexB = -1;
                            actB[0].Visible = false;
                            actB[1].Visible = false;
                            actB[2].Visible = false;
                            break;
                    }
                };

                activityB.Start();
                endActivityB.Start();

            }

        }

        private void StopB()
        {
            try
            {
                activityB.Stop();
                endActivityB.Stop();
                currentIndexB = -1;

                actB[0].Visible = false;
                actB[1].Visible = false;
                actB[2].Visible = false;

                endActivityB.Dispose();
                endActivityB = null;
                activityB.Dispose();
                activityB = null;
            }
            catch { }
        }

        public new void Dispose()
        {
            StopA();
            StopB();
            base.Dispose();
        }

        private void Control_Click(object sender, EventArgs e)
        {
            Select();
            this.BackColor = SystemColors.ControlLight;

            if (Selected != null)
                Selected(this, EventArgs.Empty);
        }
    }
}
