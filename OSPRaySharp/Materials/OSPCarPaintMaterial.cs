using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay.Materials
{
    /// <summary>
    /// The CarPaint material is a specialized version of the Principled material for rendering different types of car paints.
    /// </summary>
    public class OSPCarPaintMaterial : OSPMaterial
    {
        public OSPCarPaintMaterial() : base("carPaint")
        {
        }
    }
}
