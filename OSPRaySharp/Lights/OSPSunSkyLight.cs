using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay.Lights
{
    /// <summary>
    /// The sun-sky light is a combination of a distant light for the sun and a procedural hdri light for the sky. 
    /// </summary>
    public class OSPSunSkyLight : OSPLight
    {
        public OSPSunSkyLight(): base("sunSky")
        {
        }

        public void SetUp(Vector3 up) => SetParam("up", up);
        public void SetDirection(Vector3 direction) => SetParam("direction", direction);
        public void SetTurbidity(float turbidity) => SetParam("turbidity", turbidity);
        public void SetAlbedo(float albedo) => SetParam("albedo", albedo);
        public void SetHorizonExtension(float horizonExtension) => SetParam("horizonExtension", horizonExtension);
    }
}
