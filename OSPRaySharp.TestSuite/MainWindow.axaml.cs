using Avalonia.Controls;
using OSPRay.TestSuite.Render;
using OSPRay.TestSuite.Scenes;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Tmds.DBus;

namespace OSPRay.TestSuite
{
    internal class MainWindowViewModel : INotifyPropertyChanged
    {
        private int sceneIndex = 0;

        public event PropertyChangedEventHandler? PropertyChanged;

        public MainWindowViewModel()
        {
            Scenes = new SceneViewModel[] { 
                new BoxesSceneViewModel(),
                new SphereSceneViewModel(),
                new CornellBoxViewModel()
            };
        }

        public int SceneIndex
        {
            get => sceneIndex;
            set
            {
                if (sceneIndex != value)
                {
                    sceneIndex = value;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged(nameof(CurrentScene));
                }
            }
        }

        public SceneViewModel CurrentScene => Scenes[SceneIndex];

        public SceneViewModel[] Scenes
        {
            get;
        }

        private void NotifyPropertyChanged([CallerMemberName] string? propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public partial class MainWindow : Window
    {
        private MainWindowViewModel model;

        public MainWindow()
        {
            InitializeComponent();
            model = new MainWindowViewModel();
            model.PropertyChanged += OnCurrentSceneChangedChanged;
            this.DataContext = model;
        }

        private void OnCurrentSceneChangedChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MainWindowViewModel.CurrentScene))
            {
                renderControl.SetRenderModel(model.CurrentScene.RenderModel);
            }
        }

        protected override void OnOpened(EventArgs e)
        {
            base.OnOpened(e);
            renderControl.Initialize();
            renderControl.SetRenderModel(model.CurrentScene.RenderModel);
        }

        protected override void OnClosed(EventArgs e)
        {
            renderControl.SetRenderModel(null);
            renderControl.Shutdown();
            base.OnClosed(e);
        }
    }
}
