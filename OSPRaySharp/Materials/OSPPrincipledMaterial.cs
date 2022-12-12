using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay.Materials
{
    /// <summary>
    /// The Principled material is the most complex material offered by the path tracer, 
    /// which is capable of producing a wide variety of materials (e.g., plastic, metal, wood, glass) 
    /// by combining multiple different layers and lobes. It uses the GGX microfacet distribution 
    /// with approximate multiple scattering for dielectrics and metals, uses the Oren-Nayar model 
    /// for diffuse reflection, and is energy conserving.
    /// </summary>
    public class OSPPrincipledMaterial : OSPMaterial
    {
        public OSPPrincipledMaterial(): base("principled")
        {
        }
    }
}
