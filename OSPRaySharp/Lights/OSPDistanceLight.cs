using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay.Lights
{
    /// <summary>
    /// The distant light (or traditionally the directional light) is thought to be far away (outside of the scene), 
    /// thus its light arrives (almost) as parallel rays.
    /// </summary>
    public class OSPDistanceLight : OSPLight
    {
        public OSPDistanceLight() : base("distance")
        {
        }

        public void SetDirection(Vector3 direction) => SetParam("direction", direction);
        public void SetAngularDiameter(float angularDiameter) => SetParam("angularDiameter", angularDiameter);
    }
}
