using OSPRay.Lights;
using OSPRay.TestSuite.Render;
using OSPRay.TransferFunctions;
using OSPRay.Volumes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay.TestSuite.Scenes.RenderModels
{
    internal class Stagbeetle : RenderModelBase
    {
        private static readonly Vector3[] DefaultColor = new Vector3[] { new Vector3(1f, 0.3f, 0.6f), new Vector3(0.9f, 1f, 0.3f), new Vector3(0.3f, 0.6f, 1f) };
        private static readonly float[] DefaultOpacity = new float[] { 0f, 0f, 0.5f, 1f };

        OSPStructuredRegularVolume? volume = null;
        float density = 1f;
        float[] opacity = DefaultOpacity;
        Vector3[] color = DefaultColor;
        bool useLight = true;

        public Stagbeetle()
        {
        }

        public float Density
        {
            get => density;
            set
            {
                if (density != value)
                {
                    density = value;
                    NotifyChangedAll();
                }
            }
        }

        public bool UseLight
        {
            get => useLight;
            set
            {
                if (useLight != value)
                {
                    useLight = value;
                    NotifyChangedAll();
                }
            }
        }

        public float[] Opacity
        {
            get => opacity;
            set
            {
                if (opacity != value)
                {
                    if (value == null || value.Length == 0)
                        opacity = DefaultOpacity;
                    else
                        opacity = value;
                }

                NotifyChangedAll();
            }
        }

        public Vector3[] Color
        {
            get => color;
            set
            {
                if (color != value)
                {
                    if (value == null || value.Length == 0)
                        color = DefaultColor;
                    else
                        color = value;
                }

                NotifyChangedAll();
            }
        }

        public string VolumeFilePath
        {
            get
            {
                var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var dataPath = System.IO.Path.Combine(appDataPath, "OSPRaySharp");
                return System.IO.Path.Combine(dataPath, "stagbeetle832x832x494.dat");
            }
        }

        internal override void Free(RenderContext renderContext)
        {
            volume?.Dispose();
            volume = null;
            base.Free(renderContext);
        }

        protected override OSPGroup BuildGroup()
        {
            Random random = new Random();

            if (volume == null)
            {
                string volumeFilePath = VolumeFilePath;
                if (File.Exists(volumeFilePath))
                {
                    using (var binaryReader = new BinaryReader(
                        new BufferedStream(new FileStream(volumeFilePath, FileMode.Open))))
                    {
                        int w = binaryReader.ReadInt16();
                        int h = binaryReader.ReadInt16();
                        int d = binaryReader.ReadInt16();

                        var data = new float[w * h * d];
                        for (int i = 0; i < data.Length; ++i)
                        {
                            data[i] = binaryReader.ReadUInt16() / 2048f; // we know the range
                        }

                        using var dataObject = OSPDataFactory.CreateData3D(data, w, h, d);
                        volume = new OSPStructuredRegularVolume();
                        volume.SetData(dataObject);
                        volume.SetGridOrgin(new Vector3(-1.5f));
                        volume.SetGridSpacing(new Vector3(3f / (w-1)));
                        volume.Commit();
                    }
                }
            }

            if (volume != null) {

                using var transferFunction = CreateTransferFunction();
                using var model = new OSPVolumetricModel(volume);
                model.SetTransferFunction(transferFunction);
                model.SetDensityScale(density);
                model.Commit();

                var group = new OSPGroup();
                group.SetVolume(model);
                group.Commit();
                return group;
            }
            else
            {
                return new OSPGroup();
            }
        }

        protected override OSPWorld BuildWorld()
        {
            var world = base.BuildWorld();
            if (UseLight == false)
            {
                return world;
            }

            using var light = new OSPDistantLight();

            light.SetColor(new Vector3(0.78f, 0.551f, 0.483f));
            light.SetIntensity(3.14f);
            light.SetDirection(new Vector3(-0.8f, -0.6f, 0.3f));
            light.Commit();

            using var ambient = new OSPAmbientLight();
            ambient.SetIntensity(0.35f);
            ambient.SetVisible(false);
            ambient.Commit();
            world.SetLights(light, ambient);
            world.Commit();

            return world;
        }

        private OSPTransferFunction CreateTransferFunction()
        {
            var transferFunction = new OSPPiecewiseLinearTransferFunction();

            transferFunction.SetDomain(0f, 1f);
            transferFunction.SetOpacity(opacity);
            transferFunction.SetColor(color);
            transferFunction.Commit();
            return transferFunction;
        }
    }
}
