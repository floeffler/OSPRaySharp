using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay.Geometries
{
    /// <summary>
    /// A geometry consisting of multiple curves
    /// </summary>
    public class OSPCurveGeometry : OSPGeometry
    {
        public OSPCurveGeometry() : base("curve")
        {
        }

        public void SetPositionRadius(Vector4[] positionRadius) => SetArrayParam("vertex.position_radius", positionRadius);
        public void SetTexcoord(Vector2[] texcoord) => SetArrayParam("vertex.texcoord", texcoord);
        public void SetColor(Vector4[] color) => SetArrayParam("vertex.color", color);
        public void SetNormal(Vector3[] normal) => SetArrayParam("vertex.normal", normal);
        public void SetTangent(Vector3[] tangent) => SetArrayParam("vertex.tangent", tangent);
        public void SetIndex(int[] index)
        {
            using (var data = OSPDataFactory.CreateArray(index, OSPDataType.UInt, index.Length))
            {
                SetObjectParam("index", data);
            }
        }
        public void SetType(OSPCurveType type) => SetParam("type", type);
        public void SetBasis(OSPCurveBasis basis) => SetParam("basis", basis);

    }
}
