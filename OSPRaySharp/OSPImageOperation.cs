using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay
{
    internal class OSPImageOperationHandle : OSPObjectHandle
    {
    }

    public abstract class OSPImageOperation : OSPObject
    {
        private OSPImageOperationHandle handle;

        public OSPImageOperation(string type)
        {
            handle = NativeMethods.ospNewImageOperation(type);
            if (handle.IsInvalid)
                OSPDevice.ThrowLastError();
        }

        internal override OSPObjectHandle Handle => handle;
    }
}
