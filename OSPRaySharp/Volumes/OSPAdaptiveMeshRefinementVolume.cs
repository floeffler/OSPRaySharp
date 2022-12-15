using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace OSPRay.Volumes
{
    /// <summary>
    /// OSPRay currently supports block-structured (Berger-Colella) AMR volumes. 
    /// Volumes are specified as a list of blocks, which exist at levels of refinement in potentially 
    /// overlapping regions. Blocks exist in a tree structure, with coarser refinement level blocks 
    /// containing finer blocks. The cell width is equal for all blocks at the same refinement level,
    /// though blocks at a coarser level have a larger cell width than finer levels.
    /// There can be any number of refinement levels and any number of blocks at any level of refinement.
    /// </summary>
    public class OSPAdaptiveMeshRefinementVolume : OSPVolume
    {
        public OSPAdaptiveMeshRefinementVolume(): base("amr")
        {
        }
    }
}
