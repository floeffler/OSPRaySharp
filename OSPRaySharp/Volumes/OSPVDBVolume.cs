using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay.Volumes
{
    /// <summary>
    /// VDB volumes implement a data structure that is very similar to the data structure outlined in 
    /// (Museth, K. VDB: High-Resolution Sparse Volumes with Dynamic Topology. ACM Transactions on Graphics 32(3), 2013. DOI: 10.1145/2487228.2487235)
    /// The data structure is a hierarchical regular grid at its core: Nodes are regular grids, 
    /// and each grid cell may either store a constant value (this is called a tile), or child pointers. 
    /// Nodes in VDB trees are wide: Nodes on the first level have a resolution of 32^3 voxels, on the next level 16^3, 
    /// and on the leaf level 8^3 voxels.
    /// </summary>
    public class OSPVDBVolume : OSPVolume
    {
        public OSPVDBVolume(): base("vdb")
        {
        }
    }
}
