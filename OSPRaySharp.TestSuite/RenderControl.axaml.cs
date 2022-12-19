using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.LogicalTree;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using OSPRay.TestSuite.Interaction;
using OSPRay.TestSuite.Render;
using System;
using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OSPRay.TestSuite
{
    internal class RenderControlModel : INotifyPropertyChanged, ICameraPoseProvider
    {
        private static readonly Pose HomePose = new Pose(new Vector3(0.5f, 2f, -3f), Vector3.Zero, Vector3.UnitY);
        private static readonly Pose FrontPose = new Pose(new Vector3(0f, 0f, -3f), Vector3.Zero, Vector3.UnitY);
        private static readonly Pose TopPose = new Pose(new Vector3(0f, 3f, 0f), Vector3.Zero, Vector3.UnitZ);
        private static readonly Pose LeftPose = new Pose(new Vector3(3f, 0f, 0f), Vector3.Zero, Vector3.UnitY);



        private WriteableBitmap? content = null;
        private Renderer? renderer = null;
        private RenderModel? renderModel = null;
        private int filterIndex = 0;
        private int samplesPerPixelIndex = 0;
        private int aoSamplesIndex = 0;
        private Pose cameraPose = HomePose;
        private int rendererIndex = 0;

        private float lensRadius = 0f;
        private float focusDistance = 1f;
        private bool denoiser = false;
        private bool toneMapper = false;
        private ToneMapperParams toneMapperParams = ToneMapperParams.Default;

        public event PropertyChangedEventHandler? PropertyChanged;

        public RenderControlModel()
        {
            Interactor = new TransformInteractor(this);
        }

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
                renderer?.SetRenderModel(renderModel);
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(DenoisingSupported));
            }
        }

        public RenderModel? RenderModel
        {
            get => renderModel;
            set
            {
                renderModel = value;
                Renderer?.SetRenderModel(renderModel);
                NotifyPropertyChanged();
            }
        }

        public int FilterIndex
        {
            get => filterIndex;
            set
            {
                if (filterIndex != value)
                {
                    filterIndex = value;
                    renderer?.SetPixelFilter((OSPPixelFilter)filterIndex);
                    NotifyPropertyChanged();
                }
            }
        }

        public int SamplesPerPixelIndex
        {
            get => samplesPerPixelIndex;
            set
            {
                if (samplesPerPixelIndex != value)
                {
                    samplesPerPixelIndex = value;
                    renderer?.SetRendererSamples(1 << samplesPerPixelIndex, 1 << aoSamplesIndex);
                    NotifyPropertyChanged();
                }
            }
        }

        public int AOSamplesIndex
        {
            get => aoSamplesIndex;
            set
            {
                if (aoSamplesIndex != value)
                {
                    aoSamplesIndex = value;
                    renderer?.SetRendererSamples(1 << samplesPerPixelIndex, 1 << aoSamplesIndex);
                    NotifyPropertyChanged();
                }
            }
        }

        public int RendererIndex
        {
            get => rendererIndex;
            set
            {
                if (rendererIndex != value)
                {
                    rendererIndex = value;
                    renderer?.SetRenderer((RendererType)rendererIndex);
                    renderer?.SetDenoisingEnabled(denoiser);
                    renderer?.SetToneMapper(toneMapper ? toneMapperParams : null);
                    NotifyPropertyChanged();
                    NotifyPropertyChanged(nameof(IsPathTracer));
                }
            }
        }

        public bool IsPathTracer
        {
            get => rendererIndex == 2;
        }

        public Pose CameraPose
        {
            get => cameraPose;
            set
            {
                if (cameraPose != value)
                {
                    cameraPose = value;
                    renderer?.SetCameraPose(cameraPose);
                    NotifyPropertyChanged();
                }
            }
        }

        public float LensRadius
        {
            get => lensRadius;
            set
            {
                if (lensRadius != value)
                {
                    lensRadius = value;
                    renderer?.SetThinLens(focusDistance, lensRadius);
                    NotifyPropertyChanged();
                    NotifyPropertyChanged(nameof(LensRadiusSlider));
                }
            }
        }

        public float FocusDistance
        {
            get => focusDistance;
            set
            {
                if (focusDistance != value)
                {
                    focusDistance = value;
                    renderer?.SetThinLens(focusDistance, lensRadius);
                    NotifyPropertyChanged();
                    NotifyPropertyChanged(nameof(FocusDistanceSlider));
                }
            }
        }

        public bool Denoiser
        {
            get => denoiser;
            set
            {
                if (denoiser != value)
                {
                    denoiser = value;
                    renderer?.SetDenoisingEnabled(denoiser);
                    NotifyPropertyChanged();
                }
            }
        }

        public bool ToneMapper
        {
            get => toneMapper;
            set
            {
                if (toneMapper != value)
                {
                    toneMapper = value;
                    renderer?.SetToneMapper(toneMapper ? toneMapperParams : null);
                    NotifyPropertyChanged();
                }
            }
        }

        public int Exposure
        {
            get => (int)(toneMapperParams.Exposure * 100);
            set
            {
                if (Exposure != value)
                {
                    toneMapperParams.Exposure = value / 100f;
                    if (toneMapper)
                        renderer?.SetToneMapper(toneMapperParams);
                    NotifyPropertyChanged();
                }
            }
        }

        public int HdrMax
        {
            get => (int)(toneMapperParams.HdrMax * 100);
            set
            {
                if (HdrMax != value)
                {
                    toneMapperParams.HdrMax = value / 100f;
                    if (toneMapper)
                        renderer?.SetToneMapper(toneMapperParams);
                    NotifyPropertyChanged();
                }
            }
        }

        public int Contrast
        {
            get => (int)(toneMapperParams.Contrast * 100);
            set
            {
                if (Contrast != value)
                {
                    toneMapperParams.Contrast = (value / 100f);
                    if (toneMapper)
                        renderer?.SetToneMapper(toneMapperParams);
                    NotifyPropertyChanged();
                }
            }
        }

        public int Shoulder
        {
            get => (int)(toneMapperParams.Shoulder * 1000);
            set
            {
                if (Shoulder != value)
                {
                    toneMapperParams.Shoulder = value / 1000f;
                    if (toneMapper)
                        renderer?.SetToneMapper(toneMapperParams);
                    NotifyPropertyChanged();
                }
            }
        }

        public int MidIn
        {
            get => (int)(toneMapperParams.MidIn * 100);
            set
            {
                if (MidIn != value)
                {
                    toneMapperParams.MidIn = value / 100f;
                    if (toneMapper)
                        renderer?.SetToneMapper(toneMapperParams);
                    NotifyPropertyChanged();
                }
            }
        }

        public int MidOut
        {
            get => (int)(toneMapperParams.MidOut * 100);
            set
            {
                if (MidOut != value)
                {
                    toneMapperParams.MidOut = value / 100f;
                    if (toneMapper)
                        renderer?.SetToneMapper(toneMapperParams);
                    NotifyPropertyChanged();
                }
            }
        }

        public bool AcesColor
        {
            get => toneMapperParams.AcesColor;
            set
            {
                if (AcesColor != value)
                {
                    toneMapperParams.AcesColor = value;
                    if (toneMapper)
                        renderer?.SetToneMapper(toneMapperParams);
                    NotifyPropertyChanged();
                }
            }
        }


        public bool DenoisingSupported
        {
            get => renderer?.DenoisingSupported ?? false;
        }

        public int FocusDistanceSlider
        {
            get => (int)(focusDistance * 100);
            set => FocusDistance = value / 100f;
        }

        public int LensRadiusSlider
        {
            get => (int)(lensRadius * 1000);
            set => LensRadius = value / 1000f;
        }


        public TransformInteractor Interactor
        {
            get;
        }

        public void UpdateRenderState()
        {
            renderer?.SetRenderModel(renderModel);
            renderer?.SetPixelFilter((OSPPixelFilter)filterIndex);
            renderer?.SetRendererSamples(1 << samplesPerPixelIndex, 1 << aoSamplesIndex);
            renderer?.SetRenderer((RendererType)rendererIndex);
            renderer?.SetCameraPose(cameraPose);
            renderer?.SetThinLens(focusDistance, lensRadius);
        }


        public void RefreshCommand() => Renderer?.Refresh();

        public void HomeViewCommand() => AnimateCameraTo(HomePose, Interactor.Reset);

        public void FrontViewCommand() => AnimateCameraTo(FrontPose, Interactor.Reset);

        public void TopViewCommand() => AnimateCameraTo(TopPose, Interactor.Reset);

        public void LeftViewCommand() => AnimateCameraTo(LeftPose, Interactor.Reset);

        public void ResetToneMapperCommand()
        {
            toneMapperParams = ToneMapperParams.Default;
            renderer?.SetToneMapper(toneMapper ? toneMapperParams : null);
            NotifyPropertyChanged(nameof(Exposure));
            NotifyPropertyChanged(nameof(Contrast));
            NotifyPropertyChanged(nameof(HdrMax));
            NotifyPropertyChanged(nameof(Shoulder));
            NotifyPropertyChanged(nameof(MidIn));
            NotifyPropertyChanged(nameof(MidOut));
            NotifyPropertyChanged(nameof(AcesColor));
        }

        public Vector3? GetSceneCoordinate(float x, float y) => renderer?.Pick(x, y);

        public void AnimateCameraTo(Pose targetpPose, Action completed)
        {
            var currentPose = CameraPose;
            var task = Task.Run(() =>
            {
                int steps = 500 / 25;
                for (int i = 0; i < steps; ++i)
                {
                    var t = easeInOutCubic(i / (double)steps);
                    var pose = Pose.Lerp(currentPose, targetpPose, (float)t);
                    Dispatcher.UIThread.InvokeAsync(() => CameraPose = pose);
                    Thread.Sleep(25);
                }
                completed?.Invoke();
            });
        }

      
        private double easeInOutCubic(double x)
        {
            return x < 0.5 ? 4 * x * x * x : 1 - Math.Pow(-2 * x + 2, 3) / 2;
        }

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
        
            image.PointerPressed += OnImagePointerPressed;
            image.PointerReleased += OnImagePointerReleased;
            image.PointerMoved += OnImagePointerMoved;
            image.PointerWheelChanged += OnImagePointerWheelChanged;
        }

        private void OnImagePointerMoved(object? sender, PointerEventArgs e)
        {

            var currentPoint = e.GetCurrentPoint(this);
            if (currentPoint != null)
            {
                var p = currentPoint.Position;
                MouseEvent evt = new MouseEvent()
                {
                    X = (float)p.X,
                    Y = (float)p.Y,
                    EventType = MouseEventType.Move,
                };

                model.Interactor.InjectMouseEvent(evt);
            }
        }

        private void OnImagePointerWheelChanged(object? sender, PointerWheelEventArgs e)
        {
            var currentPoint = e.GetCurrentPoint(this);
            if (currentPoint != null)
            {
                var p = currentPoint.Position;
                MouseEvent evt = new MouseEvent()
                {
                    X = (float)p.X,
                    Y = (float)p.Y,
                    EventType = MouseEventType.Wheel,
                    Delta = (float)e.Delta.Y
                };

                model.Interactor.InjectMouseEvent(evt);
            }
        }

        private void OnImagePointerReleased(object? sender, PointerReleasedEventArgs e)
        {
            var currentPoint = e.GetCurrentPoint(this);
            if (currentPoint != null && currentPoint.Properties.IsLeftButtonPressed == false)
            {
                var p = currentPoint.Position;
                MouseEvent evt = new MouseEvent()
                {
                    X = (float)p.X,
                    Y = (float)p.Y,
                    EventType = MouseEventType.Up,
                };

                model.Interactor.InjectMouseEvent(evt);
            }
        }

        private void OnImagePointerPressed(object? sender, PointerPressedEventArgs e)
        {
            var currentPoint = e.GetCurrentPoint(this);

            if (currentPoint != null && currentPoint.Properties.IsLeftButtonPressed == true)
            {
                var p = currentPoint.Position;
                MouseEvent evt = new MouseEvent()
                {
                    X = (float)p.X,
                    Y = (float)p.Y,
                    EventType = MouseEventType.Down,
                };

                model.Interactor.InjectMouseEvent(evt);
            }

            if (currentPoint != null && currentPoint.Properties.IsRightButtonPressed == true)
            {
                var p = currentPoint.Position;
                MouseEvent evt = new MouseEvent()
                {
                    X = (float)p.X,
                    Y = (float)p.Y,
                    EventType = MouseEventType.DblClick,
                };

                model.Interactor.InjectMouseEvent(evt);
            }

            if (currentPoint != null && currentPoint.Properties.IsMiddleButtonPressed== true)
            {
                var p = currentPoint.Position;
                var scenePos = model.GetSceneCoordinate((float)p.X, (float)p.Y);
                if (scenePos.HasValue)
                {
                    var cameraFrame = model.CameraPose.ToFrame();
                    var direction = cameraFrame.Position - scenePos.Value;
                    model.FocusDistance = Vector3.Dot(direction, cameraFrame.Front); // get the new focal plane distance
                }
            }
        }

        private void OnImageEffectiveViewportChanged(object? sender, Avalonia.Layout.EffectiveViewportChangedEventArgs e)
        {
            model.Renderer?.Resize((int)e.EffectiveViewport.Width, (int)e.EffectiveViewport.Height);
        }

        private void OnRendererFrameCompleted(object? sender, FrameCompletedEventArgs e)
        {
            if (updateImage == 0)
            {
                int width = e.Width;
                int height = e.Height;
                byte[] frameData = e.Pixels;

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
                        int rowIndex = (h - i - 1) * w * 4;
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
            model.UpdateRenderState();

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

        internal void SetRenderModel(RenderModel? value)
        {
            model.RenderModel = value;
        }
    }
}
