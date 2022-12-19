using Avalonia;
using Avalonia.Media;
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

namespace OSPRay.TestSuite.Scenes.RenderModels
{
    internal abstract class RenderModelBase : RenderModel
    {
        private bool showGroundPlane = false;
        private int randomSeed = 16480;

        public RenderModelBase(): base()
        {
        }

        public bool ShowGroundPlane 
        {
            get => showGroundPlane;
            set
            {
                if (showGroundPlane != value)
                {
                    showGroundPlane = value;
                    NotifyChangedAll();
                }
            }
        }


        public int RandomSeed
        {
            get => randomSeed;
            set
            {
                if (randomSeed != value)
                {
                    randomSeed= value;
                    NotifyChangedAll();
                }
            }
        }


        internal override void Setup(RenderContext renderContext)
        {
            UpdateCore(renderContext, ALL_STATES_BIT);
        }

        internal override void Free(RenderContext renderContext)
        {
            renderContext.World = null;
        }

        protected override void UpdateCore(RenderContext renderContext, int stateChanges)
        {
            var world = BuildWorld();
            world.Commit();
            renderContext.World = world;
            renderContext.ResetAccumulation();
        }

        protected virtual OSPWorld BuildWorld() => BuildWorld(null);

        protected abstract OSPGroup BuildGroup();

        protected OSPWorld BuildWorld(OSPInstance[]? instances)
        {
            using var group = BuildGroup();
            var instance = new OSPInstance(group);
            instance.Commit();

            var worldInstances = new List<OSPInstance>();
            if (instances != null)
            {
                worldInstances.AddRange(instances);
            }
            worldInstances.Add(instance);


            OSPInstance? groundPlane = null;
            if (ShowGroundPlane)
            {
                groundPlane = MakeGroundPlane(group.Bounds);
                worldInstances.Add(groundPlane);
            }

            using var light = new OSPAmbientLight();
            light.SetVisible(false);
            light.Commit();


            var world = new OSPWorld();
            world.SetInstances(worldInstances.ToArray());
            world.SetLights(light);
            world.Commit();

            instance.Dispose();
            groundPlane?.Dispose();

            return world;
        }

        private OSPInstance MakeGroundPlane(OSPBounds bounds)
        {
            var planeExtent = 0.8f * (bounds.Center - bounds.Lower).Length();

            using var planeGeometry = new OSPMeshGeometry();

            var v_position = new List<Vector3>();
            var v_normal = new List<Vector3>();
            var v_color = new List<Vector4>();
            var indices = new List<int>();

            int startingIndex = 0;

            var gray = new Vector4(0.9f, 0.9f, 0.9f, 0.75f);

            v_position.Add(new Vector3(-planeExtent, -1f, -planeExtent));
            v_position.Add(new Vector3(planeExtent, -1f, -planeExtent));
            v_position.Add(new Vector3(planeExtent, -1f, planeExtent));
            v_position.Add(new Vector3(-planeExtent, -1f, planeExtent));

            v_normal.Add(Vector3.UnitY);
            v_normal.Add(Vector3.UnitY);
            v_normal.Add(Vector3.UnitY);
            v_normal.Add(Vector3.UnitY);

            v_color.Add(gray);
            v_color.Add(gray);
            v_color.Add(gray);
            v_color.Add(gray);

            indices.AddRange(new int[] {
                startingIndex, startingIndex + 1, startingIndex + 2, startingIndex + 3 });

            // stripes on ground plane
            float stripeWidth = 0.025f;
            float paddedExtent = planeExtent + stripeWidth;
            int numStripes = 10;

            var stripeColor = new Vector4(1f, 0.1f, 0.1f, 1f);

            for (int i = 0; i < numStripes; i++)
            {
                // the center coordinate of the stripe, either in the x or z
                // direction
                float coord = -planeExtent + (float)i / (numStripes - 1) * 2f * planeExtent;

                // offset the stripes by an epsilon above the ground plane
                float yLevel = -1f + 1e-3f;

                // x-direction stripes
                startingIndex = v_position.Count;

                v_position.Add(new Vector3(-paddedExtent, yLevel, coord - stripeWidth));
                v_position.Add(new Vector3(paddedExtent, yLevel, coord - stripeWidth));
                v_position.Add(new Vector3(paddedExtent, yLevel, coord + stripeWidth));
                v_position.Add(new Vector3(-paddedExtent, yLevel, coord + stripeWidth));

                v_normal.Add(Vector3.UnitY);
                v_normal.Add(Vector3.UnitY);
                v_normal.Add(Vector3.UnitY);
                v_normal.Add(Vector3.UnitY);

                v_color.Add(stripeColor);
                v_color.Add(stripeColor);
                v_color.Add(stripeColor);
                v_color.Add(stripeColor);

                indices.AddRange(new int[] {
                    startingIndex, startingIndex + 1, startingIndex + 2, startingIndex + 3 });

                // z-direction stripes
                startingIndex = v_position.Count;

                // offset another epsilon to avoid z-figthing for primID AOV
                float yLevel2 = yLevel + 1e-4f;

                v_position.Add(new Vector3(coord - stripeWidth, yLevel2, -paddedExtent));
                v_position.Add(new Vector3(coord + stripeWidth, yLevel2, -paddedExtent));
                v_position.Add(new Vector3(coord + stripeWidth, yLevel2, paddedExtent));
                v_position.Add(new Vector3(coord - stripeWidth, yLevel2, paddedExtent));

                v_normal.Add(Vector3.UnitY);
                v_normal.Add(Vector3.UnitY);
                v_normal.Add(Vector3.UnitY);
                v_normal.Add(Vector3.UnitY);

                v_color.Add(stripeColor);
                v_color.Add(stripeColor);
                v_color.Add(stripeColor);
                v_color.Add(stripeColor);

                indices.AddRange(new int[] {
                    startingIndex, startingIndex + 1, startingIndex + 2, startingIndex + 3 });
            }

            planeGeometry.SetVertexPositions(v_position.ToArray());
            planeGeometry.SetVertexNormals(v_normal.ToArray());
            planeGeometry.SetVertexColors(v_color.ToArray());
            planeGeometry.SetIndices(indices.ToArray(), true);
            planeGeometry.Commit();


            using var plane = new OSPGeometricModel(planeGeometry);
            using (var material = new OSPObjMaterial())
            {
                material.Commit();
                plane.SetMaterials(material);
            }
            plane.Commit();

            using var planeGroup = new OSPGroup();
            planeGroup.SetGeometry(plane);
            planeGroup.Commit();


            var planeInstance = new OSPInstance(planeGroup);
            planeInstance.Commit();
            return planeInstance;
        }
    }
}
