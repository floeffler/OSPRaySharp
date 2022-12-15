using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay
{
    internal class OSPRendererHandle : OSPObjectHandle
    {
    }

    public abstract class OSPRenderer : OSPObject
    {
        private OSPRendererHandle handle;

        public OSPRenderer(string type): base()
        {
            handle = NativeMethods.ospNewRenderer(type);
            if (handle.IsInvalid)
            {
                OSPDevice.ThrowLastError();
            }
        }

        public void SetSamplesPerPixel(int pixelSamples) => SetParam("pixelSamples", pixelSamples);
        public void SetMaxPathLength(int maxPathLength) => SetParam("maxPathLength", maxPathLength);
        public void SetMinContribution(float minContribution) => SetParam("minContribution", minContribution);
        public void SetVarianceThreshold(float varianceThreshold) => SetParam("varianceThreshold", varianceThreshold);

        public void SetBackgroundColor(float alpha) => SetParam("backgroundColor", alpha);
        public void SetBackgroundColor(Vector3 rgbColor) => SetParam("backgroundColor", rgbColor);
        public void SetBackgroundColor(Vector4 rgbaColor) => SetParam("backgroundColor", rgbaColor);

        public void SetBackplate(OSPTexture backplate) => SetObjectParam("map_backplate", backplate);
        public void SetMaxDepth(OSPTexture maxDepth) => SetObjectParam("map_maxDepth", maxDepth);
        public void SetMaterial(OSPMaterial[] material) => SetObjectArrayParam("material", material);
        public void SetPixelFilter(OSPPixelFilter filter) => SetParam("pixelFilter", filter);

        public OSPFuture RenderFrame(OSPFrameBuffer frameBuffer, OSPCamera camera, OSPWorld world)
        {
            var future = NativeMethods.ospRenderFrame(
                (OSPFrameBufferHandle)frameBuffer.Handle,
                handle,
                (OSPCameraHandle)camera.Handle,
                (OSPWorldHandle)world.Handle);

            if (future.IsInvalid)
                OSPDevice.ThrowLastError();

            return new OSPFuture(future);
        }

        public float ospRenderFrameBlocking(OSPFrameBuffer frameBuffer, OSPCamera camera, OSPWorld world)
        {
            float variance = NativeMethods.ospRenderFrameBlocking(
                (OSPFrameBufferHandle)frameBuffer.Handle,
                handle,
                (OSPCameraHandle)camera.Handle,
                (OSPWorldHandle)world.Handle);

            OSPDevice.CheckLastDeviceError();
            return variance;
        }

        internal override OSPObjectHandle Handle => handle;
    }
}
