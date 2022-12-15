using OSPRay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OSPRay.TestSuite.Render
{
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

        public void SetSceneModel(SceneModel? sceneModel)
        {
            bool res = InvokeAsync(x =>
            {
                if (x != null)
                    x.SceneModel = sceneModel;
            });

            if (res == false)
                throw new InvalidOperationException("Renderthread is not alive.");
        }

        public void Refresh() => InvokeAsync(x => x?.ResetAccumulation());

        public void Resize(int width, int height) => renderThread.Resize(width, height);

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
