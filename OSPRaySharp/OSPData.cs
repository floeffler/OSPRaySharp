using System;
using System.Buffers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay
{
    internal class OSPDataHandle : OSPObjectHandle
    {
    }

    public class OSPData<T> : OSPObject where T : unmanaged
    {
        private OSPDataHandle handle;
        private ReadOnlyMemory<T>? sharedMemory;
        private MemoryHandle? sharedMemoryHandle;

        internal OSPData(OSPDataType type, int length)
        {
            NumItems1 = length;
            NumItems2 = 1;
            NumItems3 = 1;

            handle = NativeMethods.ospNewData(type, length, 1, 1);
            if (handle.IsInvalid)
            {
                OSPDevice.ThrowLastError();
            }
        }

        /// <summary>
        /// Creates a new shared data object with memory allocated by the application. 
        /// </summary>
        /// <remarks>
        /// The provided memory object is pinned to avoid moving the memory region by the garbage collection. The pinning is released
        /// if the data object is disposed. Users have to take care of not using shared data objects in OSPRay which memory has not been pinned.
        /// </remarks>
        /// <param name="memory">the memory allocated by the application</param>
        /// <param name="numItems1">the number of items in the first dimension</param>
        /// <param name="numItems2">the number of items in the second dimension</param>
        /// <param name="numItems3">the number of items in the third dimension</param>
        public OSPData(ReadOnlyMemory<T> memory, int numItems1, int numItems2, int numItems3) : base()
        {
            long totalItems = (long)numItems1 * numItems2 * numItems3;
            if (totalItems > memory.Length)
                throw new ArgumentException("The total item count exceeds provided memory.");

            sharedMemory = memory;
            sharedMemoryHandle = memory.Pin();

            NumItems1 = numItems1;
            NumItems2 = numItems2;
            NumItems3 = numItems3;

            var dataType = OSPDataTypeUtil.GetDataTypeOrThrow<T>();
            unsafe
            {
                handle = NativeMethods.ospNewSharedData(sharedMemoryHandle.Value.Pointer, dataType, numItems1, 0, numItems2, 0, numItems3, 0);
                if (handle.IsInvalid)
                {
                    OSPDevice.ThrowLastError();
                }
            }
        }

        /// <summary>
        /// Creates a new data object (3D array) with memory allocated by OSPRay.
        /// </summary>
        /// <param name="numItems1">the number of items in the first dimension</param>
        /// <param name="numItems2">the number of items in the second dimension</param>
        /// <param name="numItems3">the number of items in the third dimension</param>
        public OSPData(int numItems1, int numItems2, int numItems3) : base()
        {
            NumItems1 = numItems1;
            NumItems2 = numItems2;
            NumItems3 = numItems3;

            var dataType = OSPDataTypeUtil.GetDataTypeOrThrow<T>();
            handle = NativeMethods.ospNewData(dataType, numItems1, numItems2, numItems3);
            if (handle.IsInvalid)
            {
                OSPDevice.ThrowLastError();
            }
        }

        /// <summary>
        /// Gets if the underlaying memory is shared between ospray and the application
        /// </summary>
        public bool IsShared => sharedMemory.HasValue;

        /// <summary>
        /// Gets the shared memory object
        /// </summary>
        public ReadOnlyMemory<T>? SharedMemory => sharedMemory;

        public int NumItems1 { get; }
        public int NumItems2 { get; }
        public int NumItems3 { get; }


        internal override OSPObjectHandle Handle => handle;

        public void CopyTo(OSPData<T> destination) => CopyTo(destination, 0, 0, 0);

        public void CopyTo(OSPData<T> destination, long destinationIndex) => CopyTo(destination, destinationIndex, 0, 0);

        public void CopyTo(OSPData<T> destination, long destinationIndex1, long destinationIndex2) => CopyTo(destination, destinationIndex1, destinationIndex2, 0);

        public void CopyTo(OSPData<T> destination, long destinationIndex1, long destinationIndex2, long destinationIndex3)
        {
            NativeMethods.ospCopyData(handle, destination.handle, destinationIndex1, destinationIndex2, destinationIndex3);
            OSPDevice.CheckLastDeviceError();
        }

        public override void Dispose()
        {
            sharedMemoryHandle?.Dispose();
            sharedMemory = null;
            sharedMemory = null;
            base.Dispose();
        }
    }
}
