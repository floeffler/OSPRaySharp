using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay.Materials
{
    /// <summary>
    /// The path tracer offers a metallic paint material, 
    /// consisting of a base coat with optional flakes and a clear coat.
    /// </summary>
    public class OSPMetallicPaintMaterial : OSPMaterial
    {
        public OSPMetallicPaintMaterial() : base("metallicPaint")
        {
        }
    }
}
