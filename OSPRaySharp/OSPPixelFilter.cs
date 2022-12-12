using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay
{
    /// <summary>
    /// OSPRay pixel filter types
    /// </summary>
    public enum OSPPixelFilter
    {
        Point,
        Box,
        Gaussian,
        Mitchell,
        BlackmanHarris,
    }
}
