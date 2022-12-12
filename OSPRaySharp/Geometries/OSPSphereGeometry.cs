using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay.Geometries
{
    /// <summary>
    /// A geometry consisting of individual spheres, each of which can have an own radius.
    /// </summary>
    public class OSPSphereGeometry : OSPGeometry
    {
        public OSPSphereGeometry(): base("sphere")
        {
        }

        public void SetRadius(float radius) => SetParam("radius", radius);
        public void SetRadii(float[] radii) => SetArrayParam("sphere.radius", radii);
        public void SetPositions(Vector3[] positions) => SetArrayParam("sphere.position", positions);

    }
}
