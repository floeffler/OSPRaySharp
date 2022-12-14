using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay
{
    internal class OSPVolumeHandle : OSPObjectHandle
    {
        public new static readonly OSPVolumeHandle Empty = new OSPVolumeHandle();
    }

    /// <summary>
    /// Volumes are volumetric data sets with discretely sampled values in 3D space, typically a 3D scalar field. 
    /// </summary>
    public abstract class OSPVolume :  OSPObject
    {
        private OSPVolumeHandle handle;

        public OSPVolume(string type): base()
        {
            handle = NativeMethods.ospNewVolume(type);
            if (handle.IsInvalid)
                OSPDevice.ThrowLastError();
        }

        internal override OSPObjectHandle Handle => handle;
    }
}
