using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using WebDav.Client;
using WebDav.Client.Exceptions;

namespace WebDav {
	namespace Client {
		public interface IFolder : IHierarchyItem {
			IResource CreateResource(string name);
            IFolder CreateFolder(string name);
			IHierarchyItem[] GetChildren();
            IResource GetResource(string name);
		}
		
		public class WebDavFolder : WebDavHierarchyItem, IFolder {
			private Uri _path;
			private IHierarchyItem[] _children = new IHierarchyItem[0];
			
            /// <summary>
            /// The constructor
            /// </summary>
			public WebDavFolder () {
				
			}
			
            /// <summary>
            /// The constructor
            /// </summary>
            /// <param name="path">Path to the folder.</param>
			public WebDavFolder (string path) {
				this._path = new Uri(path);
			}
			
            /// <summary>
            /// The constructor
            /// </summary>
            /// <param name="path">Path to the folder.</param>
			public WebDavFolder (Uri path) {
				this._path = path;
			}
			
            /// <summary>
            /// Creates a resource with a specified name.
            /// </summary>
            /// <param name="name">Name of the new resource.</param>
            /// <returns>Newly created resource.</returns>
			public IResource CreateResource(string name) {
				WebDavResource resource = new WebDavResource();
				try {
					resource.SetHref(new Uri(this.Href.AbsoluteUri + name));
                    NetworkCredential credentials = (NetworkCredential)this._credentials;
                    string auth = "Basic " + Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(credentials.UserName + ":" + credentials.Password));
					HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(resource.Href);
					request.Method = "PUT";
                    request.Credentials = credentials;
					request.ContentType = "text/xml";
					request.Accept = "text/xml";
					request.Headers["translate"] = "f";
                    request.Headers.Add("Authorization", auth);
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse()) {
                        if (response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.NoContent) {
                            this.Open(this.Href);
                            resource = (WebDavResource)this.GetResource(name);
                            resource.SetCredentials(this._credentials);
                        }
                    }
				} catch(Exception e) {
					Console.WriteLine(e.Message);
				}
				
				return resource;
			}

