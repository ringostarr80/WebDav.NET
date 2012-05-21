using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WebDav {
    namespace Client {
        public interface IItemContent {
            long ContentLength { get; }
            string ContentType { get; }

            void Download(string filename);
            void Upload(string filename);
            Stream GetReadStream();
            Stream GetWriteStream(long contentLength);
            Stream GetWriteStream(string contentType, long contentLength);
        }
    }
}
