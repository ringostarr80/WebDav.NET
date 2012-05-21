using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using WebDav.Client;
using WebDav.Client.Exceptions;

namespace WebDavWinFormsClientDemo
{
    public partial class WebDavWinFormsClient : Form
    {
        private string _protocol = "";
        private string _domain = "";
        private string _path = "";
        private string _username = "";
        private string _password = "";

        public WebDavWinFormsClient() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
            comboBoxProtocol.SelectedIndex = 0;
        }

        private string GetSelectedFolder() {
            string selectedFolder = this._path;
            if (treeViewServer.SelectedNode.Text != "/") {
                string nodeText = "";
                TreeNode currentNode = treeViewServer.SelectedNode;
                while (currentNode != null) {
                    if (currentNode.Text != "/") {
                        nodeText = "/" + currentNode.Text + nodeText;
                    }
                    currentNode = currentNode.Parent;
                }
                selectedFolder += nodeText;
            }

            return Regex.Replace(selectedFolder, "^/", "");
        }

        private void buttonConnect_Click(object sender, EventArgs e) {
            treeViewServer.Nodes.Clear();

            this._protocol = comboBoxProtocol.Text;
            this._domain = textBoxServer.Text;
            this._path = Regex.Replace(Regex.Replace(textBoxPath.Text, "^\\/+", ""), "\\/+$", "");
            this._username = textBoxUsername.Text;
            this._password = textBoxPassword.Text;

            this.RefreshFolderView();
        }

        private void RefreshFolderView() {
            treeViewServer.Nodes.Clear();

            IFolder folder = null;
            try {
                WebDavSession session = new WebDavSession();
                session.Credentials = new NetworkCredential(this._username, this._password);
                string url = this._protocol + "://" + this._domain + "/";
                if (this._path != String.Empty) {
                    url += this._path + "/";
                }
                folder = session.OpenFolder(url);
            } catch (UnauthorizedException) {
                MessageBox.Show("nicht Authorisiert");
                return;
            } catch (Exception) {

            }

            if (folder != null) {
                IHierarchyItem[] items = folder.GetChildren();
                TreeNode root = new TreeNode("/");
                treeViewServer.Nodes.Add(root);
                foreach (IHierarchyItem item in items) {
                    if (item.ItemType == ItemType.Folder) {
                        root.Nodes.Add(new TreeNode(item.DisplayName));
                    }
                }
            }
        }

        private void treeViewServer_AfterSelect(object sender, TreeViewEventArgs e) {
            this.ShowSelectedFolderContent();
        }

        private void ShowSelectedFolderContent() {
            listViewFiles.Items.Clear();

            IFolder folder = null;
            try {
                string path = this.GetSelectedFolder();
                WebDavSession session = new WebDavSession();
                session.Credentials = new NetworkCredential(this._username, this._password);
                string url = this._protocol + "://" + this._domain + "/";
                if (path != String.Empty) {
                    url += path + "/";
                }
                folder = session.OpenFolder(url);
            } catch (Exception) {

            }

            if (folder != null) {
                IHierarchyItem[] items = folder.GetChildren();
                foreach (IHierarchyItem item in items) {
                    if (item.ItemType == ItemType.Resource) {
                        PropertyName[] propNames = new PropertyName[] { new PropertyName("getcontentlength", "DAV:") };
                        Property[] properties = item.GetPropertyValues(propNames);

                        string contentLength = "";
                        if (properties.Length > 0) {
                            try {
                                Int64 content_length = Convert.ToInt64(properties.First().StringValue);
                                if (content_length > 1000000000000) {
                                    contentLength = (content_length / 1099511627776.0).ToString("f") + " TB";
                                } else if (content_length > 1000000000) {
                                    contentLength = (content_length / (1024.0 * 1024.0 * 1024.0)).ToString("f") + " GB";
                                } else if (content_length > 1000000) {
                                    contentLength = (content_length / (1024.0 * 1024.0)).ToString("f") + " MB";
                                } else if (content_length > 1000) {
                                    contentLength = (content_length / 1024.0).ToString("f") + " KB";
                                } else {
                                    contentLength = content_length.ToString() + " B";
                                }
                            } catch (Exception) {
                                contentLength = properties.First().StringValue;
                            }
                        }
                        string[] test = new string[] { item.DisplayName, contentLength };
                        ListViewItem lvItem = new ListViewItem(test);
                        listViewFiles.Items.Add(lvItem);
                    } else if (item.ItemType == ItemType.Folder) {
                        if (item.DisplayName != String.Empty) {
                            bool nodeExists = false;
                            foreach (TreeNode currentNode in treeViewServer.SelectedNode.Nodes) {
                                if (currentNode.Text == item.DisplayName) {
                                    nodeExists = true;
                                    break;
                                }
                            }
                            if (!nodeExists) {
                                TreeNode currentNode = new TreeNode(item.DisplayName);
                                treeViewServer.SelectedNode.Nodes.Add(currentNode);
                            }
                        }
                    }
                }
            }
        }