            /// <summary>
            /// Creates new folder with specified name as child of this one.
            /// </summary>
            /// <param name="name">Name of the new folder.</param>
            /// <returns>IFolder</returns>
            public IFolder CreateFolder(string name) {
                WebDavFolder folder = new WebDavFolder();
                try {
                    NetworkCredential credentials = (NetworkCredential)this._credentials;
                    string auth = "Basic " + Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(credentials.UserName + ":" + credentials.Password));
                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(this.Href.AbsoluteUri + name);
                    request.Method = "MKCOL";
                    request.Credentials = credentials;
                    request.Headers.Add("Authorization", auth);
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse()) {
                        if (response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.NoContent) {
                            folder.SetCredentials(this._credentials);
                            folder.Open(this.Href.AbsoluteUri + name + "/");
                        }
                    }
                } catch (Exception e) {
                    Console.WriteLine(e.Message);
                }

                return folder;
            }

            /// <summary>
            /// Returns children of this folder.
            /// </summary>
            /// <returns>Array that include child folders and resources.</returns>
            public IHierarchyItem[] GetChildren() {
                return this._children;
            }

            /// <summary>
            /// Gets the specified resource from server.
            /// </summary>
            /// <param name="name">Name of the resource.</param>
            /// <returns>Resource corresponding to requested name.</returns>
            public IResource GetResource(string name) {
                IHierarchyItem item = this._children.Where(i => i.ItemType == ItemType.Resource && i.DisplayName == name).Single();
                WebDavResource resource = new WebDavResource();
                resource.SetCredentials(this._credentials);
                resource.SetHierarchyItem(item);
                return resource;
            }

            /// <summary>
            /// Opens the folder.
            /// </summary>
            public void Open() {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(this._path);
                request.PreAuthenticate = true;
                request.Method = "PROPFIND";
                request.ContentType = "application/xml";
                request.Headers["Depth"] = "1";

                NetworkCredential credentials = (NetworkCredential)this._credentials;
                if (credentials != null && credentials.UserName != null) {
                    request.Credentials = credentials;
                    string auth = "Basic " + Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(credentials.UserName + ":" + credentials.Password));
                    request.Headers.Add("Authorization", auth);
                }
                try {
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse()) {
                        using (StreamReader responseStream = new StreamReader(response.GetResponseStream())) {
                            this.ProcessResponse(responseStream.ReadToEnd());
                        }
                    }
                } catch (WebException e) {
                    if (e.Status == WebExceptionStatus.ProtocolError) {
                        throw new UnauthorizedException();
                    }
                }
            }

            /// <summary>
            /// Opens the folder
            /// </summary>
            /// <param name="path">Path of the folder to open.</param>
            public void Open(string path) {
                this._path = new Uri(path);
                this.Open();
            }

            /// <summary>
            /// Opens the folder
            /// </summary>
            /// <param name="path">Path of the folder to open.</param>
            public void Open(Uri path) {
                this._path = path;
                this.Open();
            }

            /// <summary>
            /// Processes the response from the server.
            /// </summary>
            /// <param name="response">The raw response from the server.</param>
            private void ProcessResponse(string response) {
                try {
                    XmlDocument XmlDoc = new XmlDocument();
                    XmlDoc.LoadXml(response);
                    XmlNodeList XmlResponseList = XmlDoc.GetElementsByTagName("D:response");
                    WebDavHierarchyItem[] children = new WebDavHierarchyItem[XmlResponseList.Count];
                    int counter = 0;
                    foreach (XmlNode XmlCurrentResponse in XmlResponseList) {
                        WebDavHierarchyItem item = new WebDavHierarchyItem();

                        foreach (XmlNode XmlCurrentNode in XmlCurrentResponse.ChildNodes) {
                            switch (XmlCurrentNode.LocalName) {
                                case "href":
                                    string href = XmlCurrentNode.InnerText;
                                    if (!Regex.Match(href, "^https?:\\/\\/").Success) {
                                        href = this._path.Scheme + "://" + this._path.Host + href;
                                    }
                                    item.SetHref(href, this._path);
                                    break;

                                case "propstat":
                                    foreach (XmlNode XmlCurrentPropStatNode in XmlCurrentNode) {
                                        switch (XmlCurrentPropStatNode.LocalName) {
                                            case "prop":
                                                foreach (XmlNode XmlCurrentPropNode in XmlCurrentPropStatNode) {
                                                    switch (XmlCurrentPropNode.LocalName) {
                                                        case "creationdate":
                                                            item.SetCreationDate(XmlCurrentPropNode.InnerText);
                                                            break;

                                                        case "getcontentlanguage":
                                                            item.SetProperty(new Property(new PropertyName("getcontentlanguage", XmlCurrentPropNode.NamespaceURI), XmlCurrentPropNode.InnerText));
                                                            break;

                                                        case "getcontentlength":
                                                            item.SetProperty(new Property(new PropertyName("getcontentlength", XmlCurrentPropNode.NamespaceURI), XmlCurrentPropNode.InnerText));
                                                            break;
                                                        case "getcontenttype":
                                                            item.SetProperty(new Property(new PropertyName("getcontenttype", XmlCurrentPropNode.NamespaceURI), XmlCurrentPropNode.InnerText));
                                                            break;

                                                        case "getlastmodified":
                                                            item.SetLastModified(XmlCurrentPropNode.InnerText);
                                                            break;

                                                        case "resourcetype":
                                                            item.SetProperty(new Property(new PropertyName("resourcetype", XmlCurrentPropNode.NamespaceURI), XmlCurrentPropNode.InnerXml));
                                                            break;
                                                    }
                                                }
                                                break;

                                            case "status":
                                                //Console.WriteLine("status: " + XmlCurrentPropStatNode.InnerText);
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }

                        if (item.DisplayName != String.Empty) {
                            children[counter] = item;
                            counter++;
                        } else {
                            this.SetItemType(ItemType.Folder);
                            this.SetHref(item.Href.AbsoluteUri, item.Href);
                            this.SetCreationDate(item.CreationDate);
                            this.SetComment(item.Comment);
                            this.SetCreatorDisplayName(item.CreatorDisplayName);
                            this.SetLastModified(item.LastModified);
                            foreach (Property property in item.Properties) {
                                this.SetProperty(property);
                            }
                        }
                        //Console.WriteLine();
                    }

                    int childrenCount = 0;
                    foreach (IHierarchyItem item in children) {
                        if (item != null) {
                            childrenCount++;
                        }
                    }
                    this._children = new IHierarchyItem[childrenCount];

                    counter = 0;
                    foreach (IHierarchyItem item in children) {
                        if (item != null) {
                            this._children[counter] = item;
                            counter++;
                        }
                    }
                } catch (XmlException e) {
                    Console.WriteLine(e.Message);
                    Console.WriteLine(response);
                }
            }
		}
	}
}
