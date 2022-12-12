using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay.Geometries
{
    /// <summary>
    /// OSPRay can directly render planes defined by plane equation coefficients in its implicit form ax + by + cz + d = 0. 
    /// By default planes are infinite but their extents can be limited by defining optional bounding boxes.
    /// </summary>
    public class OSPPlaneGeometry : OSPGeometry
    {
        public OSPPlaneGeometry() : base("plane")
        {
        }

        // TODO: add properties
    }
}
