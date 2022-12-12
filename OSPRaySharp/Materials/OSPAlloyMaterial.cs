using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay.Materials
{
    /// <summary>
    /// The path tracer offers an alloy material, which behaves similar to Metal, 
    /// but allows for more intuitive and flexible control of the color.
    /// </summary>
    public class OSPAlloyMaterial : OSPMaterial
    {
        public OSPAlloyMaterial(): base("alloy")
        {
        }
    }
}
