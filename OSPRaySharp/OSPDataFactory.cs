using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay
{
    public static class OSPDataFactory
    {
        public unsafe static OSPData<IntPtr> CreateObjectArray<T>(T[] objects) where T : OSPObject
        {
            // get type and handles
            var dataType = OSPDataTypeUtil.GetDataTypeOrThrow<T>();
            var handles = objects.Select(x => x.Handle.DangerousGetHandle()).ToArray();

            // create opaque data object and copy data from shared data object
            var dataObject = new OSPData<IntPtr>(dataType, handles.Length);
            fixed (void* dataPointer = handles)
            {
                using (var sharedDataHandle = NativeMethods.ospNewSharedData(dataPointer, dataType, handles.Length, 0, 1, 0, 1, 0))
                {
                    NativeMethods.ospCopyData(sharedDataHandle, (OSPDataHandle)dataObject.Handle, 0, 0, 0);
                }
            }

            return dataObject;
        }


        public static OSPData<T> CreateData1D<T>(T[] data) where T : unmanaged => CreateData3D<T>(new ReadOnlySpan<T>(data), data.Length, 1, 1);
        public static OSPData<T> CreateData1D<T>(ReadOnlySpan<T> data) where T : unmanaged => CreateData3D<T>(data, data.Length, 1, 1);

        public static OSPData<T> CreateData2D<T>(T[] data, int width, int height) where T : unmanaged => CreateData3D<T>(new ReadOnlySpan<T>(data), width, height, 1);
        public static OSPData<T> CreateData2D<T>(ReadOnlySpan<T> data, int width, int height) where T : unmanaged => CreateData3D<T>(data, width, height, 1);

        public static OSPData<T> CreateData3D<T>(T[] data, int width, int height, int depth) where T : unmanaged => CreateData3D<T>(new ReadOnlySpan<T>(data), width, height, depth);
        public static OSPData<T> CreateData3D<T>(ReadOnlySpan<T> data, int width, int height, int depth) where T : unmanaged
        {
            long totalSize = width * height * depth;
            if (data.Length < totalSize)
                throw new ArgumentException("The total number of elements exceeds the provided data span.");

            var dataType = OSPDataTypeUtil.GetDataTypeOrThrow<T>();
            var dataObject = new OSPData<T>(width, height, depth);
            unsafe
            {
                fixed (void* dataPointer = data)
                {
                    using (var sharedDataHandle = NativeMethods.ospNewSharedData(dataPointer, dataType, width, 0, height, 0, depth, 0))
                    {
                        NativeMethods.ospCopyData(sharedDataHandle, (OSPDataHandle)dataObject.Handle, 0, 0, 0);
                    }
                }
            }
            OSPDevice.CheckLastDeviceError();
            return dataObject;
        }


        public static OSPData<T> CreateSharedData1D<T>(T[] data) where T : unmanaged => CreateSharedData3D<T>(data, data.Length, 1, 1);
        public static OSPData<T> CreateSharedData2D<T>(T[] data, int width, int height) where T : unmanaged => CreateSharedData3D<T>(data, width, height, 1);
        public static OSPData<T> CreateSharedData3D<T>(T[] data, int width, int height, int depth) where T : unmanaged => new OSPData<T>(new ReadOnlyMemory<T>(data), width, height, depth);
    }
}
