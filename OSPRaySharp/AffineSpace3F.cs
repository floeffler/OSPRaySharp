using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay
{
    [StructLayout(LayoutKind.Sequential)]
    public struct AffineSpace3F : IEquatable<AffineSpace3F>
    {
        public static readonly AffineSpace3F Identity = new AffineSpace3F(LinearSpace3F.Identity);

        public AffineSpace3F(LinearSpace3F l): this(l, Vector3.Zero)
        {
        }

        public AffineSpace3F(LinearSpace3F l, Vector3 p)
        {
            L = l;
            P = p;
        }

        public AffineSpace3F(Matrix4x4 affineMatrix)
        {
            // matrix is row-major -> affine space column major
            L = new LinearSpace3F(
                affineMatrix.M11, affineMatrix.M21, affineMatrix.M31,
                affineMatrix.M12, affineMatrix.M22, affineMatrix.M32,
                affineMatrix.M13, affineMatrix.M23, affineMatrix.M33);

            P = affineMatrix.Translation;
        }

        public LinearSpace3F L { get; set; }
        public Vector3 P { get; set; }

        public AffineSpace3F? Inverted
        {
            get
            {
                var invL = L.Inverted;
                if (invL.HasValue)
                {
                    return new AffineSpace3F(invL.Value, -(invL.Value * P));
                }
                else
                {
                    return null;
                }
}
        }

        public bool Equals(AffineSpace3F other)
        {
            return L == other.L && P == other.P;
        }

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            return obj is AffineSpace3F other && Equals(other);
        }

        public override int GetHashCode() => HashCode.Combine(L, P);

        public override string ToString() => FormattableString.Invariant($"l: {L}, p: {P}");

        public static bool operator ==(AffineSpace3F left, AffineSpace3F right) => left.L == right.L && left.P == right.P;
        public static bool operator !=(AffineSpace3F left, AffineSpace3F right) => left.L != right.L || left.P != right.P;

        public static AffineSpace3F operator *(AffineSpace3F left, AffineSpace3F right)
        {
            return new AffineSpace3F(left.L * right.L, left.L * right.P + left.P);
        }

        public static Vector3 operator *(AffineSpace3F left, Vector3 right)
        {
            return left.L * right + left.P;
        }
    }
}
