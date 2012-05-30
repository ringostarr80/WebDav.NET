namespace WebDavWinFormsClientDemo
{
    partial class DownloadForm
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.labelFileDownload = new System.Windows.Forms.Label();
            this.progressBarFileDownload = new System.Windows.Forms.ProgressBar();
            this.labelPercentage = new System.Windows.Forms.Label();
            this.buttonReady = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelFileDownload
            // 
            this.labelFileDownload.AutoSize = true;
            this.labelFileDownload.Location = new System.Drawing.Point(12, 9);
            this.labelFileDownload.Name = "labelFileDownload";
            this.labelFileDownload.Size = new System.Drawing.Size(125, 13);
            this.labelFileDownload.TabIndex = 0;
            this.labelFileDownload.Text = "Datei wird runtergeladen:";
            // 
            // progressBarFileDownload
            // 
            this.progressBarFileDownload.Location = new System.Drawing.Point(12, 25);
            this.progressBarFileDownload.Name = "progressBarFileDownload";
            this.progressBarFileDownload.Size = new System.Drawing.Size(531, 23);
            this.progressBarFileDownload.TabIndex = 1;
            // 
            // labelPercentage
            // 
            this.labelPercentage.AutoSize = true;
            this.labelPercentage.Location = new System.Drawing.Point(549, 30);
            this.labelPercentage.Name = "labelPercentage";
            this.labelPercentage.Size = new System.Drawing.Size(21, 13);
            this.labelPercentage.TabIndex = 2;
            this.labelPercentage.Text = "0%";
            // 
            // buttonReady
            // 
            this.buttonReady.Location = new System.Drawing.Point(196, 75);
            this.buttonReady.Name = "buttonReady";
            this.buttonReady.Size = new System.Drawing.Size(75, 23);
            this.buttonReady.TabIndex = 3;
            this.buttonReady.Text = "Fertig";
            this.buttonReady.UseVisualStyleBackColor = true;
            this.buttonReady.Click += new System.EventHandler(this.buttonReady_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(323, 74);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "Abbrechen";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // DownloadForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(594, 122);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonReady);
            this.Controls.Add(this.labelPercentage);
            this.Controls.Add(this.progressBarFileDownload);
            this.Controls.Add(this.labelFileDownload);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DownloadForm";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Download";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DownloadForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelFileDownload;
        private System.Windows.Forms.ProgressBar progressBarFileDownload;
        private System.Windows.Forms.Label labelPercentage;
        private System.Windows.Forms.Button buttonReady;
        private System.Windows.Forms.Button buttonCancel;
    }
}