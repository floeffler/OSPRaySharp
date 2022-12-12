using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay.Geometries
{
    /// <summary>
    /// OSPRay can directly render multiple isosurfaces of a volume without first tessellating them. 
    /// </summary>
    public class OSPIsoSurfaceGeometry : OSPGeometry
    {
        public OSPIsoSurfaceGeometry() : base("isosurface")
        {
        }

        // TODO: add properties
    }
}
