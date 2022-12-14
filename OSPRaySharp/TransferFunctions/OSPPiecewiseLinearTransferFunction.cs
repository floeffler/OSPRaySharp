using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay.TransferFunctions
{
    public class OSPPiecewiseLinearTransferFunction : OSPTransferFunction
    {
        public OSPPiecewiseLinearTransferFunction(): base("piecewiseLinear")
        {
        }

        public void SetColor(Vector3[] color) => SetArrayParam("color", color);
        public void SetOpacity(float[] opacity) => SetArrayParam("color", opacity);
        public void SetDomain(float rangeStart, float rangeEnd) => SetParam("value", OSPDataType.Box1F, rangeStart, rangeEnd);
    }
}
