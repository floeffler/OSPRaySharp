using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay.Materials
{
    /// <summary>
    /// The path tracer offers a realistic a glass material, supporting refraction and volumetric attenuation
    /// (i.e., the transparency color varies with the geometric thickness).
    /// </summary>
    public class OSPGlassMaterial : OSPMaterial
    {
        public OSPGlassMaterial() : base("glass")
        {
        }
    }
}
