using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay.TestSuite.Render
{
    internal static class PixelHelper
    {
        public static byte ToSRGB(float x)
        {
            float value;
            if (x > 0.0031308f)
                value = 1.055f * (float)Math.Pow(x, 1 / 2.4) - 0.055f;
            else
                value = 12.92f * x;

            value = Math.Clamp(value, 0, 1);
            return (byte)(value * 255);
        }


    }
}
