using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay.Lights
{
    /// <summary>
    /// The cylinder light is a cylinderical, procedural area light source emitting uniformly 
    /// outwardly into the space beyond the boundary. 
    /// </summary>
    public class OSPCylinderLight : OSPLight
    {
        public OSPCylinderLight() : base("cylinder")
        {
        }

        public void SetStartPosition(Vector3 startPosition) => SetParam("position0", startPosition);
        public void SetEndPosition2(Vector3 endPosition) => SetParam("position1", endPosition);
        public void SetRadius(float radius) => SetParam("radius", radius);
    }
}
