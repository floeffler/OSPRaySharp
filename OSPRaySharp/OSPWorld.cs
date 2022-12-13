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

        public void SetInstances(params OSPInstance[] instances) => SetObjectArrayParam("instance", instances);
        public void SetLights(params OSPLight[] lights) => SetObjectArrayParam("light", lights);
        public void SetDynamicScene(bool dynamicScene) => SetParam("dynamicScene", dynamicScene);
        public void SetCompactMode(bool compactMode) => SetParam("compactMode", compactMode);
        public void SetRobustMode(bool robustMode) => SetParam("robustMode", robustMode);

        internal override OSPObjectHandle Handle => handle;
    }
}
