using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using WebDav.Client;

namespace WebDav {
	namespace Client {
		public interface IResource : IItemContent, IHierarchyItem, IConnectionSettings {
			bool CheckedOut { get; }
			bool VersionControlled { get; }
		}

		public class WebDavResource : IResource {
			private ICredentials _credentials = new NetworkCredential();

			// IResource
			private bool _checkedOut = false;
			private bool _versionControlled = false;
			public bool CheckedOut { get { return this._checkedOut; } }
			public bool VersionControlled { get { return this._versionControlled; } }

			// IItemContent
			private long _contentLength = 0;
			private string _contentType = "";
			public long ContentLength { get { return this._contentLength; } }
			public string ContentType { get { return this._contentType; } }

			/// <summary>
			/// Downloads content of the resource to a file specified by filename
			/// </summary>
			/// <param name="filename">Full path of a file to be downloaded to</param>
			public void Download(string filename) {
				WebClient webClient = new WebClient();
				webClient.DownloadFile(this._href, filename);
			}

			/// <summary>
			/// Uploads content of a file specified by filename to the server
			/// </summary>
			/// <param name="filename">Full path of a file to be uploaded from</param>
			public void Upload(string filename) {
				NetworkCredential credentials = (NetworkCredential)this._credentials;
				string auth = "Basic " + Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(credentials.UserName + ":" + credentials.Password));
				WebClient webClient = new WebClient();
				webClient.Credentials = credentials;
				webClient.Headers.Add("Authorization", auth);
				webClient.UploadFile(this.Href, "PUT", filename);
			}

			/// <summary>
			/// Loads content of the resource from WebDAV server. 
			/// </summary>
			/// <returns>Stream to read resource content.</returns>
			public Stream GetReadStream() {
				NetworkCredential credentials = (NetworkCredential)this._credentials;
				string auth = "Basic " + Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(credentials.UserName + ":" + credentials.Password));
				WebClient webClient = new WebClient();
				webClient.Credentials = credentials;
				webClient.Headers.Add("Authorization", auth);
				return webClient.OpenRead(this._href);
			}

			/// <summary>
			/// Saves resource's content to WebDAV server.
			/// </summary>
			/// <param name="contentLength">Length of data to be written.</param>
			/// <returns>Stream to write resource content.</returns>
			public Stream GetWriteStream(long contentLength) {
				return this.GetWriteStream("application/octet-stream", contentLength);

				/*
				NetworkCredential credentials = (NetworkCredential)this._credentials;
				string auth = "Basic " + Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(credentials.UserName + ":" + credentials.Password));
				HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(this.Href);
				webRequest.AllowWriteStreamBuffering = false;
				webRequest.Method = "PUT";
				webRequest.Credentials = credentials;
				webRequest.ContentLength = contentLength;
				webRequest.Headers.Add("Authorization", auth);
				Stream webStream = webRequest.GetRequestStream();
				if (webStream.CanTimeout) {
					webStream.WriteTimeout = this.TimeOut * 1000;
				}
				return webStream;
				*/
			}

			/// <summary>
			/// Saves resource's content to WebDAV server.
			/// </summary>
			/// <param name="contentType">Media type of the resource.</param>
			/// <param name="contentLength">Length of data to be written.</param>
			/// <returns>Stream to write resource content.</returns>
			public Stream GetWriteStream(string contentType, long contentLength) {
				TcpClient tcpClient = new TcpClient(this.Href.Host, this.Href.Port);
				if (tcpClient.Connected) {
					NetworkCredential credentials = (NetworkCredential)this._credentials;
					string auth = "Basic " + Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(credentials.UserName + ":" + credentials.Password));

					try {
						if (this.TimeOut != System.Threading.Timeout.Infinite) {
							tcpClient.SendTimeout = this.TimeOut;
							tcpClient.ReceiveTimeout = this.TimeOut;
						} else {
							tcpClient.SendTimeout = 0;
							tcpClient.ReceiveTimeout = 0;
						}
					} catch(SocketException e) {
						Console.WriteLine("TcpClient.Timeout SocketException: " + e.Message);
						Console.WriteLine("TcpClient.Timeout set timeout to default value 0!");
						tcpClient.SendTimeout = 0;
						tcpClient.ReceiveTimeout = 0;
					}
					NetworkStream networkStream = tcpClient.GetStream();
					if (networkStream.CanTimeout) {
						try {
							networkStream.WriteTimeout = this.TimeOut;
							networkStream.ReadTimeout = this.TimeOut;
						} catch(Exception e) {
							Console.WriteLine("NetworkStream.Timeout Exception: " + e.Message);
						}
					}
					byte[] methodBuffer = Encoding.UTF8.GetBytes("PUT " + this.Href.AbsolutePath + " HTTP/1.1\r\n");
					byte[] hostBuffer = Encoding.UTF8.GetBytes("Host: " + this.Href.Host + "\r\n");
					byte[] contentLengthBuffer = Encoding.UTF8.GetBytes("Content-Length: " + contentLength + "\r\n");
					byte[] authorizationBuffer = Encoding.UTF8.GetBytes("Authorization: " + auth + "\r\n");
					byte[] connectionBuffer = Encoding.UTF8.GetBytes("Connection: Close\r\n\r\n");
					networkStream.Write(methodBuffer, 0, methodBuffer.Length);
					networkStream.Write(hostBuffer, 0, hostBuffer.Length);
					networkStream.Write(contentLengthBuffer, 0, contentLengthBuffer.Length);
					networkStream.Write(authorizationBuffer, 0, authorizationBuffer.Length);
					networkStream.Write(connectionBuffer, 0, connectionBuffer.Length);

					return networkStream;
				}

