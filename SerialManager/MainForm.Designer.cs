namespace SerialManager
{
    partial class MainForm
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnRemoveBridge = new System.Windows.Forms.Button();
            this.btnAddBridge = new System.Windows.Forms.Button();
            this.cbBauds = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnRefreshBridge = new System.Windows.Forms.Button();
            this.cbRemote = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbLocal = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.pnlPorts = new System.Windows.Forms.FlowLayoutPanel();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btnRefreshFreePorts = new System.Windows.Forms.Button();
            this.btnDeletePair = new System.Windows.Forms.Button();
            this.btnCreatePair = new System.Windows.Forms.Button();
            this.cbOutputPort = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbInputPort = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lstPortPairs = new System.Windows.Forms.ListBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtCom0com = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(270, 452);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnRemoveBridge);
            this.tabPage1.Controls.Add(this.btnAddBridge);
            this.tabPage1.Controls.Add(this.cbBauds);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.btnRefreshBridge);
            this.tabPage1.Controls.Add(this.cbRemote);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.cbLocal);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.pnlPorts);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(262, 426);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Bridges";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnRemoveBridge
            // 
            this.btnRemoveBridge.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemoveBridge.Location = new System.Drawing.Point(8, 395);
            this.btnRemoveBridge.Name = "btnRemoveBridge";
            this.btnRemoveBridge.Size = new System.Drawing.Size(246, 23);
            this.btnRemoveBridge.TabIndex = 19;
            this.btnRemoveBridge.Text = "Remove bridge";
            this.btnRemoveBridge.UseVisualStyleBackColor = true;
            this.btnRemoveBridge.Click += new System.EventHandler(this.btnRemoveBridge_Click);
            // 
            // btnAddBridge
            // 
            this.btnAddBridge.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddBridge.Location = new System.Drawing.Point(8, 366);
            this.btnAddBridge.Name = "btnAddBridge";
            this.btnAddBridge.Size = new System.Drawing.Size(246, 23);
            this.btnAddBridge.TabIndex = 18;
            this.btnAddBridge.Text = "Add bridge";
            this.btnAddBridge.UseVisualStyleBackColor = true;
            this.btnAddBridge.Click += new System.EventHandler(this.btnAddBridge_Click);
            // 
            // cbBauds
            // 
            this.cbBauds.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbBauds.FormattingEnabled = true;
            this.cbBauds.Items.AddRange(new object[] {
            "110",
            "300",
            "600",
            "1200",
            "2400",
            "4800",
            "9600",
            "14400",
            "19200",
            "28800",
            "38400",
            "56000",
            "57600",
            "115200",
            "128000",
            "153600",
            "230400",
            "250000",
            "256000",
            "460800",
            "921600"});
            this.cbBauds.Location = new System.Drawing.Point(74, 339);
            this.cbBauds.Name = "cbBauds";
            this.cbBauds.Size = new System.Drawing.Size(180, 21);
            this.cbBauds.TabIndex = 17;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 342);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(37, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "Bauds";
            // 
            // btnRefreshBridge
            // 
            this.btnRefreshBridge.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefreshBridge.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold);
            this.btnRefreshBridge.Location = new System.Drawing.Point(205, 285);
            this.btnRefreshBridge.Name = "btnRefreshBridge";
            this.btnRefreshBridge.Size = new System.Drawing.Size(49, 48);
            this.btnRefreshBridge.TabIndex = 14;
            this.btnRefreshBridge.Text = "↻";
            this.btnRefreshBridge.UseVisualStyleBackColor = true;
            this.btnRefreshBridge.Click += new System.EventHandler(this.btnRefreshBridge_Click);
            // 
            // cbRemote
            // 
            this.cbRemote.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbRemote.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRemote.FormattingEnabled = true;
            this.cbRemote.Location = new System.Drawing.Point(74, 312);
            this.cbRemote.Name = "cbRemote";
            this.cbRemote.Size = new System.Drawing.Size(125, 21);
            this.cbRemote.TabIndex = 13;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 315);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Remote port";
            // 
            // cbLocal
            // 
            this.cbLocal.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbLocal.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLocal.FormattingEnabled = true;
            this.cbLocal.Location = new System.Drawing.Point(74, 285);
            this.cbLocal.Name = "cbLocal";
            this.cbLocal.Size = new System.Drawing.Size(125, 21);
            this.cbLocal.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 288);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(54, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Local port";
            // 
            // pnlPorts
            // 
            this.pnlPorts.AutoScroll = true;
            this.pnlPorts.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.pnlPorts.Location = new System.Drawing.Point(8, 6);
            this.pnlPorts.Name = "pnlPorts";
            this.pnlPorts.Size = new System.Drawing.Size(246, 273);
            this.pnlPorts.TabIndex = 9;
            this.pnlPorts.WrapContents = false;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btnRefreshFreePorts);
            this.tabPage2.Controls.Add(this.btnDeletePair);
            this.tabPage2.Controls.Add(this.btnCreatePair);
            this.tabPage2.Controls.Add(this.cbOutputPort);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.cbInputPort);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.lstPortPairs);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(262, 426);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Port pairs";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // btnRefreshFreePorts
            // 
            this.btnRefreshFreePorts.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefreshFreePorts.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold);
            this.btnRefreshFreePorts.Location = new System.Drawing.Point(205, 312);
            this.btnRefreshFreePorts.Name = "btnRefreshFreePorts";
            this.btnRefreshFreePorts.Size = new System.Drawing.Size(49, 48);
            this.btnRefreshFreePorts.TabIndex = 8;
            this.btnRefreshFreePorts.Text = "↻";
            this.btnRefreshFreePorts.UseVisualStyleBackColor = true;
            this.btnRefreshFreePorts.Click += new System.EventHandler(this.btnRefreshFreePorts_Click);
            // 
            // btnDeletePair
            // 
            this.btnDeletePair.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeletePair.Location = new System.Drawing.Point(8, 395);
            this.btnDeletePair.Name = "btnDeletePair";
            this.btnDeletePair.Size = new System.Drawing.Size(246, 23);
            this.btnDeletePair.TabIndex = 7;
            this.btnDeletePair.Text = "Delete pair";
            this.btnDeletePair.UseVisualStyleBackColor = true;
            this.btnDeletePair.Click += new System.EventHandler(this.btnDeletePair_Click);
            // 
            // btnCreatePair
            // 
            this.btnCreatePair.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCreatePair.Location = new System.Drawing.Point(8, 366);
            this.btnCreatePair.Name = "btnCreatePair";
            this.btnCreatePair.Size = new System.Drawing.Size(246, 23);
            this.btnCreatePair.TabIndex = 6;
            this.btnCreatePair.Text = "Create pair";
            this.btnCreatePair.UseVisualStyleBackColor = true;
            this.btnCreatePair.Click += new System.EventHandler(this.btnCreatePair_Click);
            // 
            // cbOutputPort
            // 
            this.cbOutputPort.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbOutputPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbOutputPort.FormattingEnabled = true;
            this.cbOutputPort.Location = new System.Drawing.Point(74, 339);
            this.cbOutputPort.Name = "cbOutputPort";
            this.cbOutputPort.Size = new System.Drawing.Size(125, 21);
            this.cbOutputPort.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 342);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Output port";
            // 
            // cbInputPort
            // 
            this.cbInputPort.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbInputPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbInputPort.FormattingEnabled = true;
            this.cbInputPort.Location = new System.Drawing.Point(74, 312);
            this.cbInputPort.Name = "cbInputPort";
            this.cbInputPort.Size = new System.Drawing.Size(125, 21);
            this.cbInputPort.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 315);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Input port";
            // 
            // lstPortPairs
            // 
            this.lstPortPairs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstPortPairs.FormattingEnabled = true;
            this.lstPortPairs.IntegralHeight = false;
            this.lstPortPairs.Location = new System.Drawing.Point(8, 6);
            this.lstPortPairs.Name = "lstPortPairs";
            this.lstPortPairs.Size = new System.Drawing.Size(246, 300);
            this.lstPortPairs.TabIndex = 1;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.txtCom0com);
            this.tabPage3.Controls.Add(this.label8);
            this.tabPage3.Controls.Add(this.txtPort);
            this.tabPage3.Controls.Add(this.label7);
            this.tabPage3.Controls.Add(this.button1);
            this.tabPage3.Controls.Add(this.txtServer);
            this.tabPage3.Controls.Add(this.label1);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(262, 426);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Configuration";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Server";
            // 
            // txtServer
            // 
            this.txtServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtServer.Location = new System.Drawing.Point(92, 9);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(162, 20);
            this.txtServer.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(8, 395);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(246, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Save";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtPort
            // 
            this.txtPort.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPort.Location = new System.Drawing.Point(92, 35);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(162, 20);
            this.txtPort.TabIndex = 4;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 38);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(26, 13);
            this.label7.TabIndex = 3;
            this.label7.Text = "Port";
            // 
            // txtCom0com
            // 
            this.txtCom0com.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCom0com.Location = new System.Drawing.Point(92, 61);
            this.txtCom0com.Name = "txtCom0com";
            this.txtCom0com.Size = new System.Drawing.Size(162, 20);
            this.txtCom0com.TabIndex = 6;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 64);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(78, 13);
            this.label8.TabIndex = 5;
            this.label8.Text = "Com0com path";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(270, 452);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(286, 491);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(286, 491);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Serial client";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ListBox lstPortPairs;
        private System.Windows.Forms.ComboBox cbInputPort;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnDeletePair;
        private System.Windows.Forms.Button btnCreatePair;
        private System.Windows.Forms.ComboBox cbOutputPort;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnRefreshFreePorts;
        private System.Windows.Forms.Button btnRemoveBridge;
        private System.Windows.Forms.Button btnAddBridge;
        private System.Windows.Forms.ComboBox cbBauds;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnRefreshBridge;
        private System.Windows.Forms.ComboBox cbRemote;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbLocal;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.FlowLayoutPanel pnlPorts;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TextBox txtCom0com;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.Label label1;
    }
}

