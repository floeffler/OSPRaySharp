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
    public struct LinearSpace3F : IEquatable<LinearSpace3F>
    {
        public static readonly LinearSpace3F Identity = new LinearSpace3F(Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ);

        public LinearSpace3F(Vector3 vx, Vector3 vy, Vector3 vz)
        {
            VX = vx;
            VY = vy;
            VZ = vz;
        }

        public LinearSpace3F(
            float m00, float m01, float m02,
            float m10, float m11, float m12,
            float m20, float m21, float m22)
        {
            VX = new Vector3(m00, m10, m20);
            VY = new Vector3(m01, m11, m21);
            VZ = new Vector3(m02, m12, m22);
        }

        public Vector3 VX { get; set; }
        public Vector3 VY { get; set; }
        public Vector3 VZ { get; set; }

        public float Determinant => Vector3.Dot(VX, Vector3.Cross(VY, VZ));

        public LinearSpace3F Adjoint
        {
            get
            {
                return new LinearSpace3F(
                   Vector3.Cross(VY, VZ),
                   Vector3.Cross(VZ, VX),
                   Vector3.Cross(VX, VY)).Transposed;
            }
        }

        public LinearSpace3F Transposed
        {
            get
            {
                return new LinearSpace3F(
                            VX.X, VX.Y, VX.Z,
                            VY.X, VY.Y, VY.Z,
                            VZ.X, VZ.Y, VZ.Z);
            }
        }

        public LinearSpace3F? Inverted
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

        public bool Equals(LinearSpace3F other)
        {
            return VX == other.VX && VY == other.VY && VZ == other.VZ;
        }

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            return obj is LinearSpace3F other && Equals(other);
        }

        public override int GetHashCode() => HashCode.Combine(VX, VY, VZ);

        public override string ToString() => FormattableString.Invariant($"vx: {VX}, vy: {VY}, vz: {VZ}");

        public static bool operator ==(LinearSpace3F left, LinearSpace3F right) => left.VX == right.VX && left.VY == right.VY && left.VZ == right.VZ;
        public static bool operator !=(LinearSpace3F left, LinearSpace3F right) => left.VX != right.VX || left.VY != right.VY || left.VZ != right.VZ;


        public static LinearSpace3F operator *(LinearSpace3F left, LinearSpace3F right)
        {
            return new LinearSpace3F(left * right.VX, left * right.VY, left * right.VZ);
        }

        public static LinearSpace3F operator* (float left, LinearSpace3F right)
        {
            return new LinearSpace3F(
                left * right.VX,
                left * right.VY,
                left * right.VZ);
        }

        public static LinearSpace3F operator /(LinearSpace3F left, float right)
        {
            float rcpRight = 1f / right;
            return rcpRight * left;
        }

        public static Vector3 operator *(LinearSpace3F left, Vector3 right)
        {
            return left.VX * right.X + left.VY * right.Y + left.VZ * right.Z;
        }

    }
}
