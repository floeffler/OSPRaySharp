using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay.Lights
{
    /// <summary>
    /// The sphere light (or the special case point light) is a light emitting uniformly in all directions 
    /// from the surface toward the outside. It does not emit any light toward the inside of the sphere.
    /// </summary>
    public class OSPSphereLight : OSPPhotometricLight
    {
        public OSPSphereLight() : base("sphere")
        {
        }

        public void SetPosition(Vector3 position) => SetParam("position", position);
        public void SetRadius(float radius) => SetParam("radius", radius);
        public void SetDirection(Vector3 direction) => SetParam("direction", direction);
    }
}
