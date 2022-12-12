using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay.Renderers
{
    /// <summary>
    /// The path tracer supports soft shadows, indirect illumination and realistic materials.
    /// </summary>
    public class OSPPathTracer : OSPRenderer
    {
        public OSPPathTracer(): base("pathtracer")
        {
        }

        public void SetBackgroundRefraction(bool backgroundRefraction) => SetParam("backgroundRefraction", backgroundRefraction);
        public void SetRoulettePathLength(int roulettePathLength) => SetParam("roulettePathLength", roulettePathLength);
        public void SetLightSamples(int lightSamples) => SetParam("lightSamples", lightSamples);
    }
}
