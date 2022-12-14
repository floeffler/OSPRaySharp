using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay.Volumes
{
    /// <summary>
    /// Structured volumes only need to store the values of the samples, 
    /// because their addresses in memory can be easily computed from a 3D position.
    /// Structured volumes are represented through an OSPData 3D array data 
    /// (which may or may not be shared with the application). The voxel data must be laid out 
    /// in xyz-order2 and can be compact (best for performance) or can have a stride between voxels, 
    /// specified through the byteStride1 parameter when creating the OSPData. 
    /// Only 1D strides are supported, additional strides between scanlines (2D, byteStride2) 
    /// and slices (3D, byteStride3) are not.
    /// </summary>
    public abstract class OSPStructuredVolume : OSPVolume
    {
        public OSPStructuredVolume(string type) : base(type)
        {
        }

        public void SetGridOrgin(Vector3 gridOrigin) => SetParam("gridOrigin", gridOrigin);
        public void SetGridSpacing(Vector3 gridSpacing) => SetParam("gridSpacing", gridSpacing);
        public void SetData<T>(OSPData<T> data) where T : unmanaged => SetObjectParam("data", data);
        public void SetCellCentered(bool cellCentered) => SetParam("cellCentered", cellCentered);
        public void SetFilter(OSPVolumeFilter filter) => SetParam("filter", filter);
        public void SetGradientFilter(OSPVolumeFilter gradientFilter) => SetParam("gradientFilter", gradientFilter);
        public void SetBackground(float background) => SetParam("background", background);
    }
}
