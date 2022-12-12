using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay.Materials
{
    /// <summary>
    /// The path tracer supports the Luminous material which emits light uniformly 
    /// in all directions and which can thus be used to turn any geometric object into a light source. 
    /// </summary>
    public class OSPLuminousMaterial : OSPMaterial
    {
        public OSPLuminousMaterial(): base("luminous")
        {
        }
    }
}
