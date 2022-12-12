using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay.Renderers
{
    /// <summary>
    /// The SciVis renderer is a fast ray tracer for scientific visualization 
    /// which supports volume rendering and ambient occlusion (AO).
    /// </summary>
    public class OSPSciVisRenderer : OSPRenderer
    {
        public OSPSciVisRenderer(): base("scivis")
        {
        }

        public void SetShadows(bool shadows) => SetParam("shadows", shadows);
        public void SetAOSamples(int aoSamples) => SetParam("aoSamples", aoSamples);
        public void SetAODistance(float aoDistance) => SetParam("aoDistance", aoDistance);
        public void SetVolumeSamplingRate(float volumeSamplingRate) => SetParam("volumeSamplingRate", volumeSamplingRate);
        public void SetVisibleLights(float visibleLights) => SetParam("visibleLights", visibleLights);
    }
}
