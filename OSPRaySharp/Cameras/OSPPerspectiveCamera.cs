using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay.Cameras
{
    /// <summary>
    /// The perspective camera implements a simple thin lens camera for perspective rendering, 
    /// supporting optionally depth of field and stereo rendering (with the path tracer).
    /// </summary>
    public class OSPPerspectiveCamera : OSPCamera
    {
        public OSPPerspectiveCamera(): base("perspective")
        {
        }

        public void SetFovY(float fovy) => SetParam("fovy", fovy);
        public void SetAspect(float aspect) => SetParam("aspect", aspect);
        public void SetApertureRadius(float apertureRadius) => SetParam("apertureRadius", apertureRadius);
        public void SetFocusDistance(float focusDistance) => SetParam("focusDistance", focusDistance);
        public void SetArchitectural(bool architectural) => SetParam("architectural", architectural);
        public void SetInterpupillaryDistance(float interpupillaryDistance) => SetParam("interpupillaryDistance", interpupillaryDistance);
        public void SetStereoMode(OSPStereoMode stereoMode) => SetParam("stereoMode", stereoMode);
    }
}
