using OSPRay;
using OSPRay.Cameras;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private SceneModel? sceneModel = null;

        public RenderContext() 
        {
        }

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

        public OSPFrameBuffer? FrameBuffer => frameBuffer;

        public SceneModel? SceneModel
        {
            get => sceneModel;
            set
            {
                if (sceneModel != value)
                {
                    sceneModel?.Free(this);
                    sceneModel = value;
                    sceneModel?.Setup(this);
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

        public bool CanRenderFrame => world != null && frameBuffer != null && renderer != null && camera != null;

        public void RenderNextFrame()
        {
            if (world != null && frameBuffer != null && renderer != null && camera != null)
            {
                // render if possible
                if (FrameIndex == 0)
                    frameBuffer.ResetAccumulation();

                renderer.ospRenderFrameBlocking(frameBuffer, camera, world);
                FrameIndex++;
            }
        }

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
