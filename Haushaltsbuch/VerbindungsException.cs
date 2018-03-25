using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haushaltsbuch
{
    [System.Serializable]
    public class VerbindungsException : Exception
    {
        public VerbindungsException() { }
        public VerbindungsException(string message) : base(message) { }
        public VerbindungsException(string message, Exception inner) : base(message, inner) { }
        protected VerbindungsException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }        
}
