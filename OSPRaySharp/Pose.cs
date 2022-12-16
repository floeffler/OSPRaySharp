using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay
{
    public struct Pose : IEquatable<Pose>
    {
        public static readonly Pose Identity = new Pose(Vector3.Zero, Quaternion.Identity);
        

        public Pose(Vector3 p)
        {
            Position = p;
            Rotation = Quaternion.Identity;
        }

        public Pose(Vector3 p, Quaternion q)
        {
            Position = p;
            Rotation = q;
        }

        public Pose(Vector3 pos, Vector3 lookat, Vector3 up)
        {
            Position = pos;

            var mat = Matrix4x4.CreateWorld(pos, lookat - pos, up);
            Rotation = Quaternion.CreateFromRotationMatrix(mat);
        }

        public Vector3 Position
        {
            get;
            set;
        }


        public Quaternion Rotation
        {
            get;
            set;
        }

        public Matrix4x4 ToMatrix4x4()
        {
            return 
                Matrix4x4.CreateFromQuaternion(Rotation) * 
                Matrix4x4.CreateTranslation(Position);
        }

        public Frame ToFrame()
        {
            var right = Vector3.Normalize(Vector3.Transform(Vector3.UnitX, Rotation));
            var up = Vector3.Normalize(Vector3.Transform(Vector3.UnitY, Rotation));
            var front = Vector3.Normalize(Vector3.Transform(Vector3.UnitZ, Rotation));
            return new Frame(right, up, front, Position);
        }

        public bool Equals(Pose other)
        {
            return Position == other.Position && Rotation == other.Rotation;
        }

        public override bool Equals(object? obj)
        {
            return obj is Pose other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Position, Rotation);
        }

        public override string ToString()
        {
            return FormattableString.Invariant($"{{ position:{Position}, rotation:{Rotation} }}");
        }

        public static Pose Lerp(Pose a, Pose b, float t)
        {
            var A = Quaternion.Normalize(a.Rotation);
            var B = Quaternion.Normalize(b.Rotation);
            var Q = Quaternion.Slerp(A, B, t);

            Q = Quaternion.Normalize(Q);
            return new Pose(Vector3.Lerp(a.Position, b.Position, t), Q);
        }

        public static bool operator ==(Pose lhs, Pose rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Pose lhs, Pose rhs)
        {
            return !lhs.Equals(rhs);
        }
    }
}
