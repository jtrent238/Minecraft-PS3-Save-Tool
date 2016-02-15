using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBTSharp
{
    public class InvalidStringException : Exception
    {
        public InvalidStringException(String message) : base(message)
        {
        }

        public InvalidStringException(String message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
