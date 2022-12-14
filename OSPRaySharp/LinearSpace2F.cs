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
    public struct LinearSpace2F : IEquatable<LinearSpace2F>
    {
        public static readonly LinearSpace2F Identity = new LinearSpace2F(Vector2.UnitX, Vector2.UnitY);

        public LinearSpace2F(Vector2 vx, Vector2 vy)
        {
            VX = vx;
            VY = vy;
        }

        public LinearSpace2F(
            float m00, float m01,
            float m10, float m11)
        {
            VX = new Vector2(m00, m10);
            VY = new Vector2(m01, m11);
        }

        public Vector2 VX { get; set; }
        public Vector2 VY { get; set; }
     
        public float Determinant => VX.X * VY.Y - VX.Y * VY.X;

        public LinearSpace2F Adjoint
        {
            get
            {
                return new LinearSpace2F(VY.Y, -VY.X, -VX.Y, VX.X);
            }
        }

        public LinearSpace2F Transposed
        {
            get
            {
                return new LinearSpace2F(VX.X, VX.Y, VY.X, VY.Y);
            }
        }

        public LinearSpace2F? Inverted
        {
            get
            {
                float det = Determinant;
                if (det != 0)
                    return Adjoint / det;
                else
                    return null;
            }
        }

        public bool Equals(LinearSpace2F other)
        {
            return VX == other.VX && VY == other.VY;
        }

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            return obj is LinearSpace2F other && Equals(other);
        }

        public override int GetHashCode() => HashCode.Combine(VX, VY);

        public override string ToString() => FormattableString.Invariant($"vx: {VX}, vy: {VY}");

        public static bool operator ==(LinearSpace2F left, LinearSpace2F right) => left.VX == right.VX && left.VY == right.VY;
        public static bool operator !=(LinearSpace2F left, LinearSpace2F right) => left.VX != right.VX || left.VY != right.VY;


        public static LinearSpace2F operator *(LinearSpace2F left, LinearSpace2F right)
        {
            return new LinearSpace2F(left * right.VX, left * right.VY);
        }

        public static LinearSpace2F operator* (float left, LinearSpace2F right)
        {
            return new LinearSpace2F(
                left * right.VX,
                left * right.VY);
        }

        public static LinearSpace2F operator /(LinearSpace2F left, float right)
        {
            float rcpRight = 1f / right;
            return rcpRight * left;
        }

        public static Vector2 operator *(LinearSpace2F left, Vector2 right)
        {
            return left.VX * right.X + left.VY * right.Y;
        }

    }
}
