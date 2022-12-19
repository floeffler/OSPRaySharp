using OSPRay.Geometries;
using OSPRay.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay.TestSuite.Scenes.RenderModels
{
    internal class RandomSpheres : RenderModelBase
    {
        private int numSpheres = 100;

        public RandomSpheres()
        {
            ShowGroundPlane = true;
        }

        public int NumSpheres
        {
            get => numSpheres;
            set
            {
                if (numSpheres != value)
                {
                    numSpheres = value;
                    NotifyChangedAll();
                }
            }
        }

        protected override OSPGroup BuildGroup()
        {
            Random random = new Random(RandomSeed + NumSpheres);
            int count = NumSpheres;

            var center = new Vector3[count];
            var radius = new float[count];
            var color = new Vector4[count];

            for (int i = 0; i < count; i++)
            {
                center[i] = new Vector3(
                    random.NextSingle() * 2f - 1f,
                    random.NextSingle() * 2f - 1f,
                    random.NextSingle() * 2f - 1f);

                radius[i] = random.NextSingle() * 0.1f + 0.05f;

                color[i] = new Vector4(
                    random.NextSingle() * 0.5f + 0.5f,
                    random.NextSingle() * 0.5f + 0.5f,
                    random.NextSingle() * 0.5f + 0.5f,
                    random.NextSingle() * 0.5f + 0.5f);
            }


            using var spheresGeometry = new OSPSphereGeometry();
            spheresGeometry.SetPositions(center);
            spheresGeometry.SetRadii(radius);
            spheresGeometry.Commit();

            using var material = new OSPThinGlassMaterial();
            material.SetAttenuationDistance(0.2f);
            material.Commit();

            using var model = new OSPGeometricModel(spheresGeometry);
            model.SetColor(color);
            model.SetMaterials(material);
            model.Commit();

            var group = new OSPGroup();
            group.SetGeometry(model);
            group.Commit();

            return group;
        }
    }
}
