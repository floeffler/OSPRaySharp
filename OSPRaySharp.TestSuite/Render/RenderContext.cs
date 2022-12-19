using OSPRay;
using OSPRay.Cameras;
using OSPRay.ImageOperations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay.TestSuite.Render
{
    internal class RenderContext : IDisposable
    {
        private OSPRenderer? renderer = null;
        private OSPCamera? camera = null;
        private OSPFrameBuffer? frameBuffer = null;
        private OSPWorld? world = null;
        private RenderModel? model = null;

        private OSPToneMapper? toneMapper = null;
        private OSPDenoiser? denoiser = null;


        public RenderContext()
        {
        }

        /// <summary>
        /// The current framebuffer aspect ratio
        /// </summary>
        public float AspectRatio
        {
            get
            {
                if (frameBuffer != null)
                    return frameBuffer.Width / (float)frameBuffer.Height;
                else
                    return 1f;
            }
        }

        /// <summary>
        /// The current renderer. Can be changed via the render settings
        /// </summary>
        public OSPRenderer? Renderer
        {
            get => renderer;
            set
            {
                if (renderer != value)
                {
                    renderer?.Dispose();
                    renderer = value;
                }
            }
        }

        /// <summary>
        /// Gets the current camera. Can be changed via the render settings
        /// </summary>
        public OSPCamera? Camera
        {
            get => camera;
            set
            {
                if (camera != value)
                {
                    camera?.Dispose();
                    camera = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the world to render
        /// </summary>
        public OSPWorld? World
        {
            get => world;
            set
            {
                if (world != value)
                {
                    world?.Dispose();
                    world = value;
                }
            }
        }

        /// <summary>
        /// The actual frame buffer
        /// </summary>
        public OSPFrameBuffer? FrameBuffer => frameBuffer;

        /// <summary>
        /// The model to render
        /// </summary>
        public RenderModel? Model
        {
            get => model;
            set
            {
                if (model != value)
                {
                    model?.Free(this);
                    model = value;
                    model?.Setup(this);
                }
            }
        }

        /// <summary>
        /// The current frame index
        /// </summary>
        public int FrameIndex
        {
            get;
            private set;
        }

        /// <summary>
        /// Resize the current framebuffer
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void Resize(int width, int height)
        {
            if (frameBuffer == null || frameBuffer.Width != width || frameBuffer.Height != height)
            {
                frameBuffer?.Dispose();
                if (width > 0 && height > 0)
                {
                    frameBuffer = new OSPFrameBuffer(width, height, OSPFrameBufferFormat.RGBA32F);
                    SetImageOperations(frameBuffer);
                    frameBuffer.Commit();


                    if (camera is OSPPerspectiveCamera perspectiveCamera)
                    {
                        float aspect = width / (float)height;
                        perspectiveCamera.SetAspect(aspect);
                        perspectiveCamera.Commit();
                    }
                }
                else
                {
                    frameBuffer = null;
                }
            }
        }
        /// <summary>
        /// Gets whether all required objects for rendering are available
        /// </summary>
        public bool CanRenderFrame => world != null && frameBuffer != null && renderer != null && camera != null;

        public byte[] ResolveFrameBuffer(OSPFrameBuffer frameBuffer)
        {
            using (var mappedData = frameBuffer.Map())
            {
                if (frameBuffer.Format != OSPFrameBufferFormat.RGBA32F)
                    return mappedData.Span.ToArray();

                var npixels = frameBuffer.Width * frameBuffer.Height;
                var bytes = new byte[npixels * 4];
                Parallel.For(0, npixels, i =>
                {
                    var pixels = mappedData.GetSpan<Vector4>();

                    int j = i * 4;
                    bytes[j++] = PixelHelper.ToSRGB(pixels[i].X);
                    bytes[j++] = PixelHelper.ToSRGB(pixels[i].Y);
                    bytes[j++] = PixelHelper.ToSRGB(pixels[i].Z);
                    bytes[j] = (byte)(Math.Clamp(pixels[i].W, 0, 1) * 255);
                });
                return bytes;
            }
        }

        /// <summary>
        /// Render the next frame
        /// </summary>
        public void RenderNextFrame()
        {
            if (world != null && frameBuffer != null && renderer != null && camera != null)
            {
                // render if possible
                if (FrameIndex == 0)
                    frameBuffer.ResetAccumulation();

                renderer.RenderFrameBlocking(frameBuffer, camera, world);
                FrameIndex++;
            }
        }

        public Vector3? Pick(float x, float y)
        {
            if (world != null && frameBuffer != null && renderer != null && camera != null)
            {
                float screenX = x / frameBuffer.Width;
                float screenY = 1f - y / frameBuffer.Height;

                using (var pickResult = renderer.Pick(frameBuffer, camera, world, screenX, screenY))
                {
                    if (pickResult.HasHit)
                        return pickResult.WorldPosition;
                }
            }
            return null;
        }

        /// <summary>
        /// Reset the accumulation of the frame buffer
        /// </summary>
        public void ResetAccumulation() => FrameIndex = 0;


        public void SetDenoiser(bool enabled)
        {
            if (enabled)
            {
                if (denoiser != null)
                    return;

                denoiser = new OSPDenoiser();
            }
            else
            {
                denoiser?.Dispose();
                denoiser = null;
            }

            if (frameBuffer != null)
            {
                SetImageOperations(frameBuffer);
                frameBuffer.Commit();
            }
        }

        public void SetToneMapper(ToneMapperParams? parameters)
        {
            if (parameters.HasValue)
            {
                if (toneMapper == null)
                    toneMapper = new OSPToneMapper();

                toneMapper.SetExposure(parameters.Value.Exposure);
                toneMapper.SetContrast(parameters.Value.Contrast);
                toneMapper.SetShoulder(parameters.Value.Shoulder);
                toneMapper.SetMidIn(parameters.Value.MidIn);
                toneMapper.SetMidOut(parameters.Value.MidOut);
                toneMapper.SetHdrMax(parameters.Value.HdrMax);
                toneMapper.SetAcesColor(parameters.Value.AcesColor);
                toneMapper.Commit();
            }
            else
            {
                toneMapper?.Dispose();
                toneMapper = null;
            }

            if (frameBuffer != null)
            {
                SetImageOperations(frameBuffer);
                frameBuffer.Commit();
            }
        }


        public void Dispose()
        {
            denoiser?.Dispose();
            toneMapper?.Dispose();
            renderer?.Dispose();
            world?.Dispose();
            camera?.Dispose();
            frameBuffer?.Dispose();
        }

        private void SetImageOperations(OSPFrameBuffer frameBuffer)
        {
            List<OSPImageOperation> imageOperations = new List<OSPImageOperation>();
            if (denoiser != null)
                imageOperations.Add(denoiser);

            if (toneMapper != null)
                imageOperations.Add(toneMapper);

            if (imageOperations.Count > 0)
                frameBuffer.SetImageOperations(imageOperations.ToArray());
            else
                frameBuffer.SetImageOperations(null);
        }
    }
}
