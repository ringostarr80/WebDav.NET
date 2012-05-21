using System;

namespace WebDav {
	namespace Client {
		public sealed class PropertyName {
			public readonly string Name;
			public readonly string NamespaceUri;
			
			public PropertyName (string name, string namespaceUri) {
				this.Name = name;
				this.NamespaceUri = namespaceUri;
			}
			
			public override bool Equals(object obj) {
				if (obj.GetType() == this.GetType()) {
					if (((PropertyName)obj).Name == this.Name && ((PropertyName)obj).NamespaceUri == this.NamespaceUri) {
						return true;
					}
				}
				
				return false;
			}
			
			public override int GetHashCode() {
				return base.GetHashCode();
			}
			
			public override string ToString () {
				 return this.Name;
			}
		}
	}
}

