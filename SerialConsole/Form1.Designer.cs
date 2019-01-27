namespace SerialConsole
{
    partial class Form1
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
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.txtInput = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtOutput
            // 
            this.txtOutput.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.txtOutput.Location = new System.Drawing.Point(12, 12);
            this.txtOutput.Multiline = true;
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.ReadOnly = true;
            this.txtOutput.Size = new System.Drawing.Size(585, 371);
            this.txtOutput.TabIndex = 0;
            // 
            // txtInput
            // 
            this.txtInput.Location = new System.Drawing.Point(12, 389);
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new System.Drawing.Size(585, 20);
            this.txtInput.TabIndex = 1;
            this.txtInput.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtInput_KeyUp);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(610, 422);
            this.Controls.Add(this.txtInput);
            this.Controls.Add(this.txtOutput);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtOutput;
        private System.Windows.Forms.TextBox txtInput;
    }
}

