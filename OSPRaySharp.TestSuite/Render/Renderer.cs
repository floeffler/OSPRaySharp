using Avalonia.Controls;
using OSPRay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OSPRay.TestSuite.Render
{

    internal class ValueReference<T> where T : struct
    {
        public T? Value { get; set; }
    }


    internal class Renderer : IDisposable
    {
        private OSPLibrary ospray;
        private OSPDevice device;
        private RenderThread renderThread;

        public Renderer()
        {
            ospray = new OSPLibrary();
            DenoisingSupported = ospray.TryLoadModule("denoiser");
            device = ospray.CreateCPUDevice(numberOfThreads: Environment.ProcessorCount - 1, setThreadAffinity: true);
            device.SetCurrent();

            renderThread = new RenderThread();
            renderThread.FrameCompletedHandler = args => FrameCompleted?.Invoke(this, args);
            renderThread.ExceptionHandler = args => Exception?.Invoke(this, args);
            renderThread.Start();

        }

        /// <summary>
        /// List of handler which are notify if a frame has been rendered. The handlers are
        /// invoke in the context of the render thread.
        /// </summary>
        public event EventHandler<FrameCompletedEventArgs>? FrameCompleted;

        /// <summary>
        /// List of handler which are notify if a exception occur in the render thread. 
        /// The handlers are invoke in the context of the render thread.
        /// </summary>
        public event EventHandler<ExceptionEventArgs>? Exception;

        public bool DenoisingSupported { get; }

        public void SetInteractive(bool interactive)
        {
            renderThread.InteractiveMode = interactive;
        }

        /// <summary>
        /// Sets the model to render 
        /// </summary>
        /// <param name="model"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public void SetModel(Model? model)
        {
            bool res = InvokeAsync(x =>
            {
                if (x != null)
                    x.Model = model;
            });

            if (res == false)
                throw new InvalidOperationException("Renderthread is not alive.");
        }

        public void Refresh() => InvokeAsync(x => x?.ResetAccumulation());

        public void Resize(int width, int height) => renderThread.Resize(width, height);

        public void SetCameraPose(Pose cameraPose)
        {
            var settings = renderThread.RenderSettings;
            if (settings != null)
            {
                settings.CameraPose = cameraPose;
            }
        }

        public void SetThinLens(float focalDistance, float apertureRadius)
        {
            var settings = renderThread.RenderSettings;
            if (settings != null)
            {
                settings.FocusDistance = focalDistance;
                settings.ApertureRadius = apertureRadius;
            }
        }

        public void SetRenderer(RendererType rendererType)
        {
            var settings = renderThread.RenderSettings;
            if (settings != null)
            {
                settings.RendererType = rendererType;
            }
        }

        public void SetRendererSamples(int samplesPerPixel, int aoSamples)
        {
            var settings = renderThread.RenderSettings;
            if (settings != null)
            {
                settings.SamplesPerPixel = samplesPerPixel;
                settings.AOSamples = aoSamples;
            }
        }

        public void SetPixelFilter(OSPPixelFilter pixelFilter)
        {
            var settings = renderThread.RenderSettings;
            if (settings != null)
            {
                settings.PixelFilter = pixelFilter;
            }
        }

        public Vector3? Pick(float windowX, float windowY)
        {
            ManualResetEvent manualReset = new ManualResetEvent(false);
            var output = new ValueReference<Vector3>();

            bool res = InvokeAsync(x =>
            {
                try
                {
                    if (x != null)
                    {
                        output.Value = x.Pick(windowX, windowY);
                    }
                }
                finally
                {
                    manualReset.Set();
                }
            });
            if (res)
            {
                manualReset.WaitOne();
                return output.Value;
            }
            return null;
        }

        /// <summary>
        /// blocks the current thread until the current enquened work is completed
        /// </summary>
        public void Synchronize()
        {
            ManualResetEvent manualReset = new ManualResetEvent(false);
            if (InvokeAsync(x => manualReset.Set()) == true)
                manualReset.WaitOne();
        }

        internal bool InvokeAsync(RenderCommand command) => renderThread.EnqueueCommand(command);
        
        public void Dispose()
        {
            renderThread.Stop();
            device.Dispose();
            ospray.Dispose();
        }
    }
}
