using OSPRay.Geometries;
using OSPRay.Lights;
using OSPRay.Materials;
using OSPRay.TestSuite.Render;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace OSPRay.TestSuite.Scenes.RenderModels
{
    internal class Streamlines : RenderModelBase
    {
        private int numLines = 100;
        private bool useLight = true;

        public int NumLines
        {
            get => numLines;
            set
            {
                if (numLines != value)
                {
                    numLines = value;
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


        protected override OSPGroup BuildGroup()
        {
            var points = new List<Vector4>();
            var indices = new List<int>();
            var colors = new List<Vector4>();

            var random = new Random(RandomSeed);

            var radDist = new UniformDistribution(0.5f, 1.5f);
            var stepDist = new UniformDistribution(0.001f, 0.1f);
            var sDist = new UniformDistribution(0, 360);
            var dDist = new UniformDistribution(360, 720);
            var freqDist = new UniformDistribution(0.5f, 1.5f);

            // create multiple lines
            int numLines = NumLines;
            for (int l = 0; l < numLines; l++)
            {
                int dStart = sDist.SampleInt(random);
                int dEnd = dDist.SampleInt(random);
                float radius = radDist.SampleSingle(random);
                float h = 0;
                float hStep = stepDist.SampleSingle(random);
                float f = freqDist.SampleSingle(random);

                float r = (720 - dEnd) / 360f;
                var c = new Vector4(r, 1 - r, 1 - r / 2, 1f);

                // spiral up with changing radius of curvature
                for (int d = dStart; d < dStart + dEnd; d += 10, h += hStep)
                {
                    var p = Vector3.Zero;
                    var q = Vector3.Zero;
                    float startRadius, endRadius;

                    p.X = radius * (float)Math.Sin(d * Math.PI / 180f);
                    p.Y = h - 2;
                    p.Z = radius * (float)Math.Cos(d * Math.PI / 180f);
                    startRadius = 0.015f * (float)Math.Sin(f * d * Math.PI / 180) + 0.02f;

                    q.X = (radius - 0.05f) * (float)Math.Sin((d + 10) * Math.PI / 180f);
                    q.Y = h + hStep - 2;
                    q.Z = (radius - 0.05f) * (float)Math.Cos((d + 10) * Math.PI / 180f);
                    endRadius = 0.015f * (float)Math.Sin(f * (d + 10) * Math.PI / 180) + 0.02f;

                    if (d == dStart)
                    {
                        var rim = Vector3.Lerp(q, p, 1f + endRadius / (q - p).Length());
                        var cap = Vector3.Lerp(p, rim, 1f + startRadius / (rim - p).Length());
                        points.Add(new Vector4(cap, 0f));
                        points.Add(new Vector4(rim, 0f));
                        points.Add(new Vector4(p, startRadius));
                        points.Add(new Vector4(q, endRadius));
                        indices.Add(points.Count - 4);
                        colors.Add(c);
                        colors.Add(c);
                    }
                    else if (d + 10 < dStart + dEnd && d + 20 > dStart + dEnd)
                    {
                        var rim = Vector3.Lerp(p, q, 1f + startRadius / (p - q).Length());
                        var cap = Vector3.Lerp(q, rim, 1f + endRadius / (rim - q).Length());
                        points.Add(new Vector4(p, startRadius));
                        points.Add(new Vector4(q, endRadius));
                        points.Add(new Vector4(rim, 0f));
                        points.Add(new Vector4(cap, 0f));
                        indices.Add(points.Count - 7);
                        indices.Add(points.Count - 6);
                        indices.Add(points.Count - 5);
                        indices.Add(points.Count - 4);
                        colors.Add(c);
                        colors.Add(c);
                    }
                    else if ((d != dStart && d != dStart + 10) && d + 20 < dStart + dEnd)
                    {
                        points.Add(new Vector4(p, startRadius));
                        indices.Add(points.Count - 4);
                    }
                    colors.Add(c);
                    radius -= 0.05f;
                }
            }

            var slGeom = new OSPCurveGeometry();


            slGeom.SetPositionRadius(points.ToArray());
            slGeom.SetIndex(indices.ToArray());
            slGeom.SetColor(colors.ToArray());
            slGeom.SetType(OSPCurveType.Round);
            slGeom.SetBasis(OSPCurveBasis.CatmullRom);
            slGeom.Commit();

            
            var material = new OSPPrincipledMaterial();
            material.SetParam("metallic", 0.5f);
            material.SetParam("roughness", 0.5f);

            material.Commit();

            var model = new OSPGeometricModel(slGeom);
            model.SetMaterials(material);
            model.Commit();

            var group = new OSPGroup();
            group.SetGeometry(model);
            group.Commit();

            return group;
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
    }
}
