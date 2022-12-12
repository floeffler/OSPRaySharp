using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay.Renderers
{
    /// <summary>
    /// This renderer supports only a subset of the features of the SciVis renderer to gain performance. 
    /// As the name suggest its main shading method is ambient occlusion (AO), 
    /// lights are not considered at all and , Volume rendering is supported.
    /// </summary>
    public class OSPAmbientOcclusionRenderer : OSPRenderer
    {
        public OSPAmbientOcclusionRenderer(): base("ao")
        {
        }

        public void SetAOSamples(int aoSamples) => SetParam("aoSamples", aoSamples);
        public void SetAODistance(float aoDistance) => SetParam("aoDistance", aoDistance);
        public void SetAOIntensity(float aoIntensity) => SetParam("aoIntensity", aoIntensity);
        public void SetVolumeSamplingRate(float volumeSamplingRate) => SetParam("volumeSamplingRate", volumeSamplingRate);

    }
}
