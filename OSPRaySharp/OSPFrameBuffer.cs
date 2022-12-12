using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay
{
    internal class OSPFrameBufferHandle : OSPObjectHandle
    {
    }

    public enum OSPFrameBufferFormat
    {
        None,
        RGBA8,
        SRGBA,
        RGBA32F,
    }

    [Flags]
    public enum OSPFrameBufferChannel
    {
        Color = (1 << 0),
        Depth = (1 << 1),
        Accum = (1 << 2),
        Variance = (1 << 3),
        Normal = (1 << 4),
        Albedo = (1 << 5),

        Default = Color | Depth | Accum,
        All = Color | Depth | Accum | Variance | Normal | Albedo,
    }
    
    public class OSPFrameBufferMappedData : IDisposable
    {
        private unsafe void* pointer;
        private OSPFrameBufferHandle frameBuffer;
        
        internal unsafe OSPFrameBufferMappedData(void* pointer, int sizeInBytes, OSPFrameBufferHandle frameBuffer)
        {
            this.pointer = pointer;
            this.frameBuffer = frameBuffer;
            SizeInBytes = sizeInBytes;
        }

        public int SizeInBytes { get; }

        public unsafe ReadOnlySpan<byte> Span => GetSpan<byte>();

        public unsafe ReadOnlySpan<T> GetSpan<T>() where T : unmanaged 
        {
            int length = SizeInBytes / sizeof(T);
            return new Span<T>(pointer, length);
        }

        public void CopyTo<T>(Span<T> dest) where T : unmanaged
        {
            var span = GetSpan<T>();
            span.CopyTo(dest);
        }

        public unsafe void Dispose()
        {
            if (pointer != null)
                NativeMethods.ospUnmapFrameBuffer(pointer, frameBuffer);

            pointer = null;
        }
    }

    public class OSPFrameBuffer : OSPObject
    {
        private OSPFrameBufferHandle handle;

        public OSPFrameBuffer(int width, int height, OSPFrameBufferFormat format = OSPFrameBufferFormat.SRGBA, OSPFrameBufferChannel channels = OSPFrameBufferChannel.Default)
        {
            Width = width;
            Height = height;
            Format = format;
            Channels = channels; 

            handle = NativeMethods.ospNewFrameBuffer(width, height, format, channels);
            if (handle.IsInvalid)
                OSPDevice.ThrowLastError();
        }

        public int Width { get; }
        public int Height { get; }
        public OSPFrameBufferFormat Format { get; }
        public OSPFrameBufferChannel Channels { get; }

        /// <summary>
        /// Maps the memory of a framebuffer channel to the application. 
        /// The mapped memory must be unmap by disposing the mapped data.
        /// </summary>
        /// <param name="channel">the channel to map</param>
        /// <returns>The mapped data</returns>
        public unsafe OSPFrameBufferMappedData Map(OSPFrameBufferChannel channel = OSPFrameBufferChannel.Color)
        {
            if (Format == OSPFrameBufferFormat.None)
                throw new InvalidOperationException("No framebuffer format specified.");

            if (Channels.HasFlag(channel) == false)
                throw new ArgumentException("Invalid framebuffer channel.");


            int stride = Format == OSPFrameBufferFormat.RGBA32F ? sizeof(float) * 4 : 4;
            int length = Width * Height;
            int sizeInBytes = stride * length;

            void* pointer = NativeMethods.ospMapFrameBuffer(handle, channel);
            if (pointer == null)
                throw new InvalidOperationException("Framebuffer channel cannot be mapped to application.");

            return new OSPFrameBufferMappedData(pointer, sizeInBytes, handle);
        }

        public void SetImageOperations(OSPImageOperation[]? imageOperations)
        {
            SetObjectArrayParam("imageOperation", imageOperations);
        }

        /// <summary>
        /// Gets the variance of the final image. The value can be used by the application 
        /// as a quality indicator and thus to decide whether to stop or to continue progressive rendering.
        /// </summary>
        /// <returns></returns>
        public float GetVariance()
        {
            float variance = NativeMethods.ospGetVariance(handle);
            OSPDevice.CheckLastDeviceError();
            return variance;
        }

        /// <summary>
        /// Clears the individual channels of a framebuffer
        /// </summary>
        public void ResetAccumulation()
        {
            NativeMethods.ospResetAccumulation(handle);
            OSPDevice.CheckLastDeviceError();
        }

        internal override OSPObjectHandle Handle => handle;
    }
}