				throw new IOException("could not connect to server");
			}

			// IHierarchyItem
			private string _comment = "";
			private DateTime _creationDate = new DateTime(0);
			private string _creatorDisplayName = "";
			private Uri _href = null;
			private Uri _baseUri = null;
			private ItemType _itemType;
			private DateTime _lastModified = new DateTime(0);
			private Property[] _properties = { };
			public string Comment { get { return this._comment; } }
			public DateTime CreationDate { get { return this._creationDate; } }
			public string CreatorDisplayName { get { return this._creatorDisplayName; } }
			public string DisplayName {
				get {
					string displayName = this._href.AbsoluteUri.Replace(this._baseUri.AbsoluteUri, "");
					displayName = Regex.Replace(displayName, "\\/$", "");
					Match displayNameMatch = Regex.Match(displayName, "([\\/]+)$");
					if (displayNameMatch.Success) {
						displayName = displayNameMatch.Groups[1].Value;
					}
					return HttpUtility.UrlDecode(displayName);
				}
			}
			public Uri Href { get { return this._href; } }
			public Uri BaseUri { get { return this._baseUri; } }
			public ItemType ItemType { get { return this._itemType; } }
			public DateTime LastModified { get { return this._lastModified; } }
			public Property[] Properties { get { return this._properties; } }

			// IHierarchyItem Methods
			/// <summary>
			/// Retrieves all custom properties exposed by the item.
			/// </summary>
			/// <returns>This method returns the array of custom properties exposed by the item.</returns>
			public Property[] GetAllProperties() {
				return this._properties;
			}

			/// <summary>
			/// Returns names of all custom properties exposed by this item.
			/// </summary>
			/// <returns></returns>
			public PropertyName[] GetPropertyNames() {
				return this._properties.Select(p => p.Name).ToArray();
			}

			/// <summary>
			/// Retrieves values of specific properties.
			/// </summary>
			/// <param name="names"></param>
			/// <returns>Array of requested properties with values.</returns>
			public Property[] GetPropertyValues(PropertyName[] names) {
				return (from p in this._properties from pn in names where pn.Equals(p.Name) select p).ToArray();
			}

			/// <summary>
			/// Deletes this item.
			/// </summary>
			public void Delete() {
				NetworkCredential credentials = (NetworkCredential)this._credentials;
				string auth = "Basic " + Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(credentials.UserName + ":" + credentials.Password));
				WebRequest webRequest = HttpWebRequest.Create(this.Href);
				webRequest.Method = "DELETE";
				webRequest.Credentials = credentials;
				webRequest.Headers.Add("Authorization", auth);
				using(WebResponse webResponse = webRequest.GetResponse()) {
					using(Stream responseStream = webResponse.GetResponseStream()) {
						byte[] buffer = new byte[8192];
						string result = "";
						int bytesRead = 0;
						do {
							bytesRead = responseStream.Read(buffer, 0, buffer.Length);
							if (bytesRead > 0) {
								result += Encoding.UTF8.GetString(buffer, 0, bytesRead);
							}
						} while (bytesRead > 0);
					}
				}
			}

			/// <summary>
			/// For internal use only.
			/// </summary>
			/// <param name="comment"></param>
			public void SetComment(string comment) {
				this._comment = comment;
			}

			/// <summary>
			/// For internal use only.
			/// </summary>
			/// <param name="comment"></param>
			public void SetCreationDate(string creationDate) {
				this._creationDate = DateTime.Parse(creationDate);
			}

			/// <summary>
			/// For internal use only.
			/// </summary>
			/// <param name="comment"></param>
			public void SetCreationDate(DateTime creationDate) {
				this._creationDate = creationDate;
			}

			/// <summary>
			/// For internal use only.
			/// </summary>
			/// <param name="comment"></param>
			public void SetCreatorDisplayName(string creatorDisplayName) {
				this._creatorDisplayName = creatorDisplayName;
			}

			/// <summary>
			/// For internal use only.
			/// </summary>
			/// <param name="comment"></param>
			public void SetHref(string href, Uri baseUri) {
				this._href = new Uri(href);
				this._baseUri = baseUri;
			}

			/// <summary>
			/// For internal use only.
			/// </summary>
			/// <param name="comment"></param>
			public void SetHref(Uri href) {
				this._href = href;
				string baseUri = this._href.Scheme + "://" + this._href.Host;
				for(int i = 0; i < this._href.Segments.Length - 1; i++) {
					if (this._href.Segments[i] != "/") {
						baseUri += "/" + this._href.Segments[i];
					}
				}
				this._baseUri = new Uri(baseUri);
			}

			/// <summary>
			/// For internal use only.
			/// </summary>
			/// <param name="comment"></param>
			public void SetLastModified(string lastModified) {
				this._lastModified = DateTime.Parse(lastModified);
			}

			/// <summary>
			/// For internal use only.
			/// </summary>
			/// <param name="comment"></param>
			public void SetLastModified(DateTime lastModified) {
				this._lastModified = lastModified;
			}

			/// <summary>
			/// For internal use only.
			/// </summary>
			/// <param name="comment"></param>
			public void SetProperty(Property property) {
				if (property.Name.Name == "resourcetype" && property.StringValue != String.Empty) {
					XmlDocument XmlDoc = new XmlDocument();
					try {
						XmlDoc.LoadXml(property.StringValue);
						property.StringValue = XmlDoc.DocumentElement.LocalName;
						switch(property.StringValue) {
							case "collection":
								this._itemType = ItemType.Folder;
								break;

							default:
								Console.WriteLine("unknown resourcetype: " + property.StringValue);
								break;
						}
					} catch(Exception e) {
						Console.WriteLine("WebDavHierarchyItem.SetProperty()-Exception: " + e.Message);
					}
				}

				bool propertyFound = false;
				foreach(Property prop in this._properties) {
					if (prop.Name.Equals(property.Name)) {
						prop.StringValue = property.StringValue;
						propertyFound = true;
					}
				}

				if (!propertyFound) {
					Property[] newProperties = new Property[this._properties.Length + 1];
					for(int i = 0; i < this._properties.Length; i++) {
						newProperties[i] = this._properties[i];
					}
					if (property.Name.Name == "getcontentlength") {
						try {
							this._contentLength = Convert.ToInt64(property.StringValue);
						} catch(Exception) {

						}
					}
					newProperties[this._properties.Length] = property;
					this._properties = newProperties;
				}
			}

			/// <summary>
			/// For internal use only.
			/// </summary>
			/// <param name="comment"></param>
			public void SetProperty(PropertyName propertyName, string value) {
				this.SetProperty(new Property(propertyName, value));
			}

			/// <summary>
			/// For internal use only.
			/// </summary>
			/// <param name="comment"></param>
			public void SetProperty(string name, string nameSpace, string value) {
				this.SetProperty(new Property(name, nameSpace, value));
			}

			/// <summary>
			/// For internal use only.
			/// </summary>
			/// <param name="comment"></param>
			public void SetProperties(Property[] properties) {
				foreach(Property property in properties) {
					this.SetProperty(property);
				}
			}

			/// <summary>
			/// For internal use only.
			/// </summary>
			/// <param name="comment"></param>
			public void SetCredentials(ICredentials credentials) {
				this._credentials = credentials;
			}

			// IConnectionSettings
			private bool _allowWriteStreamBuffering = false;
			private bool _sendChunked = false;
			private int _timeOut = 30000;
			public bool AllowWriteStreamBuffering { get { return this._allowWriteStreamBuffering; } set { this._allowWriteStreamBuffering = value; } }
			public bool SendChunked { get { return this._sendChunked; } set { this._sendChunked = value; } }
			public int TimeOut { get { return this._timeOut; } set { this._timeOut = value; } }

			/// <summary>
			/// For internal use only.
			/// </summary>
			/// <param name="comment"></param>
			public void SetHierarchyItem(IHierarchyItem item) {
				this.SetComment(item.Comment);
				this.SetCreationDate(item.CreationDate);
				this.SetCreatorDisplayName(item.CreatorDisplayName);
				this.SetHref(item.Href);
				this.SetLastModified(item.LastModified);
				this.SetProperties(item.Properties);
			}
		}
	}
}
