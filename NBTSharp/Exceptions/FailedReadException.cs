using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBTSharp
{
    public class FailedReadException : Exception
    {
        public FailedReadException(String message) : base(message)
        {
        }

        public FailedReadException(String message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
