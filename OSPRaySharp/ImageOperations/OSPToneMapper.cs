using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay.ImageOperations
{
    public class OSPToneMapper : OSPImageOperation
    {
        public OSPToneMapper(): base("tonemapper")
        {
        }

        public void SetExposure(float exposure) => SetParam("exposure", exposure);
        public void SetContrast(float contrast) => SetParam("contrast", contrast);
        public void SetShoulder(float shoulder) => SetParam("shoulder", shoulder);
        public void SetMidIn(float midIn) => SetParam("midIn", midIn);
        public void SetMidOut(float midOut) => SetParam("midOut", midOut);
        public void SetHdrMax(float hdrMax) => SetParam("hdrMax", hdrMax);
        public void SetAcesColor(bool acesColor) => SetParam("acesColor", acesColor);

    }
}
