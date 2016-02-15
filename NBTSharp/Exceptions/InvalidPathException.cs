using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBTSharp
{
    public class InvalidPathException : Exception
    {
        public InvalidPathException(String message) : base(message)
        {
        }

        public InvalidPathException(String message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
