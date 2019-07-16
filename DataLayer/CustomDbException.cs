using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace DataLayer
{
    [Serializable]
    internal class CustomDbException : Exception
    {
        private string message;
        private Exception ex;
        private string v;

        public CustomDbException()
        {
        }

        public CustomDbException(string message) : base(message)
        {
        }

        public CustomDbException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public CustomDbException(string message, Exception ex, string v)
        {
            this.message = message;
            this.ex = ex;
            this.v = v;
        }

        protected CustomDbException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}

