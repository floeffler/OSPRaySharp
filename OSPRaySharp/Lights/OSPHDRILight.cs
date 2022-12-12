using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay.Lights
{
    /// <summary>
    /// The HDRI light is a textured light source surrounding the scene and illuminating it from infinity.
    /// </summary>
    public class OSPHDRILight : OSPLight
    {
        public OSPHDRILight() : base("hdri")
        {
        }

        public void SetUp(Vector3 up) => SetParam("up", up);
        public void SetDirection(Vector3 direction) => SetParam("direction", direction);

        //TODO: texture map
    }
}
