using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay
{
    internal class OSPGroupHandle : OSPObjectHandle
    {
        public new static readonly OSPGroupHandle Empty = new OSPGroupHandle();
    }

    /// <summary>
    /// Groups in OSPRay represent collections of GeometricModels, 
    /// VolumetricModels and Lights which share a common local-space coordinate system.
    /// </summary>
    public class OSPGroup : OSPObject
    {
        private OSPGroupHandle handle;

        public OSPGroup(): base()
        {
            handle = NativeMethods.ospNewGroup();
            if (handle.IsInvalid)
                OSPDevice.ThrowLastError();
        }

        public void SetGeometry(params OSPGeometricModel[] geometry) => SetObjectArrayParam("geometry", geometry);
        public void SetVolume(params OSPVolumetricModel[] volume) => SetObjectArrayParam("volume", volume);
        public void SetClippingGeometry(params OSPGeometricModel[] clippingGeometry) => SetObjectArrayParam("clippingGeometry", clippingGeometry);
        public void SetLight(params OSPLight[] lights) => SetObjectArrayParam("lights", lights);
        public void SetDynamicScene(bool dynamicScene) => SetParam("dynamicScene", dynamicScene);
        public void SetCompactMode(bool compactMode) => SetParam("compactMode", compactMode);
        public void SetRobustMode(bool robustMode) => SetParam("robustMode", robustMode);

        // TODO: volumetric models

        internal override OSPObjectHandle Handle => handle;
    }
}
