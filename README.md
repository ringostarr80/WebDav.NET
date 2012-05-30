## WebDav.NET - the open source WebDav client library for the Microsoft .NET Framework

### first and simple example:

	using System;
	using WebDav.Client;
	
	namespace WebDavTest {
		class MainClass {
			public static void Main (string[] args) {
				WebDavSession session = new WebDavSession();
				IFolder folder = session.OpenFolder("http://localhost/webdav/");
				IHierarchyItem[] children = folder.GetChildren();
				foreach(IHierarchyItem child in children) {
					Console.WriteLine("'{0}' is a {1}", child.DisplayName, child.ItemType);
				}
			}
		}
	}

### a second, more practical example with authorization and encrypted https-connection:
	using System;
	using System.Net;
	using WebDav.Client;
	
	namespace WebDavTest {
		class MainClass {
			public static void Main (string[] args) {
				WebDavSession session = new WebDavSession();
				session.Credentials = new NetworkCredential("name@gmx.de", "password");
				IFolder folder = session.OpenFolder("https://mediacenter.gmx.net/");
				IHierarchyItem[] children = folder.GetChildren();
				foreach(IHierarchyItem child in children) {
					Console.WriteLine("'{0}' is a {1}", child.DisplayName, child.ItemType);
				}
				Console.WriteLine();
			}
		}
	}