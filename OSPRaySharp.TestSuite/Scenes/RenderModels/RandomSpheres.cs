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

            var centerSampler = new UniformDistribution(-1f, 1f);
            var radiiSampler = new UniformDistribution(0.05f, 0.15f);
            var colorSampler = new UniformDistribution(0.5f, 1f);


            for (int i = 0; i < count; i++)
            {
                center[i] = new Vector3(
                    centerSampler.SampleSingle(random),
                    centerSampler.SampleSingle(random),
                    centerSampler.SampleSingle(random));

                radius[i] = radiiSampler.SampleSingle(random);

                color[i] = new Vector4(
                    colorSampler.SampleSingle(random),
                    colorSampler.SampleSingle(random),
                    colorSampler.SampleSingle(random),
                    colorSampler.SampleSingle(random));
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
