﻿namespace SerialManager.Controls
{
    partial class BridgeStatus
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

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pnlConnected = new System.Windows.Forms.Panel();
            this.pbActPortA3 = new System.Windows.Forms.PictureBox();
            this.pbActPortA2 = new System.Windows.Forms.PictureBox();
            this.pbActPortB3 = new System.Windows.Forms.PictureBox();
            this.pbActPortB2 = new System.Windows.Forms.PictureBox();
            this.pbPortB = new System.Windows.Forms.PictureBox();
            this.pbActPortB1 = new System.Windows.Forms.PictureBox();
            this.pbActPortA1 = new System.Windows.Forms.PictureBox();
            this.pbPortA = new System.Windows.Forms.PictureBox();
            this.pbDisconnected = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbActPortA3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbActPortA2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbActPortB3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbActPortB2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPortB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbActPortB1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbActPortA1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPortA)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbDisconnected)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(4, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 18);
            this.label1.TabIndex = 6;
            this.label1.Text = "ttyUSB0";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label1.Click += new System.EventHandler(this.Control_Click);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(148, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 18);
            this.label2.TabIndex = 7;
            this.label2.Text = "ttyUSB0";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label2.Click += new System.EventHandler(this.Control_Click);
            // 
            // pnlConnected
            // 
            this.pnlConnected.BackColor = System.Drawing.Color.Black;
            this.pnlConnected.Location = new System.Drawing.Point(34, 19);
            this.pnlConnected.Name = "pnlConnected";
            this.pnlConnected.Size = new System.Drawing.Size(150, 2);
            this.pnlConnected.TabIndex = 14;
            this.pnlConnected.Visible = false;
            this.pnlConnected.Click += new System.EventHandler(this.Control_Click);
            // 
            // pbActPortA3
            // 
            this.pbActPortA3.Image = global::SerialManager.Properties.Resources._10101;
            this.pbActPortA3.Location = new System.Drawing.Point(127, 9);
            this.pbActPortA3.Name = "pbActPortA3";
            this.pbActPortA3.Size = new System.Drawing.Size(24, 8);
            this.pbActPortA3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbActPortA3.TabIndex = 13;
            this.pbActPortA3.TabStop = false;
            this.pbActPortA3.Visible = false;
            this.pbActPortA3.Click += new System.EventHandler(this.Control_Click);
            // 
            // pbActPortA2
            // 
            this.pbActPortA2.Image = global::SerialManager.Properties.Resources._10101;
            this.pbActPortA2.Location = new System.Drawing.Point(101, 9);
            this.pbActPortA2.Name = "pbActPortA2";
            this.pbActPortA2.Size = new System.Drawing.Size(24, 8);
            this.pbActPortA2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbActPortA2.TabIndex = 12;
            this.pbActPortA2.TabStop = false;
            this.pbActPortA2.Visible = false;
            this.pbActPortA2.Click += new System.EventHandler(this.Control_Click);
            // 
            // pbActPortB3
            // 
            this.pbActPortB3.Image = global::SerialManager.Properties.Resources._10101;
            this.pbActPortB3.Location = new System.Drawing.Point(68, 23);
            this.pbActPortB3.Name = "pbActPortB3";
            this.pbActPortB3.Size = new System.Drawing.Size(24, 8);
            this.pbActPortB3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbActPortB3.TabIndex = 11;
            this.pbActPortB3.TabStop = false;
            this.pbActPortB3.Visible = false;
            this.pbActPortB3.Click += new System.EventHandler(this.Control_Click);
            // 
            // pbActPortB2
            // 
            this.pbActPortB2.Image = global::SerialManager.Properties.Resources._10101;
            this.pbActPortB2.Location = new System.Drawing.Point(94, 23);
            this.pbActPortB2.Name = "pbActPortB2";
            this.pbActPortB2.Size = new System.Drawing.Size(24, 8);
            this.pbActPortB2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbActPortB2.TabIndex = 9;
            this.pbActPortB2.TabStop = false;
            this.pbActPortB2.Visible = false;
            this.pbActPortB2.Click += new System.EventHandler(this.Control_Click);
            // 
            // pbPortB
            // 
            this.pbPortB.Image = global::SerialManager.Properties.Resources.remote_serial_32;
            this.pbPortB.Location = new System.Drawing.Point(165, 4);
            this.pbPortB.Margin = new System.Windows.Forms.Padding(12);
            this.pbPortB.Name = "pbPortB";
            this.pbPortB.Size = new System.Drawing.Size(32, 32);
            this.pbPortB.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbPortB.TabIndex = 8;
            this.pbPortB.TabStop = false;
            this.pbPortB.Click += new System.EventHandler(this.Control_Click);
            // 
            // pbActPortB1
            // 
            this.pbActPortB1.Image = global::SerialManager.Properties.Resources._10101;
            this.pbActPortB1.Location = new System.Drawing.Point(120, 23);
            this.pbActPortB1.Name = "pbActPortB1";
            this.pbActPortB1.Size = new System.Drawing.Size(24, 8);
            this.pbActPortB1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbActPortB1.TabIndex = 5;
            this.pbActPortB1.TabStop = false;
            this.pbActPortB1.Visible = false;
            this.pbActPortB1.Click += new System.EventHandler(this.Control_Click);
            // 
            // pbActPortA1
            // 
            this.pbActPortA1.Image = global::SerialManager.Properties.Resources._10101;
            this.pbActPortA1.Location = new System.Drawing.Point(75, 9);
            this.pbActPortA1.Name = "pbActPortA1";
            this.pbActPortA1.Size = new System.Drawing.Size(24, 8);
            this.pbActPortA1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbActPortA1.TabIndex = 3;
            this.pbActPortA1.TabStop = false;
            this.pbActPortA1.Visible = false;
            this.pbActPortA1.Click += new System.EventHandler(this.Control_Click);
            // 
            // pbPortA
            // 
            this.pbPortA.Image = global::SerialManager.Properties.Resources.linked_serial_32;
            this.pbPortA.Location = new System.Drawing.Point(21, 4);
            this.pbPortA.Margin = new System.Windows.Forms.Padding(12);
            this.pbPortA.Name = "pbPortA";
            this.pbPortA.Size = new System.Drawing.Size(32, 32);
            this.pbPortA.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbPortA.TabIndex = 0;
            this.pbPortA.TabStop = false;
            this.pbPortA.Click += new System.EventHandler(this.Control_Click);
            // 
            // pbDisconnected
            // 
            this.pbDisconnected.Image = global::SerialManager.Properties.Resources.Disconnected;
            this.pbDisconnected.Location = new System.Drawing.Point(55, 14);
            this.pbDisconnected.Name = "pbDisconnected";
            this.pbDisconnected.Size = new System.Drawing.Size(108, 12);
            this.pbDisconnected.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbDisconnected.TabIndex = 15;
            this.pbDisconnected.TabStop = false;
            this.pbDisconnected.Click += new System.EventHandler(this.Control_Click);
            // 
            // BridgeStatus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pbActPortA3);
            this.Controls.Add(this.pbActPortA2);
            this.Controls.Add(this.pbActPortB3);
            this.Controls.Add(this.pbActPortB2);
            this.Controls.Add(this.pbPortB);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pbActPortB1);
            this.Controls.Add(this.pbActPortA1);
            this.Controls.Add(this.pbPortA);
            this.Controls.Add(this.pnlConnected);
            this.Controls.Add(this.pbDisconnected);
            this.MaximumSize = new System.Drawing.Size(216, 58);
            this.MinimumSize = new System.Drawing.Size(216, 58);
            this.Name = "BridgeStatus";
            this.Size = new System.Drawing.Size(216, 58);
            this.Click += new System.EventHandler(this.Control_Click);
            ((System.ComponentModel.ISupportInitialize)(this.pbActPortA3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbActPortA2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbActPortB3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbActPortB2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPortB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbActPortB1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbActPortA1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPortA)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbDisconnected)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbPortA;
        private System.Windows.Forms.PictureBox pbActPortA1;
        private System.Windows.Forms.PictureBox pbActPortB1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pbPortB;
        private System.Windows.Forms.PictureBox pbActPortB2;
        private System.Windows.Forms.PictureBox pbActPortB3;
        private System.Windows.Forms.PictureBox pbActPortA2;
        private System.Windows.Forms.PictureBox pbActPortA3;
        private System.Windows.Forms.Panel pnlConnected;
        private System.Windows.Forms.PictureBox pbDisconnected;
    }
}
