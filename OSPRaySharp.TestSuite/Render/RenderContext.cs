using OSPRay;
using OSPRay.Cameras;
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
        private Model? model = null;

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
        public Model? Model
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
                    frameBuffer = new OSPFrameBuffer(width, height);
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
                float screenY = y / frameBuffer.Height;

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

        public void Dispose()
        {
            renderer?.Dispose();
            world?.Dispose();
            camera?.Dispose();
            frameBuffer?.Dispose();
        }
    }
}
