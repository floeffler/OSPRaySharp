using Avalonia.Media;
using OSPRay.Geometries;
using OSPRay.Lights;
using OSPRay.Materials;
using OSPRay.TestSuite.Render;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay.TestSuite.Scenes
{
    internal class SphereSceneRenderModel : RenderModel
    {
        private int sphereCount = 10;
     
        public int SphereCount
        {
            get => sphereCount;
            set
            {
                if (sphereCount != value)
                {
                    sphereCount = value;
                    NotifyChangedAll();
                }
            }
        }


        internal override void Setup(RenderContext renderContext)
        {
            using var instance = BuildSpheres(sphereCount);

            using var light1 = new OSPSphereLight();
            light1.SetPosition(new Vector3(0f, 0f, -3f));
            light1.SetRadius(0.1f);
            light1.SetIntensity(2f);
            light1.Commit();

            using var light2 = new OSPSphereLight();
            light2.SetPosition(new Vector3(0f, 0f, 3f));
            light2.SetRadius(0.1f);
            light2.SetIntensity(2f);
            light2.Commit();

            var world = new OSPWorld();
            world.SetInstances(instance);
            world.SetLights(light1, light2);
            world.Commit();
            renderContext.World = world;
        }

      
        internal override void Free(RenderContext renderContext)
        {
            renderContext.World = null;
        }

        private OSPInstance BuildSpheres(int sphereCount)
        {
            Random random = new Random(16480 + sphereCount);

            var position = new Vector3[sphereCount];
            var radius = new float[sphereCount];
            var colors = new Vector4[sphereCount];

            for (int i = 0; i < sphereCount; ++i)
            {
                float x = (float)random.NextDouble() * 2f - 1f;
                float y = (float)random.NextDouble() * 2f - 1f;
                float z = (float)random.NextDouble() * 2f - 1f;

                float r = (float)random.NextDouble() * 0.75f + 0.25f;
                float g = (float)random.NextDouble() * 0.75f + 0.25f;
                float b = (float)random.NextDouble() * 0.75f + 0.25f;

                position[i] = new Vector3(x, y, z);
                colors[i] = new Vector4(r, g, b, 1f);
                radius[i] = (float)random.NextDouble() * 0.25f + 0.1f;
            }

            using var mesh = new OSPSphereGeometry();
            mesh.SetPositions(position);
            mesh.SetRadii(radius);
            mesh.Commit();

            using var model = new OSPGeometricModel(mesh);
            model.SetMaterials(new OSPMetallicPaintMaterial());
            model.SetColor(colors);
            model.Commit();

            using var group = new OSPGroup();
            group.SetGeometry(model);
            group.Commit();

            using var instance = new OSPInstance(group);
            instance.Commit();
            return instance;
        }

        protected override void UpdateCore(RenderContext renderContext, int stateChanges)
        {
            using var instance = BuildSpheres(sphereCount);
            var world = renderContext.World;
            if (world != null)
            {
                world.SetInstances(instance);
                world.Commit();
            }

            renderContext.ResetAccumulation();
        }
    }
}
