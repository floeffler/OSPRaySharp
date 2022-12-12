using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay
{
    internal class OSPMaterialHandle : OSPObjectHandle
    {
    }

    public abstract class OSPMaterial : OSPObject
    {
        private OSPMaterialHandle handle;

        public OSPMaterial(string type)
        {
            handle = NativeMethods.ospNewMaterial(null, type);
            if (handle.IsInvalid)
                OSPDevice.ThrowLastError();
        }

        internal override OSPObjectHandle Handle => handle;
    }
}
