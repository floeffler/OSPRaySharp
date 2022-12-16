using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay
{
    public struct Frame
    {
        public Vector3 Right { get; set; }
        public Vector3 Up { get; set; }
        public Vector3 Front { get; set; }

        public Vector3 Position { get; set; }

        public Frame(Vector3 position) : this(Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, position)
        {
        }

        public Frame(Vector3 right, Vector3 up, Vector3 front) : this(right, up, front, Vector3.Zero)
        {
        }

        public Frame(Vector3 right, Vector3 up, Vector3 front, Vector3 position)
        {
            Right = right;
            Up = up;
            Front = front;
            Position = position;
        }

        public Pose ToPose()
        {
            var m = new Matrix4x4(
                Right.X, Right.Y, Right.Z, 0,
                Up.X, Up.Y, Up.Z, 0,
                -Front.X, -Front.Y, -Front.Z, 0,
                0, 0, 0, 1);

            return new Pose(
                Position,
                Quaternion.CreateFromRotationMatrix(m));
        }

        public Plane ToPlane()
        {
            var N = Vector3.Normalize(Front);
            return new Plane(Position, -Vector3.Dot(N, Position));
        }

        public static readonly Frame Identity = new Frame(Vector3.Zero);
    }
}
