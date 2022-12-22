using Avalonia.Threading;
using OSPRay.TestSuite.Render;
using OSPRay.TestSuite.Scenes.RenderModels;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay.TestSuite.Scenes
{
    internal class StagbeetleViewModel : SceneViewModel
    {
        private Stagbeetle renderModel = new Stagbeetle();
        private int progressValue;
        private int density = 100;
        private int threshold = 10;
        private int steepness = 10;
        private int opacity = 100;


        public StagbeetleViewModel() : base("Stagbeetle")
        {
        }

        public int Density
        {
            get => density;
            set
            {
                if (density != value)
                {
                    density = value;
                    renderModel.Density = density / 100f;
                    NotifyPropertyChanged();
                }
            }
        }

        public int Threshold
        {
            get => threshold;
            set
            {
                if (threshold != value)
                {
                    threshold = value;
                    UpdateOpacity();
                    NotifyPropertyChanged();
                }
            }
        }

        public int Steepness
        {
            get => steepness;
            set
            {
                if (steepness != value)
                {
                    steepness = value;
                    UpdateOpacity();
                    NotifyPropertyChanged();
                }
            }
        }

        public int Opacity
        {
            get => opacity;
            set
            {
                if (opacity != value)
                {
                    opacity = value;
                    UpdateOpacity();
                    NotifyPropertyChanged();
                }
            }
        }

        public bool UseLight
        {
            get => renderModel.UseLight;
            set
            {
                if (renderModel.UseLight != value)
                {
                    renderModel.UseLight = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private void UpdateOpacity()
        {
            float p1 = threshold / 100f;
            float p2 = p1 + (1f - p1) * (1f - steepness / 100f);
            float o = opacity / 100f;

            float range = p2 - p1;
            float invRange = range > 0f ? 1f / range : 0f;

            float[] curve = new float[256];
            for (int i= 0; i < curve.Length; ++i)
            {
                float p = i / (float)(curve.Length - 1);
                float t;
                if (p <= p1)
                    t = 0f;
                else if (p >= p2)
                    t = 1f;
                else
                    t = (p - p1) * invRange;

                curve[i] = t * o;
            }
            renderModel.Opacity = curve;
        }

        private void DownloadCommand()
        {
            string zipFilePath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "stagbeetle.zip");
            using (WebClient webClient = new WebClient())
            {
                webClient.DownloadProgressChanged += DownloadProgressChanged;
                webClient.DownloadFileCompleted += (s, e) =>
                {
                    var directory = System.IO.Path.GetDirectoryName(renderModel.VolumeFilePath);

                    if (System.IO.Directory.Exists(directory))
                        System.IO.Directory.CreateDirectory(directory);

                    ZipFile.ExtractToDirectory(zipFilePath, directory);

                    Dispatcher.UIThread.Post(() => {
                        renderModel.Refresh();
                        ProgressValue = 0;
                        NotifyPropertyChanged(nameof(NeedDownload));
                    });
                };

                webClient.DownloadFileAsync(
                    new System.Uri("https://www.cg.tuwien.ac.at/research/publications/2005/dataset-stagbeetle/dataset-stagbeetle-832x832x494.zip"),
                    zipFilePath);
            }
        }

        public bool NeedDownload
        {
            get
            {
                var path = renderModel.VolumeFilePath;
                return System.IO.File.Exists(path) == false;
            }
        }

        public int ProgressValue
        {
            get => progressValue;
            private set
            {
                if (progressValue != value)
                {
                    progressValue = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private void DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Dispatcher.UIThread.Post(() => ProgressValue = e.ProgressPercentage);
        }

        public override RenderModel RenderModel => renderModel;
    }
}
