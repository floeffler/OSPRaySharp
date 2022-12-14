using System.Numerics;
using System.Runtime.CompilerServices;

namespace OSPRay
{
    public enum OSPDataType
    {
        // Object reference type.
        Device = 100,

        // Void pointer type.
        Pointer = 200,

        // Booleans, same size as OSP_INT.
        Bool = 250,

        // highest bit to represent objects/handles
        Object = 0x8000000,

        // object subtypes
        Data = 0x8000000 + 100,
        Camera,
        FrameBuffer,
        Future,
        GeometricModel,
        Geometry,
        Group,
        ImageOperation,
        Instance,
        Light,
        Material,
        Renderer,
        Texture,
        TransferFunction,
        Volume,
        VolumetricModel,
        World,

        // Pointer to a C-style NULL-terminated character string.
        String = 1500,

        // Character scalar type.
        Char = 2000,
        Vec2C,
        Vec3C,
        Vec4C,

        // Unsigned character scalar and vector types.
        UChar = 2500,
        Vec2UC,
        Vec3UC,
        Vec4UC,
        Byte = 2500, // XXX OSP_UCHAR, ISPC issue #1246
        Raw = 2500, // XXX OSP_UCHAR, ISPC issue #1246

        // Signed 16-bit integer scalar.
        Short = 3000,
        Vec2S,
        Vec3S,
        Vec4S,

        // Unsigned 16-bit integer scalar.
        UShort = 3500,
        Vec3US,
        Vec2US,
        Vec4US,

        // Signed 32-bit integer scalar and vector types.
        Int = 4000,
        Vec2I,
        Vec3I,
        Vec4I,

        // Unsigned 32-bit integer scalar and vector types.
        UInt = 4500,
        Vec2UI,
        Vec3UI,
        Vec4UI,

        // Signed 64-bit integer scalar and vector types.
        Long = 5000,
        Vec2L,
        Vec3L,
        Vec4L,

        // Unsigned 64-bit integer scalar and vector types.
        ULong = 5550,
        Vec2UL,
        Vec3UL,
        Vec4UL,

        // Half precision floating point scalar and vector types (IEEE 754
        // `binary16`).
        Half = 5800,
        Vec2H,
        Vec3H,
        Vec4H,

        // Single precision floating point scalar and vector types.
        Float = 6000,
        Vec2F,
        Vec3F,
        Vec4F,

        // Double precision floating point scalar type.
        Double = 7000,
        Vec2D,
        Vec3D,
        Vec4D,

        // Signed 32-bit integer N-dimensional box types
        Box1I = 8000,
        Box2I,
        Box3I,
        Box4I,

        // Single precision floating point N-dimensional box types
        Box1F = 10000,
        Box2F,
        Box3F,
        Box4F,

        // Transformation types
        Linear2F = 12000,
        Linear3F,
        Affine2F,
        Affine3F,

        QuatF,
        Unknown = 9999999
    }

    internal static class OSPDataTypeUtil
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static OSPDataType GetDataType<T>() 
        {
            if (typeof(T) == typeof(bool))
                return OSPDataType.Bool;

            if (typeof(T) == typeof(sbyte))
                return OSPDataType.Char;

            if (typeof(T) == typeof(byte))
                return OSPDataType.UChar;

            if (typeof(T) == typeof(short))
                return OSPDataType.Short;

            if (typeof(T) == typeof(ushort))
                return OSPDataType.UShort;

            if (typeof(T) == typeof(int))
                return OSPDataType.Int;

            if (typeof(T) == typeof(uint))
                return OSPDataType.UInt;

            if (typeof(T) == typeof(float))
                return OSPDataType.Float;

            if (typeof(T) == typeof(double))
                return OSPDataType.Double;
            
            if (typeof(T).IsEnum)
                return OSPDataType.Int;


            if (typeof(T) == typeof(Vector2))
                return OSPDataType.Vec2F;

            if (typeof(T) == typeof(Vector3))
                return OSPDataType.Vec3F;

            if (typeof(T) == typeof(Vector4))
                return OSPDataType.Vec4F;

            if (typeof(T) == typeof(Quaternion))
                return OSPDataType.QuatF;

            if (typeof(T) == typeof(LinearSpace3F))
                return OSPDataType.Linear3F;

            if (typeof(T) == typeof(AffineSpace3F))
                return OSPDataType.Affine3F;

            

            if (typeof(T) == typeof(OSPData<>))
                return OSPDataType.Data;

            if (typeof(T) == typeof(OSPImageOperation) || typeof(T).IsSubclassOf(typeof(OSPImageOperation)))
                return OSPDataType.ImageOperation;

            if (typeof(T) == typeof(OSPCamera) || typeof(T).IsSubclassOf(typeof(OSPCamera)))
                return OSPDataType.Camera;

            if (typeof(T) == typeof(OSPRenderer) || typeof(T).IsSubclassOf(typeof(OSPRenderer)))
                return OSPDataType.Renderer;

            if (typeof(T) == typeof(OSPLight) || typeof(T).IsSubclassOf(typeof(OSPLight)))
                return OSPDataType.Light;

            if (typeof(T) == typeof(OSPGeometry) || typeof(T).IsSubclassOf(typeof(OSPGeometry)))
                return OSPDataType.Geometry;

            if (typeof(T) == typeof(OSPGeometricModel) || typeof(T).IsSubclassOf(typeof(OSPGeometricModel)))
                return OSPDataType.GeometricModel;

            if (typeof(T) == typeof(OSPMaterial) || typeof(T).IsSubclassOf(typeof(OSPMaterial)))
                return OSPDataType.Material;

            if (typeof(T) == typeof(OSPInstance) || typeof(T).IsSubclassOf(typeof(OSPInstance)))
                return OSPDataType.Instance;

            if (typeof(T) == typeof(OSPGroup) || typeof(T).IsSubclassOf(typeof(OSPGroup)))
                return OSPDataType.Group;

            if (typeof(T) == typeof(OSPWorld) || typeof(T).IsSubclassOf(typeof(OSPWorld)))
                return OSPDataType.World;


            if (typeof(T).IsSubclassOf(typeof(OSPObject)))
                return OSPDataType.Object;

            return OSPDataType.Unknown;
        }

        internal static OSPDataType GetDataTypeOrThrow<T>()
        {
            var dataType = OSPDataTypeUtil.GetDataType<T>();
            if (dataType == OSPDataType.Unknown)
            {
                throw new ArgumentException("Unsupported parameter type. Please use specify the data type explicitly.");
            }
            return dataType;
        }
    }   
}
