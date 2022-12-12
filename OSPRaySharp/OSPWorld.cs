using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay
{
    internal class OSPWorldHandle : OSPObjectHandle
    {
    }

    public class OSPWorld : OSPObject
    {
        private OSPWorldHandle handle;

        public OSPWorld()
        {
            handle = NativeMethods.ospNewWorld();
            if (handle.IsInvalid)
                OSPDevice.ThrowLastError();
        }

        internal override OSPObjectHandle Handle => handle;
    }
}
