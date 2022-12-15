using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay.Volumes
{
    /// <summary>
    /// Unstructured volumes can have their topology and geometry freely defined. 
    /// Geometry can be composed of tetrahedral, hexahedral, wedge or pyramid cell types. 
    /// The data format used is compatible with VTK and consists of multiple arrays: 
    /// vertex positions and values, vertex indices, cell start indices, cell types, and cell values.
    /// </summary>
    public class OSPUnstructuredVolume : OSPVolume
    {
        public OSPUnstructuredVolume(): base("unstructured")
        {
        }
    }
}
