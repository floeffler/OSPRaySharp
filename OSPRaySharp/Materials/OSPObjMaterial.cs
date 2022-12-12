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
        public void SetSpecular(Vector3 ks) => SetParam("ks", ks);
        public void SetTransparentFilterColor(Vector3 tf) => SetParam("tf", tf);
        public void SetOpacity(float d) => SetParam("d", d);
        public void SetShininess(float ns) => SetParam("ns", ns);


        //TODO: bump map

    }
}
