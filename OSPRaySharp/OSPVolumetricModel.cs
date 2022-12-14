using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay
{
    internal class OSPVolumetricModelHandle : OSPObjectHandle
    {
    }

    public class OSPVolumetricModel : OSPObject
    {
        private OSPVolumetricModelHandle handle;

        public OSPVolumetricModel(OSPVolume? volume)
        {
            OSPVolumeHandle volumeHandle = OSPVolumeHandle.Empty;
            if (volume != null)
            {
                volumeHandle = (OSPVolumeHandle)volume.Handle;
            }
            handle = NativeMethods.ospNewVolumetricModel(volumeHandle);
            if (handle.IsInvalid)
                OSPDevice.ThrowLastError();
        }

        public void SetVolume(OSPVolume volume) => SetObjectParam("volume", volume);
        public void SetTransferFunction(OSPTransferFunction transferFunction) => SetObjectParam("transferFunction", transferFunction);

        public void SetDensityScale(float densityScale) => SetParam("densityScale", densityScale);
        public void SetAnisotropy(float anisotropy) => SetParam("anisotropy", anisotropy);
        public void SetId(uint id) => SetParam("id", id);


        internal override OSPObjectHandle Handle => handle;
    }
}
