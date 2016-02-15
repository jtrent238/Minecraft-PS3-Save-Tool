using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBTSharp
{
    public class FailedWriteException : Exception
    {
        public FailedWriteException(String message) : base(message)
        {
        }

        public FailedWriteException(String message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
