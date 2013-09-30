using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Net;
using System.Web;
using WebDav.Client;

namespace WebDavWinFormsClientDemo {
	public partial class DownloadForm : Form {
		private bool _downloadInProgress = false;
		private WebClient _webClient = new WebClient();
		private BackgroundWorker _worker = new BackgroundWorker();

		public bool DownloadInProgress { get { return this._downloadInProgress; } }

		public DownloadForm() {
			InitializeComponent();
		}

		public void DownloadResource(IResource resource, string destination) {
			try {
				this.buttonReady.Enabled = false;
				this.labelFileDownload.Text = "Datei (" + resource.DisplayName + ") wird runtergeladen: ";
				this._downloadInProgress = true;

				this._worker.WorkerReportsProgress = true;
				this._worker.WorkerSupportsCancellation = true;
				this._worker.DoWork += new DoWorkEventHandler((object sender, DoWorkEventArgs e) => {
					try {
						Stream sourceStream = resource.GetReadStream();
						FileStream fileStream = new FileStream(destination, FileMode.Create);
						byte[] buffer = new byte[10000];
						long totalBytesRead = 0;
						long bytesRead = 0;
						DateTime begin = DateTime.Now;
						do {
							if (this._worker.CancellationPending) {
								e.Cancel = true;
								Console.WriteLine("BackgroundWorker wird abgebrochen!");
								break;
							}
	
							bytesRead = sourceStream.Read(buffer, 0, buffer.Length);
							if (bytesRead > 0) {
								totalBytesRead += bytesRead;
								fileStream.Write(buffer, 0, (int)bytesRead);
	
								DateTime now = DateTime.Now;
								TimeSpan diffTime = now.Subtract(begin);
								if (diffTime.TotalSeconds >= 1) {
									int progressPercentage = (int)(totalBytesRead * 100 / resource.ContentLength);
									this._worker.ReportProgress(progressPercentage, totalBytesRead);
									begin = DateTime.Now;
								}
							}
						} while (bytesRead > 0);
						fileStream.Close();
					} catch(Exception exc) {
						Console.WriteLine(exc.Message);
					}
				}
				);
				this._worker.ProgressChanged += new ProgressChangedEventHandler((object sender, ProgressChangedEventArgs e) => {
					this.labelFileDownload.Text = String.Format("Datei ({0}) wird runtergeladen: {1}/{2}", resource.DisplayName, this.GetBestSizeFormat((long)e.UserState), this.GetBestSizeFormat(resource.ContentLength));
					this.labelPercentage.Text = e.ProgressPercentage + "%";
					progressBarFileDownload.Value = e.ProgressPercentage;
				}
				);
				this._worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler((object sender, RunWorkerCompletedEventArgs e) => {
					Console.WriteLine("BackgroundWorker fertig!");

					if (e.Cancelled) {
						try {
							File.Delete(destination);
						} catch(Exception) {

						}
					} else {
						this.labelFileDownload.Text = String.Format("Datei ({0}) wird runtergeladen: {1}/{2}", resource.DisplayName, this.GetBestSizeFormat(resource.ContentLength), this.GetBestSizeFormat(resource.ContentLength));
						this.labelPercentage.Text = "100%";
					}

					progressBarFileDownload.Value = 100;
					this._downloadInProgress = false;
					this.buttonReady.Enabled = true;
				}
				);
				this._worker.RunWorkerAsync();
			} catch(Exception e) {
				Console.WriteLine(e.Message);
			}
		}

		public void DownloadFile(Uri source, string destination) {
			this.buttonReady.Enabled = false;
			this.labelFileDownload.Text = "Datei (" + source.Segments[source.Segments.Length - 1].ToString() + ") wird runtergeladen: ";
			this._downloadInProgress = true;
			this._webClient.DownloadProgressChanged += (object sender, DownloadProgressChangedEventArgs e) =>
			{
				this.labelFileDownload.Text = String.Format("Datei ({0}) wird runtergeladen: {1}/{2}", source.Segments[source.Segments.Length - 1], this.GetBestSizeFormat(e.BytesReceived), this.GetBestSizeFormat(e.TotalBytesToReceive));
				this.labelPercentage.Text = e.ProgressPercentage.ToString() + "%";
				progressBarFileDownload.Value = e.ProgressPercentage;
			};
			this._webClient.DownloadFileCompleted += (object sender, AsyncCompletedEventArgs e) =>
			{
				if (!e.Cancelled) {
					progressBarFileDownload.Value = 100;
				} else {
					try {
						File.Delete(destination);
					} catch(Exception) {

					}
				}
				this._downloadInProgress = false;
				this.buttonReady.Enabled = true;
			};
			this._webClient.DownloadFileAsync(source, destination);
		}
		
