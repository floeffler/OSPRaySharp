using OSPRay.Geometries;
using OSPRay.Lights;
using OSPRay.Materials;
using OSPRay.TestSuite.Render;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OSPRay.TestSuite.Scenes.RenderModels
{
    internal class Boxes : RenderModelBase
    {
        private int dimensions = 4;
        private bool useLight = false;

        public Boxes()
        {
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

        public int Dimensions
        {
            get => dimensions;
            set
            {
                if (dimensions != value)
                {
                    dimensions = value;
                    NotifyChangedAll();
                }
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

        protected override OSPGroup BuildGroup()
        {
            using var boxGeometry = new OSPBoxGeometry();


            float half = Dimensions / 2f;
            float size = 2f / Dimensions;

            var boxes = new List<OSPBounds>();
            var colors = new List<Vector4>();

            for (int z = 0; z < Dimensions; z++)
            {
                for (int y = 0; y < Dimensions; y++)
                {
                    for (int x = 0; x < Dimensions; x++)
                    {
                        var lower = new Vector3(x - half, y - half, z - half) * size;
                        var upper = lower + new Vector3(0.75f * size);
                        boxes.Add(new OSPBounds(lower, upper));

                        var boxColor = 0.8f * new Vector3(x, y, z) / Dimensions + new Vector3(0.2f);
                        colors.Add(new Vector4(boxColor, 1f));
                    }
                }
            }

            boxGeometry.SetBox(boxes.ToArray());
            boxGeometry.Commit();


            using var model = new OSPGeometricModel(boxGeometry);
            model.SetColor(colors.ToArray());
            using (var material = new OSPObjMaterial())
            {
                material.SetSpecular(new Vector3(0.3f));
                material.SetShininess(10f);
                material.Commit();
                model.SetMaterials(material);
            }
            model.Commit();

            var group = new OSPGroup();
            group.SetGeometry(model);
            group.Commit();

            return group;
        }
    }
}
