using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay.Geometries
{
    /// <summary>
    /// OSPRay can directly render axis-aligned bounding boxes without the need to convert them to quads or triangles.
    /// </summary>
    public class OSPBoxGeometry : OSPGeometry
    {
        public OSPBoxGeometry() : base("box")
        {
        }

        public void SetBox(params OSPBounds[] boxes) => SetArrayParam("box", boxes);
    }
}
