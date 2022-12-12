using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay.Materials
{
    /// <summary>
    /// The path tracer offers a physical metal, supporting changing roughness and realistic color shifts at edges.
    /// </summary>
    public class OSPMetalMaterial : OSPMaterial
    {
        public OSPMetalMaterial() : base("metal")
        {
        }
    }
}
