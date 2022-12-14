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
            X = vx;
            Y = vy;
            Z = vz;
        }

        public LinearSpace3F(
            float m00, float m01, float m02,
            float m10, float m11, float m12,
            float m20, float m21, float m22)
        {
            X = new Vector3(m00, m10, m20);
            Y = new Vector3(m01, m11, m21);
            Z = new Vector3(m02, m12, m22);
        }

        public Vector3 X { get; set; }
        public Vector3 Y { get; set; }
        public Vector3 Z { get; set; }


        public bool Equals(LinearSpace3F other)
        {
            return X == other.X && Y == other.Y && Z == other.Z;
        }

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            return obj is LinearSpace3F other && Equals(other); 
        }
      
        public override int GetHashCode() => HashCode.Combine(X, Y, Z);

        public override string ToString() => FormattableString.Invariant($"vx: {X}, vy: {Y}, vz: {Z}");

        public static bool operator ==(LinearSpace3F left, LinearSpace3F right) => left.X == right.X && left.Y == right.Y && left.Z == right.Z;
        public static bool operator !=(LinearSpace3F left, LinearSpace3F right) => left.X != right.X || left.Y != right.Y || left.Z != right.Z;


        public static LinearSpace3F operator *(LinearSpace3F left, LinearSpace3F right)
        {
            return new LinearSpace3F(left * right.X, left * right.Y, left * right.Z);
        }

        public static Vector3 operator* (LinearSpace3F left, Vector3 right)
        {
            return left.X * right.X + left.Y * right.Y + left.Z * right.Z;
        }
    }
}
