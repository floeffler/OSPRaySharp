using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay.Materials
{
    /// <summary>
    /// The OBJ material is the workhorse material supported by both the SciVis renderer
    /// and the path tracer (the Ambient Occlusion renderer only uses the kd and d parameter).
    /// </summary>
    public class OSPObjMaterial : OSPMaterial
    {
        public OSPObjMaterial(): base("obj")
        {
        }

        public void SetDiffuse(Vector3 kd) => SetParam("kd", kd);
        public void SetDiffuse(OSPTexture texture) => SetTexture("kd", texture);
        public void SetSpecular(Vector3 ks) => SetParam("ks", ks);
        public void SetSpecular(OSPTexture texture) => SetTexture("ks", texture);
        public void SetTransparentFilterColor(Vector3 tf) => SetParam("tf", tf);
        public void SetOpacity(float d) => SetParam("d", d);
        public void SetOpacity(OSPTexture texture) => SetTexture("d", texture);
        public void SetShininess(float ns) => SetParam("ns", ns);
        public void SetShininess(OSPTexture texture) => SetTexture("ns", texture);
        public void SetNormalMap(OSPTexture normalMap) => SetObjectParam("map_bump", normalMap);
        
    }
}
