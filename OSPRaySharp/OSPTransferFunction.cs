using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay
{
    internal class OSPTransferFunctionHandle : OSPObjectHandle
    {
    }

    public abstract class OSPTransferFunction : OSPObject
    {
        private OSPTransferFunctionHandle handle;

        public OSPTransferFunction(string type): base()
        {
            handle = NativeMethods.ospNewTransferFunction(type);
            if (handle.IsInvalid)
                OSPDevice.ThrowLastError();
        }

        internal override OSPObjectHandle Handle => handle;
    }
}
