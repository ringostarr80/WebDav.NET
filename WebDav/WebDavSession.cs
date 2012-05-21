using System;
using System.Net;

/// <summary>
/// WebDav Namespace.
/// </summary>
namespace WebDav {
	/// <summary>
	/// WebDav.Client Namespace.
	/// </summary>
	/// <see cref="http://doc.webdavsystem.com/ITHit.WebDAV.Client.html"/>
	namespace Client {
		public class WebDavSession {
            public ICredentials Credentials { get; set; }
			
            /// <summary>
            /// Constructor for WebDAV session.
            /// </summary>
			public WebDavSession () {
				
			}
			
            /// <summary>
            /// Returns IFolder corresponding to path.
            /// </summary>
            /// <param name="path">Path to the folder.</param>
            /// <returns>Folder corresponding to requested path.</returns>
			public IFolder OpenFolder (string path) {
				WebDavFolder folder = new WebDavFolder();
                folder.SetCredentials(this.Credentials);
				folder.Open(path);
				return folder;
			}
			
            /// <summary>
            /// Returns IFolder corresponding to path.
            /// </summary>
            /// <param name="path">Path to the folder.</param>
            /// <returns>Folder corresponding to requested path.</returns>
			public IFolder OpenFolder (Uri path) {
				WebDavFolder folder = new WebDavFolder();
                folder.SetCredentials(this.Credentials);
				folder.Open(path);
				return folder;
			}

            /// <summary>
            /// Returns IResource corresponding to path.
            /// </summary>
            /// <param name="path">Path to the resource.</param>
            /// <returns>Resource corresponding to requested path.</returns>
            public IResource OpenResource(string path) {
                return this.OpenResource(new Uri(path));
            }

            /// <summary>
            /// Returns IResource corresponding to path.
            /// </summary>
            /// <param name="path">Path to the resource.</param>
            /// <returns>Resource corresponding to requested path.</returns>
            public IResource OpenResource(Uri path) {
                IFolder folder = this.OpenFolder(path);
                return folder.GetResource(path.Segments[path.Segments.Length - 1]);
            }
		}
	}
}
