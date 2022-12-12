using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay.Materials
{
    /// <summary>
    /// The path tracer offers a thin glass material useful for objects with just a single surface, 
    /// most prominently windows. It models a thin, transparent slab, i.e.,
    /// it behaves as if a second, virtual surface is parallel to the real geometric surface. 
    /// </summary>
    public class OSPThinGlassMaterial : OSPMaterial
    {
        public OSPThinGlassMaterial() : base("thinGlass")
        {
        }
    }
}
