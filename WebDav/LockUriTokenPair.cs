using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebDav {
    namespace Client {
        public class LockUriTokenPair {
            public readonly Uri Href;
            public readonly string lockToken;

            public LockUriTokenPair(Uri href, string lockToken) {
                
            }
        }
    }
}
