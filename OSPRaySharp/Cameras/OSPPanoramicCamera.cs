using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay.Cameras
{
    /// <summary>
    /// The panoramic camera implements a simple camera with support for stereo rendering. 
    /// It captures the complete surrounding with a latitude / longitude mapping and thus 
    /// the rendered images should best have a ratio of 2:1.
    /// </summary>
    public class OSPPanoramicCamera : OSPCamera
    {
        public OSPPanoramicCamera() : base("panoramic")
        {
        }

        public void SetInterpupillaryDistance(float interpupillaryDistance) => SetParam("interpupillaryDistance", interpupillaryDistance);
        public void SetStereoMode(OSPStereoMode stereoMode) => SetParam("stereoMode", stereoMode);
    }
}
