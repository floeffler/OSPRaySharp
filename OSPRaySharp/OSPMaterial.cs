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

        public void SetTexture(string attribName, OSPTexture texture) => SetObjectParam($"map_{attribName}", texture);
        public void SetTextureTransform(string attribName, AffineSpace3F transform) => SetParam($"map_{attribName}.transform", transform);
        public void SetTextureTransform(string attribName, AffineSpace2F transform) => SetParam($"map_{attribName}.transform", transform);

        internal override OSPObjectHandle Handle => handle;
    }
}
