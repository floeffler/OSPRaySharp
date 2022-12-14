using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay
{
    internal class OSPTextureHandle : OSPObjectHandle
    {
    }

    public class OSPTexture : OSPObject
    {
        private OSPTextureHandle handle;

        public OSPTexture(string type)
        {
            handle = NativeMethods.ospNewTexture(type);
            if (handle.IsInvalid)
                OSPDevice.ThrowLastError();
        }

        internal override OSPObjectHandle Handle => handle;
    }
}
