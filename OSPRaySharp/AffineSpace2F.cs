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
    public struct AffineSpace2F : IEquatable<AffineSpace2F>
    {
        public static readonly AffineSpace2F Identity = new AffineSpace2F(LinearSpace2F.Identity);

        public AffineSpace2F(LinearSpace2F l): this(l, Vector2.Zero)
        {
        }

        public AffineSpace2F(LinearSpace2F l, Vector2 p)
        {
            L = l;
            P = p;
        }

        public AffineSpace2F(Matrix3x2 affineMatrix)
        {
            // matrix is row-major -> affine space column major
            L = new LinearSpace2F(
                affineMatrix.M11, affineMatrix.M21,
                affineMatrix.M12, affineMatrix.M22);

            P = affineMatrix.Translation;
        }

        public LinearSpace2F L { get; set; }
        public Vector2 P { get; set; }

        public AffineSpace2F? Inverted
        {
            get
            {
                var invL = L.Inverted;
                if (invL.HasValue)
                {
                    return new AffineSpace2F(invL.Value, -(invL.Value * P));
                }
                else
                {
                    return null;
                }
}
        }

        public bool Equals(AffineSpace2F other)
        {
            return L == other.L && P == other.P;
        }

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            return obj is AffineSpace2F other && Equals(other);
        }

        public override int GetHashCode() => HashCode.Combine(L, P);

        public override string ToString() => FormattableString.Invariant($"l: {L}, p: {P}");

        public static bool operator ==(AffineSpace2F left, AffineSpace2F right) => left.L == right.L && left.P == right.P;
        public static bool operator !=(AffineSpace2F left, AffineSpace2F right) => left.L != right.L || left.P != right.P;

        public static AffineSpace2F operator *(AffineSpace2F left, AffineSpace2F right)
        {
            return new AffineSpace2F(left.L * right.L, left.L * right.P + left.P);
        }

        public static Vector2 operator *(AffineSpace2F left, Vector2 right)
        {
            return left.L * right + left.P;
        }
    }
}
