using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using WebDav.Client;
	
namespace WebDav {
	namespace Client {
		public interface IHierarchyItem : IConnectionSettings {
			string Comment { get; }
			DateTime CreationDate { get; }
			string CreatorDisplayName { get; }
			string DisplayName { get; }
			Uri Href { get; }
			ItemType ItemType { get; }
			DateTime LastModified { get; }
			Property[] Properties { get; }

            Property[] GetAllProperties();
            PropertyName[] GetPropertyNames();
            Property[] GetPropertyValues(PropertyName[] names);
            void Delete();
		}
		
		public class WebDavHierarchyItem : WebDavConnectionSettings, IHierarchyItem {
            protected ICredentials _credentials = new NetworkCredential();

			private string _comment = "";
			private DateTime _creationDate = new DateTime(0);
			private string _creatorDisplayName = "";
			private Uri _href = null;
            private Uri _baseUri = null;
			private ItemType _itemType;
			private DateTime _lastModified = new DateTime(0);
			private Property[] _properties = {};
			
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
			public ItemType ItemType { get { return this._itemType; } }
			public DateTime LastModified { get { return this._lastModified; } }
			public Property[] Properties { get { return this._properties; } }

            public Property[] GetAllProperties() {
                return this._properties;
            }

            public PropertyName[] GetPropertyNames() {
                return this._properties.Select(p => p.Name).ToArray();
            }

            public Property[] GetPropertyValues(PropertyName[] names) {
                return (from p in this._properties from pn in names where pn.Equals(p.Name) select p).ToArray();
            }

            public void Delete() {
                NetworkCredential credentials = (NetworkCredential)this._credentials;
                string auth = "Basic " + Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(credentials.UserName + ":" + credentials.Password));
                WebRequest webRequest = HttpWebRequest.Create(this.Href);
                webRequest.Method = "DELETE";
                webRequest.Credentials = credentials;
                webRequest.Headers.Add("Authorization", auth);
                using (WebResponse webResponse = webRequest.GetResponse()) {
                    using (Stream responseStream = webResponse.GetResponseStream()) {
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

            public void Delete(LockUriTokenPair[] lockTokens) {
                
            }

            public void Delete(string lockToken) {
                
            }

			public void SetComment (string comment) {
				this._comment = comment;
			}
			
			public void SetCreationDate (string creationDate) {
				this._creationDate = DateTime.Parse(creationDate);
			}
			
			public void SetCreationDate (DateTime creationDate) {
				this._creationDate = creationDate;
			}
			
			public void SetCreatorDisplayName (string creatorDisplayName) {
				this._creatorDisplayName = creatorDisplayName;
			}
			
			public void SetItemType (ItemType itemType) {
				this._itemType = itemType;
			}
			
			public void SetHref (string href, Uri baseUri) {
				this._href = new Uri(href);
                this._baseUri = baseUri;
			}
			
			public void SetLastModified (string lastModified) {
				this._lastModified = DateTime.Parse(lastModified);
			}
			
			public void SetLastModified (DateTime lastModified) {
				this._lastModified = lastModified;
			}
			
			public void SetProperty (Property property) {
				if (property.Name.Name == "resourcetype" && property.StringValue != String.Empty) {
					XmlDocument XmlDoc = new XmlDocument();
					try {
                        if (property.StringValue == "collection") {
                            this._itemType = ItemType.Folder;
                        } else {
                            XmlDoc.LoadXml(property.StringValue);
                            property.StringValue = XmlDoc.DocumentElement.LocalName;
                            switch (property.StringValue) {
                                case "collection":
                                    this._itemType = ItemType.Folder;
                                    break;

                                default:
                                    Console.WriteLine("unknown resourcetype: " + property.StringValue);
                                    break;
                            }
                        }
					} catch(XmlException e) {
						Console.WriteLine("WebDavHierarchyItem.SetProperty()-XmlException: " + e.Message);
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
					newProperties[this._properties.Length] = property;
					this._properties = newProperties;
				}
			}
			
			public void SetProperty (PropertyName propertyName, string value) {
				this.SetProperty(new Property(propertyName, value));
			}
			
			public void SetProperty (string name, string nameSpace, string value) {
				this.SetProperty(new Property(name, nameSpace, value));
			}

            public void SetCredentials(ICredentials credentials) {
                this._credentials = credentials;
            }
		}
	}
}
