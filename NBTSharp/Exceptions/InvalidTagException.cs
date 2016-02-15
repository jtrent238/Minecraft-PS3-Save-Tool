using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBTSharp
{
    public class InvalidTagException : Exception
    {
        public InvalidTagException(String message) : base(message)
        {
        }

        public InvalidTagException(String message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
