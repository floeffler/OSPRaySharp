using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay
{
    internal class OSPLightHandle : OSPObjectHandle
    {
    }

    public abstract class OSPLight : OSPObject
    {
        private OSPLightHandle handle;

        public OSPLight(string type): base()
        {
            handle = NativeMethods.ospNewLight(type);
            if (handle.IsInvalid)
                OSPDevice.CheckLastDeviceError();
        }

        public void SetColor(Vector3 color) => SetParam("color", color);
        public void SetVisible(bool visible) => SetParam("visible", visible);
        public void SetIntensity(float intensity) => SetParam("intensity", intensity);
        public void SetIntensityQuantity(OSPIntensityQuantity intensityQuantity) => SetParam("intensityQuantity", intensityQuantity);


        internal override OSPObjectHandle Handle => handle;
    }
}
