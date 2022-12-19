using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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

        public void SetEta(float eta) => SetParam("eta", eta);
        public void SetAttenuationColor(Vector3 attenuationColor) => SetParam("attenuationColor", attenuationColor);
        public void SetAttenuationDistance(float attenuationDistance) => SetParam("attenuationDistance", attenuationDistance);
        public void SetThickness(float thickness) => SetParam("thickness", thickness);
    }
}
