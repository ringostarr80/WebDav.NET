using System;
using WebDav.Client;

namespace WebDav {
	namespace Client {
		public class Property {
			private string _value = "";
			
			public readonly PropertyName Name;
			public string StringValue { get { return this._value; } set { this._value = value; } }
			
			public Property (PropertyName name, string value) {
				this.Name = name;
				this.StringValue = value;
			}
			
			public Property (string name, string nameSpace, string value) {
				this.Name = new PropertyName(name, nameSpace);
				this.StringValue = value;
			}
			
			public override string ToString() {
				return StringValue;
			}
		}
	}
}