        private void listViewFiles_MouseClick(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Right) {
                ListView lv = (ListView)sender;
                contextMenuStripFilename.Items["toolStripMenuItemSaveUnder"].Visible = true;
                contextMenuStripFilename.Items["toolStripMenuItemDeleteFile"].Visible = true;
                contextMenuStripFilename.Items["toolStripMenuItemCreateFolder"].Visible = false;
                contextMenuStripFilename.Items["toolStripMenuItemDeleteFolder"].Visible = false;
                contextMenuStripFilename.Show(lv, e.X, e.Y);
            }
        }

        private void treeViewServer_MouseClick(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Right) {
                TreeView tv = (TreeView)sender;
                contextMenuStripFilename.Items["toolStripMenuItemSaveUnder"].Visible = false;
                contextMenuStripFilename.Items["toolStripMenuItemDeleteFile"].Visible = false;
                contextMenuStripFilename.Items["toolStripMenuItemCreateFolder"].Visible = true;
                if (tv.SelectedNode != null && tv.SelectedNode.Text != "/") {
                    contextMenuStripFilename.Items["toolStripMenuItemDeleteFolder"].Visible = true;
                } else {
                    contextMenuStripFilename.Items["toolStripMenuItemDeleteFolder"].Visible = false;
                }
                contextMenuStripFilename.Show(tv, e.X, e.Y);
            }
        }

        private void contextMenuStripFilename_ItemClicked(object sender, ToolStripItemClickedEventArgs e) {
            ((ContextMenuStrip)sender).Hide();
            switch (e.ClickedItem.Name) {
                case "toolStripMenuItemSaveUnder":
                    if (folderBrowserDialog.ShowDialog() == DialogResult.OK) {
                        WebDavSession session = new WebDavSession();
                        session.Credentials = new NetworkCredential(this._username, this._password);
                        IFolder folder = session.OpenFolder(this._protocol + "://" + this._domain + "/" + this._path + "/");
                        IResource resource = folder.GetResource(listViewFiles.SelectedItems[0].Text);

                        string directorySeparator = (Environment.OSVersion.Platform.ToString() == "Unix") ? "/" : "\\";
                        string fullSaveFilename = folderBrowserDialog.SelectedPath + directorySeparator + listViewFiles.SelectedItems[0].Text;

                        DownloadForm downloadForm = new DownloadForm();
                        downloadForm.Shown += (object downloadFormSender, EventArgs downloadFormE) => {
                            downloadForm.DownloadResource(resource, fullSaveFilename);
                        };
                        downloadForm.ShowDialog();
                    }
                    break;

                case "toolStripMenuItemUploadFile":
                    if (openFileDialog.ShowDialog() == DialogResult.OK) {
                        string filename = openFileDialog.SafeFileName;
                        string nodeText = "";
                        TreeNode currentNode = treeViewServer.SelectedNode;
                        while (currentNode != null) {
                            if (currentNode.Text != "/") {
                                nodeText = "/" + currentNode.Text + nodeText;
                            }
                            currentNode = currentNode.Parent;
                        }
                        string fullDestinationPath = comboBoxProtocol.Text + "://" + textBoxServer.Text + textBoxPath.Text + nodeText + "/" + filename;
                        Console.WriteLine(fullDestinationPath);
                        Uri destination = new Uri(fullDestinationPath);

                        IFolder folder = null;
                        try {
                            WebDavSession session = new WebDavSession();
                            session.Credentials = new NetworkCredential(this._username, this._password);
                            folder = session.OpenFolder(Regex.Replace(destination.AbsoluteUri, "[^/]+$", ""));
                            IResource resource = folder.CreateResource(filename);

                            DownloadForm downloadForm = new DownloadForm();
                            downloadForm.Shown += (object downloadFormSender, EventArgs downloadFormE) => {
                                downloadForm.UploadFile(openFileDialog.FileName, resource);
                            };
                            downloadForm.FormClosed += (object closedSender, FormClosedEventArgs closedE) => {
                                this.ShowSelectedFolderContent();
                            };
                            downloadForm.ShowDialog();
                        } catch (Exception) {

                        }
                    }
                    break;

                case "toolStripMenuItemDeleteFile":
                    string message = "Möchstest du die Datei (" + listViewFiles.SelectedItems[0].Text + ") wirklich löschen?";
                    string caption = "Datei löschen";
                    if (MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                        string folderUri = comboBoxProtocol.Text + "://" + textBoxServer.Text + "/" + this.GetSelectedFolder() + "/";
                        Console.WriteLine(folderUri);
                        WebDavSession session = new WebDavSession();
                        session.Credentials = new NetworkCredential(this._username, this._password);
                        IFolder folder = session.OpenFolder(folderUri);
                        IResource resource = folder.GetResource(listViewFiles.SelectedItems[0].Text);
                        resource.Delete();
                        this.ShowSelectedFolderContent();
                    }
                    break;

                case "toolStripMenuItemCreateFolder":
                    string folderName = Interaction.InputBox("Name des zu erstellenden Ordners", "Bitte den Ordnernamen eingeben!");
                    if (folderName != String.Empty) {
                        string folderUri = comboBoxProtocol.Text + "://" + textBoxServer.Text + "/" + this.GetSelectedFolder() + "/";
                        Console.WriteLine(folderUri);
                        WebDavSession session = new WebDavSession();
                        session.Credentials = new NetworkCredential(this._username, this._password);
                        IFolder folder = session.OpenFolder(folderUri);
                        folder.CreateFolder(folderName);
                        this.RefreshFolderView();
                    } else {
                        MessageBox.Show("Der Ordnername kann nicht leer sein.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    break;

                case "toolStripMenuItemDeleteFolder":
                    string deleteMessage = "Möchstest du das Verzeichnis (" + treeViewServer.SelectedNode.Text + ") wirklich löschen?";
                    string deleteCaption = "Datei löschen";
                    if (MessageBox.Show(deleteMessage, deleteCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                        string folderUri = comboBoxProtocol.Text + "://" + textBoxServer.Text + "/" + this.GetSelectedFolder() + "/";
                        Console.WriteLine(folderUri);
                        WebDavSession session = new WebDavSession();
                        session.Credentials = new NetworkCredential(this._username, this._password);
                        IFolder folder = session.OpenFolder(folderUri);
                        folder.Delete();
                        this.RefreshFolderView();
                    }
                    break;
            }
        }
    }
}
