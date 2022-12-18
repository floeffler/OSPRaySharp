using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay
{
    public struct OSPBounds : IEquatable<OSPBounds>
    {
        public OSPBounds(Vector3 lower, Vector3 upper)
        {
            Lower = lower;
            Upper = upper;
        }

        public Vector3 Lower { get; set; }
        public Vector3 Upper { get; set; }
        public Vector3 Center => (Lower + Upper) * 0.5f;

        public bool Equals(OSPBounds other)
        {
            return Lower == other.Lower && Upper == other.Upper;
        }

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            return obj is AffineSpace3F other && Equals(other);
        }

        public override int GetHashCode() => HashCode.Combine(Lower, Upper);

        public override string ToString() => FormattableString.Invariant($"lower: {Lower}, upper: {Upper}");

        public static bool operator ==(OSPBounds left, OSPBounds right) => left.Lower == right.Lower && left.Upper == right.Upper;
        public static bool operator !=(OSPBounds left, OSPBounds right) => left.Lower != right.Lower || left.Upper != right.Upper;

    }
}
