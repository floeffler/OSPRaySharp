using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay
{
    internal class OSPGeometricModelHandle : OSPObjectHandle
    {
    }

    /// <summary>
    /// Geometries are matched with surface appearance information through GeometricModels. 
    /// These take a geometry, which defines the surface representation, 
    /// and applies either full-object or per-primitive color and material information.
    /// </summary>
    public class OSPGeometricModel : OSPObject
    {
        private OSPGeometricModelHandle handle;

        public OSPGeometricModel(OSPGeometry? geometry)
        {
            OSPGeometryHandle geometryHandle = OSPGeometryHandle.Empty;
            if (geometry != null)
            {
                geometryHandle = (OSPGeometryHandle)geometry.Handle;
            }
            handle = NativeMethods.ospNewGeometricModel(geometryHandle);
            if (handle.IsInvalid)
                OSPDevice.ThrowLastError();
        }

        public void SetGeometry(OSPGeometry geometry) => SetObjectParam("geometry", geometry);
        public void SetId(uint id) => SetParam("id", id);
        public void SetInvertNormals(bool invertNormals) => SetParam("invertNormals", invertNormals);
        public void SetColor(Vector4[] color) => SetArrayParam("color", color);
        public void SetIndices(int[] indices) => SetArrayParam("index", indices);

        public void SetMaterials(int[] materialIndices) => SetArrayParam("material", materialIndices);
        public void SetMaterials(params OSPMaterial[] materials) {
            if (materials.Length == 1)
            {
                SetObjectParam("material", materials[0]);
            }
            else
            {
                SetObjectArrayParam("material", materials);
            }
        } 


        internal override OSPObjectHandle Handle => handle;
    }
}
