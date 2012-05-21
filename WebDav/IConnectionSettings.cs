using System;

namespace WebDav {
	namespace Client {
		public interface IConnectionSettings {
			bool AllowWriteStreamBuffering { get; set; }
			bool SendChunked { get; set; }
			int TimeOut { get; set; }
		}
		
		public class WebDavConnectionSettings {
			private bool _allowWriteStreamBuffering = false;
			private bool _sendChunked = false;
			private int _timeOut = 30;
			
			public bool AllowWriteStreamBuffering { get { return this._allowWriteStreamBuffering; } set { this._allowWriteStreamBuffering = value; } }
			public bool SendChunked { get { return this._sendChunked; } set { this._sendChunked = value; } }
			public int TimeOut { get { return this._timeOut; } set { this._timeOut = value; } }
		}
	}
}
