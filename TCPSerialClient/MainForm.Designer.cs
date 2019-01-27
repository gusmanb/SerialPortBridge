namespace TCPSerialClient
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
            this.gbServer = new System.Windows.Forms.GroupBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.txtServerAddress = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlPorts = new System.Windows.Forms.FlowLayoutPanel();
            this.gbAddPort = new System.Windows.Forms.GroupBox();
            this.btnAddPort = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtBauds = new System.Windows.Forms.TextBox();
            this.cbOutPort = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbRemotePort = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbInPort = new System.Windows.Forms.ComboBox();
            this.gbServer.SuspendLayout();
            this.gbAddPort.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbServer
            // 
            this.gbServer.Controls.Add(this.btnConnect);
            this.gbServer.Controls.Add(this.txtServerAddress);
            this.gbServer.Controls.Add(this.label1);
            this.gbServer.Location = new System.Drawing.Point(12, 12);
            this.gbServer.Name = "gbServer";
            this.gbServer.Size = new System.Drawing.Size(250, 54);
            this.gbServer.TabIndex = 7;
            this.gbServer.TabStop = false;
            this.gbServer.Text = "Server";
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(163, 17);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 4;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // txtServerAddress
            // 
            this.txtServerAddress.Location = new System.Drawing.Point(57, 19);
            this.txtServerAddress.Name = "txtServerAddress";
            this.txtServerAddress.Size = new System.Drawing.Size(100, 20);
            this.txtServerAddress.TabIndex = 1;
            this.txtServerAddress.Text = "192.168.10.24";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Address";
            // 
            // pnlPorts
            // 
            this.pnlPorts.AutoScroll = true;
            this.pnlPorts.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.pnlPorts.Location = new System.Drawing.Point(12, 72);
            this.pnlPorts.Name = "pnlPorts";
            this.pnlPorts.Size = new System.Drawing.Size(250, 249);
            this.pnlPorts.TabIndex = 8;
            this.pnlPorts.WrapContents = false;
            // 
            // gbAddPort
            // 
            this.gbAddPort.Controls.Add(this.btnAddPort);
            this.gbAddPort.Controls.Add(this.label4);
            this.gbAddPort.Controls.Add(this.txtBauds);
            this.gbAddPort.Controls.Add(this.cbOutPort);
            this.gbAddPort.Controls.Add(this.label3);
            this.gbAddPort.Controls.Add(this.cbRemotePort);
            this.gbAddPort.Controls.Add(this.label2);
            this.gbAddPort.Controls.Add(this.cbInPort);
            this.gbAddPort.Enabled = false;
            this.gbAddPort.Location = new System.Drawing.Point(12, 327);
            this.gbAddPort.Name = "gbAddPort";
            this.gbAddPort.Size = new System.Drawing.Size(250, 134);
            this.gbAddPort.TabIndex = 9;
            this.gbAddPort.TabStop = false;
            this.gbAddPort.Text = "Port";
            // 
            // btnAddPort
            // 
            this.btnAddPort.Location = new System.Drawing.Point(9, 99);
            this.btnAddPort.Name = "btnAddPort";
            this.btnAddPort.Size = new System.Drawing.Size(229, 23);
            this.btnAddPort.TabIndex = 7;
            this.btnAddPort.Text = "Add port";
            this.btnAddPort.UseVisualStyleBackColor = true;
            this.btnAddPort.Click += new System.EventHandler(this.btnAddPort_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 76);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Bauds";
            // 
            // txtBauds
            // 
            this.txtBauds.Location = new System.Drawing.Point(57, 73);
            this.txtBauds.Name = "txtBauds";
            this.txtBauds.Size = new System.Drawing.Size(181, 20);
            this.txtBauds.TabIndex = 5;
            // 
            // cbOutPort
            // 
            this.cbOutPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbOutPort.FormattingEnabled = true;
            this.cbOutPort.Location = new System.Drawing.Point(158, 19);
            this.cbOutPort.Name = "cbOutPort";
            this.cbOutPort.Size = new System.Drawing.Size(80, 21);
            this.cbOutPort.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Remote";
            // 
            // cbRemotePort
            // 
            this.cbRemotePort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRemotePort.FormattingEnabled = true;
            this.cbRemotePort.Location = new System.Drawing.Point(57, 46);
            this.cbRemotePort.Name = "cbRemotePort";
            this.cbRemotePort.Size = new System.Drawing.Size(181, 21);
            this.cbRemotePort.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Local";
            // 
            // cbInPort
            // 
            this.cbInPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbInPort.FormattingEnabled = true;
            this.cbInPort.Location = new System.Drawing.Point(57, 19);
            this.cbInPort.Name = "cbInPort";
            this.cbInPort.Size = new System.Drawing.Size(80, 21);
            this.cbInPort.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(276, 469);
            this.Controls.Add(this.gbAddPort);
            this.Controls.Add(this.pnlPorts);
            this.Controls.Add(this.gbServer);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.gbServer.ResumeLayout(false);
            this.gbServer.PerformLayout();
            this.gbAddPort.ResumeLayout(false);
            this.gbAddPort.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox gbServer;
        private System.Windows.Forms.TextBox txtServerAddress;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.FlowLayoutPanel pnlPorts;
        private System.Windows.Forms.GroupBox gbAddPort;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbRemotePort;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbInPort;
        private System.Windows.Forms.Button btnAddPort;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtBauds;
        private System.Windows.Forms.ComboBox cbOutPort;
    }
}

