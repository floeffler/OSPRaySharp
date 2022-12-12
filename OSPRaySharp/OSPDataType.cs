using System.Numerics;
using System.Runtime.CompilerServices;

namespace OSPRay
{
    internal enum OSPDataType
    {
        // Object reference type.
        OSP_DEVICE = 100,

        // Void pointer type.
        OSP_VOID_PTR = 200,

        // Booleans, same size as OSP_INT.
        OSP_BOOL = 250,

        // highest bit to represent objects/handles
        OSP_OBJECT = 0x8000000,

        // object subtypes
        OSP_DATA = 0x8000000 + 100,
        OSP_CAMERA,
        OSP_FRAMEBUFFER,
        OSP_FUTURE,
        OSP_GEOMETRIC_MODEL,
        OSP_GEOMETRY,
        OSP_GROUP,
        OSP_IMAGE_OPERATION,
        OSP_INSTANCE,
        OSP_LIGHT,
        OSP_MATERIAL,
        OSP_RENDERER,
        OSP_TEXTURE,
        OSP_TRANSFER_FUNCTION,
        OSP_VOLUME,
        OSP_VOLUMETRIC_MODEL,
        OSP_WORLD,

        // Pointer to a C-style NULL-terminated character string.
        OSP_STRING = 1500,

        // Character scalar type.
        OSP_CHAR = 2000,
        OSP_VEC2C,
        OSP_VEC3C,
        OSP_VEC4C,

        // Unsigned character scalar and vector types.
        OSP_UCHAR = 2500,
        OSP_VEC2UC,
        OSP_VEC3UC,
        OSP_VEC4UC,
        OSP_BYTE = 2500, // XXX OSP_UCHAR, ISPC issue #1246
        OSP_RAW = 2500, // XXX OSP_UCHAR, ISPC issue #1246

        // Signed 16-bit integer scalar.
        OSP_SHORT = 3000,
        OSP_VEC2S,
        OSP_VEC3S,
        OSP_VEC4S,

        // Unsigned 16-bit integer scalar.
        OSP_USHORT = 3500,
        OSP_VEC2US,
        OSP_VEC3US,
        OSP_VEC4US,

        // Signed 32-bit integer scalar and vector types.
        OSP_INT = 4000,
        OSP_VEC2I,
        OSP_VEC3I,
        OSP_VEC4I,

        // Unsigned 32-bit integer scalar and vector types.
        OSP_UINT = 4500,
        OSP_VEC2UI,
        OSP_VEC3UI,
        OSP_VEC4UI,

        // Signed 64-bit integer scalar and vector types.
        OSP_LONG = 5000,
        OSP_VEC2L,
        OSP_VEC3L,
        OSP_VEC4L,

        // Unsigned 64-bit integer scalar and vector types.
        OSP_ULONG = 5550,
        OSP_VEC2UL,
        OSP_VEC3UL,
        OSP_VEC4UL,

        // Half precision floating point scalar and vector types (IEEE 754
        // `binary16`).
        OSP_HALF = 5800,
        OSP_VEC2H,
        OSP_VEC3H,
        OSP_VEC4H,

        // Single precision floating point scalar and vector types.
        OSP_FLOAT = 6000,
        OSP_VEC2F,
        OSP_VEC3F,
        OSP_VEC4F,

        // Double precision floating point scalar type.
        OSP_DOUBLE = 7000,
        OSP_VEC2D,
        OSP_VEC3D,
        OSP_VEC4D,

        // Signed 32-bit integer N-dimensional box types
        OSP_BOX1I = 8000,
        OSP_BOX2I,
        OSP_BOX3I,
        OSP_BOX4I,

        // Single precision floating point N-dimensional box types
        OSP_BOX1F = 10000,
        OSP_BOX2F,
        OSP_BOX3F,
        OSP_BOX4F,

        // Transformation types
        OSP_LINEAR2F = 12000,
        OSP_LINEAR3F,
        OSP_AFFINE2F,
        OSP_AFFINE3F,

        OSP_QUATF,

        // Guard value.
        OSP_UNKNOWN = 9999999
    }

    internal static class OSPDataTypeUtil
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static OSPDataType GetDataType<T>() 
        {
            if (typeof(T) == typeof(bool))
                return OSPDataType.OSP_BOOL;

            if (typeof(T) == typeof(sbyte))
                return OSPDataType.OSP_CHAR;

            if (typeof(T) == typeof(byte))
                return OSPDataType.OSP_UCHAR;

            if (typeof(T) == typeof(short))
                return OSPDataType.OSP_SHORT;

            if (typeof(T) == typeof(ushort))
                return OSPDataType.OSP_USHORT;

            if (typeof(T) == typeof(int))
                return OSPDataType.OSP_INT;

            if (typeof(T) == typeof(uint))
                return OSPDataType.OSP_UINT;

            if (typeof(T) == typeof(float))
                return OSPDataType.OSP_FLOAT;

            if (typeof(T) == typeof(double))
                return OSPDataType.OSP_DOUBLE;

            if (typeof(T) == typeof(Vector2))
                return OSPDataType.OSP_VEC2F;

            if (typeof(T) == typeof(Vector3))
                return OSPDataType.OSP_VEC3F;

            if (typeof(T) == typeof(Vector4))
                return OSPDataType.OSP_VEC4F;


            if (typeof(T).IsEnum)
                return OSPDataType.OSP_INT;


            if (typeof(T) == typeof(OSPData<>))
                return OSPDataType.OSP_DATA;

            if (typeof(T) == typeof(OSPImageOperation) || typeof(T).IsSubclassOf(typeof(OSPImageOperation)))
                return OSPDataType.OSP_IMAGE_OPERATION;

            if (typeof(T) == typeof(OSPCamera) || typeof(T).IsSubclassOf(typeof(OSPCamera)))
                return OSPDataType.OSP_CAMERA;

            if (typeof(T) == typeof(OSPRenderer) || typeof(T).IsSubclassOf(typeof(OSPRenderer)))
                return OSPDataType.OSP_RENDERER;

            if (typeof(T) == typeof(OSPLight) || typeof(T).IsSubclassOf(typeof(OSPLight)))
                return OSPDataType.OSP_LIGHT;

            if (typeof(T) == typeof(OSPGeometry) || typeof(T).IsSubclassOf(typeof(OSPGeometry)))
                return OSPDataType.OSP_GEOMETRY;

            if (typeof(T) == typeof(OSPGeometricModel) || typeof(T).IsSubclassOf(typeof(OSPGeometricModel)))
                return OSPDataType.OSP_GEOMETRIC_MODEL;

            if (typeof(T) == typeof(OSPMaterial) || typeof(T).IsSubclassOf(typeof(OSPMaterial)))
                return OSPDataType.OSP_MATERIAL;


            if (typeof(T).IsSubclassOf(typeof(OSPObject)))
                return OSPDataType.OSP_OBJECT;

            return OSPDataType.OSP_UNKNOWN;
        }

        internal static OSPDataType GetDataTypeOrThrow<T>()
        {
            var dataType = OSPDataTypeUtil.GetDataType<T>();
            if (dataType == OSPDataType.OSP_UNKNOWN)
            {
                throw new ArgumentException("Unsupported parameter type.");
            }
            return dataType;
        }
    }   
}
