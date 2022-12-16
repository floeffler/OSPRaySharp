using Avalonia.Controls.Shapes;
using OSPRay;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OSPRay.TestSuite.Render
{
    public class FrameCompletedEventArgs : EventArgs
    {
        public FrameCompletedEventArgs(OSPFrameBuffer frameBuffer, long timeInMilliseconds)
        {
            FrameBuffer = frameBuffer;
            TimeInMilliseconds = timeInMilliseconds;
        }

        public OSPFrameBuffer FrameBuffer { get; }

        public long TimeInMilliseconds { get; }
    }

    public class ExceptionEventArgs : EventArgs
    {
        public ExceptionEventArgs(Exception exception)
        {
            Exception = exception;
        }

        public Exception Exception { get; }
    }

    internal delegate void RenderCommand(RenderContext? renderContext);

    public class ResizeRequest
    {
        public ResizeRequest(int w, int h)
        {
            Width = w;
            Height = h;
        }

        public int Width { get; }
        public int Height { get; }
    }

    internal class RenderThread
    {
        private AutoResetEvent startEvent = new AutoResetEvent(false);
        private AutoResetEvent stopEvent = new AutoResetEvent(false);
        private AutoResetEvent resumeEvent = new AutoResetEvent(false);
        private Thread? thread = null;

        private ConcurrentQueue<RenderCommand> commands = new ConcurrentQueue<RenderCommand>();
        private int alive = 0;
        private ResizeRequest? currentResizeRequest = null;
        private RenderSettings? renderSettings = null;

        public RenderThread()
        {
        }

        public Action<FrameCompletedEventArgs>? FrameCompletedHandler { get; set; }

        public Action<ExceptionEventArgs>? ExceptionHandler { get; set; }

        public bool InteractiveMode { get; set; }

        public Exception? Exception { get; set; }

        public bool Alive => alive > 0;

        public RenderSettings? RenderSettings => renderSettings;

        public bool EnqueueCommand(RenderCommand command)
        {
            if (Alive)
            {
                commands.Enqueue(command);
                return true;
            }
            return false;
        }

        public void Start()
        {
            if (Alive)
                throw new InvalidOperationException("Thread is running.");

            thread = new Thread(ThreadProc);
            thread.Name = "Renderer Thread";
            thread.Priority = ThreadPriority.Highest;
            thread.Start();

            //wait for startup event
            startEvent.WaitOne();

            // check for errors
            CheckForExceptions();
        }

        public void Stop()
        {
            if (Alive)
            {
                Interlocked.Exchange(ref alive, 0);
                stopEvent.WaitOne();
            }
        }

        public void CheckForExceptions()
        {
            if (Exception != null)
                throw Exception;
        }

        public void Resize(int width, int height)
        {
            Interlocked.Exchange(ref currentResizeRequest, new ResizeRequest(width, height));
        }

        private void ThreadProc()
        {
            RenderContext? renderContext = null;
            try
            {
                renderContext = new RenderContext();

                // create the render settings
                renderSettings = new RenderSettings();
                renderSettings.Setup(renderContext);

                var watch = Stopwatch.StartNew();

            
                Interlocked.Exchange(ref alive, 1);   //mark as running
                startEvent.Set();
                do
                {
                    // handle current resize requests
                    var resizeRequest = Interlocked.Exchange(ref currentResizeRequest, null);
                    if (resizeRequest!= null)
                        renderContext.Resize(resizeRequest.Width, resizeRequest.Height);

                    // process commands
                    while (commands.TryDequeue(out var command))
                    {
                        try
                        {
                            command.Invoke(renderContext);
                        }
                        catch(Exception ex)
                        {
                            ExceptionHandler?.Invoke(new ExceptionEventArgs(ex));
                        }
                    }

                    // update render settings
                    renderSettings?.Update(renderContext); 
                    // update model
                    renderContext.Model?.Update(renderContext);

                    // render stuff
                    if (InteractiveMode && renderContext.CanRenderFrame)
                    {
                        try
                        {
                            watch.Restart();
                            renderContext.RenderNextFrame();
                            var timeInMilliseconds = watch.ElapsedMilliseconds;

                            if (renderContext.FrameBuffer != null)
                                FrameCompletedHandler?.Invoke(new FrameCompletedEventArgs(renderContext.FrameBuffer, timeInMilliseconds));
                        }
                        catch(Exception ex)
                        {
                            ExceptionHandler?.Invoke(new ExceptionEventArgs(ex));
                        }
                    }
                    else
                    {
                        // run with 10fps if nothing to render
                        Thread.Sleep(100);
                    }
                }
                while (alive == 1);
            }
            catch(Exception ex)
            {
                Exception = ex;
            }
            finally
            {
                if (renderContext != null)
                    renderSettings?.Free(renderContext);

                renderSettings = null;
                renderContext?.Dispose();
                renderContext = null;

                // clear queue
                while (commands.TryDequeue(out var command))
                {
                    try
                    {
                        command.Invoke(null);
                    }
                    finally
                    {
                    }
                }

                stopEvent.Set();
            }
        }
    }
}
