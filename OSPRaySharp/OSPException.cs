using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay
{
    internal class OSPException : Exception
    {
        public OSPException(int errorCode, string? message): base(message)
        {
            ErrorCode = errorCode;
        }

        public int ErrorCode { get; }
    }
}
