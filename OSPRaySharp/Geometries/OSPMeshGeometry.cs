using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay.Geometries
{
    public enum OSPMeshType
    {
        Triangle,
        Quad,
    }

    /// <summary>
    /// A mesh consisting of either triangles or quads.
    /// </summary>
    public class OSPMeshGeometry : OSPGeometry
    {
        public OSPMeshGeometry(): base("mesh")
        {
        }

        public void SetVertexPositions(Vector3[] positions) => SetArrayParam("vertex.position", positions);
        public void SetVertexNormals(Vector3[] normals) => SetArrayParam("vertex.normal", normals);
        public void SetVertexColors(Vector4[] colors) => SetArrayParam("vertex.color", colors);
        public void SetVertexColors(Vector3[] colors) => SetArrayParam("vertex.color", colors);
        public void SetVertexTexCoords(Vector2[] texCoords) => SetArrayParam("vertex.texcoord", texCoords);

        public void SetFaceNormals(Vector3[] normals) => SetArrayParam("normal", normals);
        public void SetFaceColors(Vector4[] colors) => SetArrayParam("color", colors);
        public void SetFaceColors(Vector3[] colors) => SetArrayParam("color", colors);

        public void SetIndices(int[] indices, bool quadFaces = false)
        {
            int indicesPerPrimitive = quadFaces ? 4 : 3;
            OSPDataType dataType = quadFaces ? OSPDataType.Vec4UI : OSPDataType.Vec3UI;
            using (var data = OSPDataFactory.CreateArray(indices, dataType, indices.Length / indicesPerPrimitive))
            {
                SetObjectParam("index", data);
            }
        }

        public void SetTime(float startTime, float endTime) => SetParam("time", OSPDataType.Box1F, startTime, endTime);

        // TODO: add properties
    }
}
