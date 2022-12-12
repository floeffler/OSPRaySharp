using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay.Cameras
{
    /// <summary>
    /// The orthographic camera implements a simple camera with orthographic projection, without support for depth.
    /// </summary>
    public class OSPOrthographicCamera : OSPCamera
    {
        public OSPOrthographicCamera() : base("orthographic")
        {
        }

        public void SetHeight(float height) => SetParam("height", height);
        public void SetAspect(float aspect) => SetParam("aspect", aspect);
    }
}
