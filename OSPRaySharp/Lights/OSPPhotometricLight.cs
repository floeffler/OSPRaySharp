using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay.Lights
{
    /// <summary>
    /// Measured light sources (IES, EULUMDAT, …) are supported by the sphere, spot, and quad lights
    /// when setting an intensityDistribution data array to modulate the intensity per direction.
    /// </summary>
    public abstract class OSPPhotometricLight : OSPLight
    {
        public OSPPhotometricLight(string type): base(type)
        {
        }

        public void SetIntensityDistribution(float[] intensityDistribution) => SetArrayParam("intensityDistribution", intensityDistribution);
        public void SetC0(Vector3 c0) => SetParam("c0", c0);
    }
}
