using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay.Lights
{
    /// <summary>
    /// The quad light is a planar, procedural area light source emitting uniformly on one side into the half-space. 
    /// </summary>
    public class OSPQuadLight : OSPLight
    {
        public OSPQuadLight() : base("quad")
        {
        }

        public void SetPosition(Vector3 position) => SetParam("position", position);
        public void SetEdge1(Vector3 edge1) => SetParam("edge1", edge1);
        public void SetEdge2(Vector3 edge2) => SetParam("edge2", edge2);

    }
}