		public void UploadFile(string source, IResource destinationResource) {
			destinationResource.TimeOut = System.Threading.Timeout.Infinite;

			this.buttonReady.Enabled = false;
			this.labelFileDownload.Text = "Datei (" + destinationResource.DisplayName + ") wird hochgeladen: ";
			this._downloadInProgress = true;
			
			FileInfo file = new FileInfo(source);
			this._worker = new BackgroundWorker();
			this._worker.WorkerReportsProgress = true;
			this._worker.WorkerSupportsCancellation = true;
			this._worker.DoWork += new DoWorkEventHandler((object sender, DoWorkEventArgs e) => {
				Console.WriteLine("BackgroundWorker gestartet");
                
				try {
					using(Stream webStream = destinationResource.GetWriteStream("application/octet-stream", file.Length)) {
						int bufSize = 1024; // 1Kb
						byte[] buffer = new byte[bufSize];
						int bytesRead = 0;
						long totalBytesRead = 0;
						DateTime begin = DateTime.Now;
						using(FileStream fileStream = file.OpenRead()) {
							do {
								if (this._worker.CancellationPending) {
									e.Cancel = true;
									Console.WriteLine("BackgroundWorker wird abgebrochen!");
									break;
								}
                                
								bytesRead = fileStream.Read(buffer, 0, bufSize);
								if (bytesRead > 0) {
									try {
										webStream.Write(buffer, 0, bytesRead);
										totalBytesRead += bytesRead;
										if (DateTime.Now.Subtract(begin).TotalSeconds >= 1) {
											int ProgressPercentage = (int)(totalBytesRead * 100 / file.Length);
											this._worker.ReportProgress(ProgressPercentage, totalBytesRead);
											begin = DateTime.Now;
										}
									} catch(Exception writeException) {
										e.Cancel = true;
										Console.WriteLine("Verbindung wurde unerwartet unterbrochen! (" + totalBytesRead + " bytes)");
										MessageBox.Show(writeException.Message);
										break;
									}
								}
							} while (bytesRead > 0);
						}

						string result = "";
						if (webStream.CanRead) {
							byte[] buffer2 = new byte[8192];
							int bytesRead2 = 0;
							do {
								bytesRead2 = webStream.Read(buffer2, 0, buffer2.Length);
								if (bytesRead2 > 0) {
									result += Encoding.UTF8.GetString(buffer2, 0, bytesRead2);
								}
							} while (bytesRead2 > 0);
						}
						if (result != String.Empty) {
							Console.WriteLine(result);
						}
					}
				} catch(Exception exc) {
					e.Cancel = true;
					MessageBox.Show(exc.Message);
				}
			}
			);
			this._worker.ProgressChanged += new ProgressChangedEventHandler((object sender, ProgressChangedEventArgs e) => {
				int ProgressPercentage = Convert.ToInt32((long)e.UserState * 100.0 / file.Length);
				this.labelFileDownload.Text = String.Format("Datei ({0}) wird hochgeladen: {1}/{2}", destinationResource.DisplayName, this.GetBestSizeFormat((long)e.UserState), this.GetBestSizeFormat(file.Length));
				this.labelPercentage.Text = ProgressPercentage.ToString() + "%";
				progressBarFileDownload.Value = ProgressPercentage;
			}
			);
			this._worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler((object sender, RunWorkerCompletedEventArgs e) => {
				Console.WriteLine("BackgroundWorker fertig!");

				if (!e.Cancelled) {
					progressBarFileDownload.Value = 100;
					this.labelFileDownload.Text = String.Format("Datei ({0}) wird hochgeladen: {1}/{2}", destinationResource.DisplayName, this.GetBestSizeFormat(file.Length), this.GetBestSizeFormat(file.Length));
					this.labelPercentage.Text = "100%";
				}
				this._downloadInProgress = false;
				this.buttonReady.Enabled = true;
			}
			);
			this._worker.RunWorkerAsync();
		}

		public void UploadFile(string source, Uri destination) {
			this.buttonReady.Enabled = false;
			this.labelFileDownload.Text = "Datei (" + destination.Segments[destination.Segments.Length - 1].ToString() + ") wird hochgeladen: ";
			this._downloadInProgress = true;
			this._webClient.UploadProgressChanged += (object sender, UploadProgressChangedEventArgs e) =>
			{
				int ProgressPercentage = Convert.ToInt32(e.BytesSent * 100.0 / e.TotalBytesToSend);
				this.labelFileDownload.Text = String.Format("Datei ({0}) wird hochgeladen: {1}/{2}", HttpUtility.UrlDecode(destination.Segments[destination.Segments.Length - 1]), this.GetBestSizeFormat(e.BytesSent), this.GetBestSizeFormat(e.TotalBytesToSend));
				this.labelPercentage.Text = ProgressPercentage.ToString() + "%";
				progressBarFileDownload.Value = ProgressPercentage;
			};
			this._webClient.UploadFileCompleted += (object sender, UploadFileCompletedEventArgs e) =>
			{
				if (!e.Cancelled) {
					progressBarFileDownload.Value = 100;
				}
				this._downloadInProgress = false;
				this.buttonReady.Enabled = true;
			};
			this._webClient.UploadFileAsync(destination, "PUT", source);
		}

		private string GetBestSizeFormat(long size) {
			string sizeFormat = "";

			try {
				if (size > 1000000000000) {
					sizeFormat = (size / 1099511627776.0).ToString("f") + "TB";
				} else if (size > 1000000000) {
						sizeFormat = (size / (1024.0 * 1024.0 * 1024.0)).ToString("f") + "GB";
					} else if (size > 1000000) {
							sizeFormat = (size / (1024.0 * 1024.0)).ToString("f") + "MB";
						} else if (size > 1000) {
								sizeFormat = (size / 1024.0).ToString("f") + "KB";
							} else {
								sizeFormat = size.ToString() + "B";
							}
			} catch(Exception) {
				sizeFormat = size.ToString();
			}

			return sizeFormat;
		}

		private void DownloadForm_FormClosing(object sender, FormClosingEventArgs e) {
			if (this._downloadInProgress) {
				MessageBox.Show("Dieses Fenster kann während des Downloads nicht geschlossen werden!");
				e.Cancel = true;
			}
		}

		private void buttonReady_Click(object sender, EventArgs e) {
			this.Close();
		}

		private void buttonCancel_Click(object sender, EventArgs e) {
			DialogResult result = MessageBox.Show("Willst du den Download wirklich abbrechen?", "Download Abbruch", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
			if (result == System.Windows.Forms.DialogResult.Yes) {
				this._webClient.CancelAsync();
				this._worker.CancelAsync();
			}
		}
	}
}
