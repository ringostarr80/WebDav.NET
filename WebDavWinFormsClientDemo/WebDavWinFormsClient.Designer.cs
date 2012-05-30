namespace WebDavWinFormsClientDemo
{
    partial class WebDavWinFormsClient
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
            this.components = new System.ComponentModel.Container();
            this.groupBoxLogin = new System.Windows.Forms.GroupBox();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.labelPassword = new System.Windows.Forms.Label();
            this.textBoxUsername = new System.Windows.Forms.TextBox();
            this.labelUsername = new System.Windows.Forms.Label();
            this.textBoxPath = new System.Windows.Forms.TextBox();
            this.labelPath = new System.Windows.Forms.Label();
            this.comboBoxProtocol = new System.Windows.Forms.ComboBox();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.textBoxServer = new System.Windows.Forms.TextBox();
            this.labelServer = new System.Windows.Forms.Label();
            this.labelProtocol = new System.Windows.Forms.Label();
            this.richTextBoxDebug = new System.Windows.Forms.RichTextBox();
            this.treeViewServer = new System.Windows.Forms.TreeView();
            this.listViewFiles = new System.Windows.Forms.ListView();
            this.columnHeaderFilename = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderFilesize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStripFilename = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemSaveUnder = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemUploadFile = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemCreateFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemDeleteFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemDeleteFile = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.groupBoxLogin.SuspendLayout();
            this.contextMenuStripFilename.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxLogin
            // 
            this.groupBoxLogin.Controls.Add(this.textBoxPassword);
            this.groupBoxLogin.Controls.Add(this.labelPassword);
            this.groupBoxLogin.Controls.Add(this.textBoxUsername);
            this.groupBoxLogin.Controls.Add(this.labelUsername);
            this.groupBoxLogin.Controls.Add(this.textBoxPath);
            this.groupBoxLogin.Controls.Add(this.labelPath);
            this.groupBoxLogin.Controls.Add(this.comboBoxProtocol);
            this.groupBoxLogin.Controls.Add(this.buttonConnect);
            this.groupBoxLogin.Controls.Add(this.textBoxServer);
            this.groupBoxLogin.Controls.Add(this.labelServer);
            this.groupBoxLogin.Controls.Add(this.labelProtocol);
            this.groupBoxLogin.Location = new System.Drawing.Point(12, 13);
            this.groupBoxLogin.Name = "groupBoxLogin";
            this.groupBoxLogin.Size = new System.Drawing.Size(929, 53);
            this.groupBoxLogin.TabIndex = 0;
            this.groupBoxLogin.TabStop = false;
            this.groupBoxLogin.Text = "Login:";
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Location = new System.Drawing.Point(737, 19);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.PasswordChar = '*';
            this.textBoxPassword.Size = new System.Drawing.Size(100, 20);
            this.textBoxPassword.TabIndex = 5;
            // 
            // labelPassword
            // 
            this.labelPassword.AutoSize = true;
            this.labelPassword.Location = new System.Drawing.Point(678, 22);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(53, 13);
            this.labelPassword.TabIndex = 10;
            this.labelPassword.Text = "Passwort:";
            // 
            // textBoxUsername
            // 
            this.textBoxUsername.Location = new System.Drawing.Point(572, 19);
            this.textBoxUsername.Name = "textBoxUsername";
            this.textBoxUsername.Size = new System.Drawing.Size(100, 20);
            this.textBoxUsername.TabIndex = 4;
            // 
            // labelUsername
            // 
            this.labelUsername.AutoSize = true;
            this.labelUsername.Location = new System.Drawing.Point(488, 22);
            this.labelUsername.Name = "labelUsername";
            this.labelUsername.Size = new System.Drawing.Size(78, 13);
            this.labelUsername.TabIndex = 8;
            this.labelUsername.Text = "Benutzername:";
            // 
            // textBoxPath
            // 
            this.textBoxPath.Location = new System.Drawing.Point(382, 19);
            this.textBoxPath.Name = "textBoxPath";
            this.textBoxPath.Size = new System.Drawing.Size(100, 20);
            this.textBoxPath.TabIndex = 3;
            this.textBoxPath.Text = "/";
            // 
            // labelPath
            // 
            this.labelPath.AutoSize = true;
            this.labelPath.Location = new System.Drawing.Point(344, 22);
            this.labelPath.Name = "labelPath";
            this.labelPath.Size = new System.Drawing.Size(32, 13);
            this.labelPath.TabIndex = 6;
            this.labelPath.Text = "Path:";
            // 
            // comboBoxProtocol
            // 
            this.comboBoxProtocol.Cursor = System.Windows.Forms.Cursors.Default;
            this.comboBoxProtocol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxProtocol.FormattingEnabled = true;
            this.comboBoxProtocol.Items.AddRange(new object[] {
            "http",
            "https"});
            this.comboBoxProtocol.Location = new System.Drawing.Point(64, 19);
            this.comboBoxProtocol.Name = "comboBoxProtocol";
            this.comboBoxProtocol.Size = new System.Drawing.Size(121, 21);
            this.comboBoxProtocol.TabIndex = 1;
            // 
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(843, 17);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(75, 23);
            this.buttonConnect.TabIndex = 6;
            this.buttonConnect.Text = "verbinden";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // textBoxServer
            // 
            this.textBoxServer.Location = new System.Drawing.Point(238, 19);
            this.textBoxServer.Name = "textBoxServer";
            this.textBoxServer.Size = new System.Drawing.Size(100, 20);
            this.textBoxServer.TabIndex = 2;
            this.textBoxServer.Text = "localhost";
            // 
            // labelServer
            // 
            this.labelServer.AutoSize = true;
            this.labelServer.Location = new System.Drawing.Point(191, 22);
            this.labelServer.Name = "labelServer";
            this.labelServer.Size = new System.Drawing.Size(41, 13);
            this.labelServer.TabIndex = 2;
            this.labelServer.Text = "Server:";
            // 
            // labelProtocol
            // 
            this.labelProtocol.AutoSize = true;
            this.labelProtocol.Location = new System.Drawing.Point(7, 22);
            this.labelProtocol.Name = "labelProtocol";
            this.labelProtocol.Size = new System.Drawing.Size(51, 13);
            this.labelProtocol.TabIndex = 1;
            this.labelProtocol.Text = "Protokoll:";
            // 
            // richTextBoxDebug
            // 
            this.richTextBoxDebug.Location = new System.Drawing.Point(12, 364);
            this.richTextBoxDebug.Name = "richTextBoxDebug";
            this.richTextBoxDebug.Size = new System.Drawing.Size(932, 300);
            this.richTextBoxDebug.TabIndex = 9;
            this.richTextBoxDebug.Text = "";
            // 
            // treeViewServer
            // 
            this.treeViewServer.FullRowSelect = true;
            this.treeViewServer.Location = new System.Drawing.Point(12, 72);
            this.treeViewServer.Name = "treeViewServer";
            this.treeViewServer.PathSeparator = "/";
            this.treeViewServer.Size = new System.Drawing.Size(156, 286);
            this.treeViewServer.TabIndex = 7;
            this.treeViewServer.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewServer_AfterSelect);
            this.treeViewServer.MouseClick += new System.Windows.Forms.MouseEventHandler(this.treeViewServer_MouseClick);
            // 
            // listViewFiles
            // 
            this.listViewFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderFilename,
            this.columnHeaderFilesize});
            this.listViewFiles.FullRowSelect = true;
            this.listViewFiles.Location = new System.Drawing.Point(175, 73);
            this.listViewFiles.Name = "listViewFiles";
            this.listViewFiles.Size = new System.Drawing.Size(769, 285);
            this.listViewFiles.TabIndex = 8;
            this.listViewFiles.UseCompatibleStateImageBehavior = false;
            this.listViewFiles.View = System.Windows.Forms.View.Details;
            this.listViewFiles.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listViewFiles_MouseClick);
            // 
            // columnHeaderFilename
            // 
            this.columnHeaderFilename.Text = "Dateiname";
            this.columnHeaderFilename.Width = 628;
            // 
            // columnHeaderFilesize
            // 
            this.columnHeaderFilesize.Text = "Dateigröße";
            this.columnHeaderFilesize.Width = 137;
            // 
            // contextMenuStripFilename
            // 
            this.contextMenuStripFilename.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemSaveUnder,
            this.toolStripMenuItemUploadFile,
            this.toolStripMenuItemCreateFolder,
            this.toolStripMenuItemDeleteFolder,
            this.toolStripMenuItemDeleteFile});
            this.contextMenuStripFilename.Name = "contextMenuStripFilename";
            this.contextMenuStripFilename.Size = new System.Drawing.Size(193, 114);
            this.contextMenuStripFilename.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.contextMenuStripFilename_ItemClicked);
            // 
            // toolStripMenuItemSaveUnder
            // 
            this.toolStripMenuItemSaveUnder.Name = "toolStripMenuItemSaveUnder";
            this.toolStripMenuItemSaveUnder.Size = new System.Drawing.Size(192, 22);
            this.toolStripMenuItemSaveUnder.Text = "Speichern unter ...";
            // 
            // toolStripMenuItemUploadFile
            // 
            this.toolStripMenuItemUploadFile.Name = "toolStripMenuItemUploadFile";
            this.toolStripMenuItemUploadFile.Size = new System.Drawing.Size(192, 22);
            this.toolStripMenuItemUploadFile.Text = "Datei hochladen ...";
            // 
            // toolStripMenuItemCreateFolder
            // 
            this.toolStripMenuItemCreateFolder.Name = "toolStripMenuItemCreateFolder";
            this.toolStripMenuItemCreateFolder.Size = new System.Drawing.Size(192, 22);
            this.toolStripMenuItemCreateFolder.Text = "Verzeichnis erstellen ...";
            // 
            // toolStripMenuItemDeleteFolder
            // 
            this.toolStripMenuItemDeleteFolder.Name = "toolStripMenuItemDeleteFolder";
            this.toolStripMenuItemDeleteFolder.Size = new System.Drawing.Size(192, 22);
            this.toolStripMenuItemDeleteFolder.Text = "Verzeichnis löschen";
            // 
            // toolStripMenuItemDeleteFile
            // 
            this.toolStripMenuItemDeleteFile.Name = "toolStripMenuItemDeleteFile";
            this.toolStripMenuItemDeleteFile.Size = new System.Drawing.Size(192, 22);
            this.toolStripMenuItemDeleteFile.Text = "Datei löschen";
            // 
            // WebDavWinFormsClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(956, 662);
            this.Controls.Add(this.listViewFiles);
            this.Controls.Add(this.treeViewServer);
            this.Controls.Add(this.richTextBoxDebug);
            this.Controls.Add(this.groupBoxLogin);
            this.Name = "WebDavWinFormsClient";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WebDav-Client";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBoxLogin.ResumeLayout(false);
            this.groupBoxLogin.PerformLayout();
            this.contextMenuStripFilename.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion 

        private System.Windows.Forms.GroupBox groupBoxLogin;
        private System.Windows.Forms.Label labelProtocol;
        private System.Windows.Forms.TextBox textBoxServer;
        private System.Windows.Forms.Label labelServer;
        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.TextBox textBoxUsername;
        private System.Windows.Forms.Label labelUsername;
        private System.Windows.Forms.TextBox textBoxPath;
        private System.Windows.Forms.Label labelPath;
        private System.Windows.Forms.ComboBox comboBoxProtocol;
        private System.Windows.Forms.RichTextBox richTextBoxDebug;
        private System.Windows.Forms.TreeView treeViewServer;
        private System.Windows.Forms.ListView listViewFiles;
        private System.Windows.Forms.ColumnHeader columnHeaderFilename;
        private System.Windows.Forms.ColumnHeader columnHeaderFilesize;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripFilename;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSaveUnder;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemUploadFile;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemDeleteFile;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemCreateFolder;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemDeleteFolder;
    }
}

