using Avalonia.Controls;
using Avalonia.LogicalTree;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using OSPRay.TestSuite.Render;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OSPRay.TestSuite
{
    internal class RenderControlModel : INotifyPropertyChanged
    {
        private WriteableBitmap? content = null;
        private Renderer? renderer = null;
        private SceneModel sceneModel = new DefaultSceneModel();


        public event PropertyChangedEventHandler? PropertyChanged;

        public WriteableBitmap? Content
        {
            get => content;
            set
            {
                content = value;
                NotifyPropertyChanged();
            }
        }

        public Renderer? Renderer
        {
            get => renderer;
            set
            {
                renderer = value;
                renderer?.SetSceneModel(sceneModel);
                NotifyPropertyChanged();
            }
        }

        public SceneModel? SceneModel
        {
            get => sceneModel;
            set
            {
                sceneModel = value ?? new SceneModel();
                Renderer?.SetSceneModel(sceneModel);
                NotifyPropertyChanged();
            }
        }

        public void RefreshCommand() => Renderer?.Refresh();

        private void NotifyPropertyChanged([CallerMemberName] string? propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public partial class RenderControl : UserControl
    {
        private RenderControlModel model = new RenderControlModel();
       
        private int updateImage = 0;

        public RenderControl()
        {
            InitializeComponent();
            DataContext = model;
            image.EffectiveViewportChanged += OnImageEffectiveViewportChanged;
        }

        private void OnImageEffectiveViewportChanged(object? sender, Avalonia.Layout.EffectiveViewportChangedEventArgs e)
        {
            model.Renderer?.Resize((int)e.EffectiveViewport.Width, (int)e.EffectiveViewport.Height);
        }

        private void OnRendererFrameCompleted(object? sender, FrameCompletedEventArgs e)
        {
            if (updateImage == 0)
            {
                int width = e.FrameBuffer.Width;
                int height = e.FrameBuffer.Height;
                byte[] frameData;
                using (var mappedData = e.FrameBuffer.Map())
                {
                    frameData = mappedData.GetSpan<byte>().ToArray();
                }


                // prepare data on a background task
                Interlocked.Increment(ref updateImage);
                Dispatcher.UIThread.InvokeAsync(() => UpdateImageContent(width, height, frameData), DispatcherPriority.Normal);
            }
        }

        private async void UpdateImageContent(int w, int h, byte[] data)
        {
            var content = model.Content;
            if (content == null || content.PixelSize.Width != w || content.PixelSize.Height != h)
            {
                content = new WriteableBitmap(
                    new Avalonia.PixelSize(w, h),
                    new Avalonia.Vector(96, 96),
                    Avalonia.Platform.PixelFormat.Rgba8888, 
                    Avalonia.Platform.AlphaFormat.Opaque);
            }

            // copy to image
            await Task.Run(() =>
            {
                using (var contentBuffer = content.Lock())
                {
                    for (int i = 0; i < h; ++i)
                    {
                        int rowIndex = i * w * 4;
                        var dstAddress = contentBuffer.Address + (contentBuffer.RowBytes * i);
                        Marshal.Copy(data, rowIndex, dstAddress, w * 4);
                    }
                }
            });

            Interlocked.Decrement(ref updateImage);
            model.Content = content;
            image.InvalidateVisual();
        }

        private void OnRendererException(object? sender, ExceptionEventArgs e)
        {
            // TODO notify error
        }

        public async void Initialize()
        {
            model.Renderer = await CreateRenderer();
            model.Renderer.Resize((int)image.Width, (int)image.Height);
            model.Renderer.SetInteractive(true);

            initPanel.IsVisible = false;
            image.IsVisible = true;
        }

        public void Shutdown()
        {
            model.Renderer?.Dispose();
            model.Renderer = null;
        }

        private Task<Renderer> CreateRenderer()
        {
            return Task.Run(() =>
            {
                var renderer = new Renderer();
                renderer.Exception += OnRendererException;
                renderer.FrameCompleted += OnRendererFrameCompleted;
                return renderer;
            });
        }
    }
}
