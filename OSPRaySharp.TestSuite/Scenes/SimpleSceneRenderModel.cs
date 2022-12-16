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

namespace OSPRay.TestSuite.Scenes
{
    internal class SimpleSceneRenderModel : RenderModel
    {
        internal override void Setup(RenderContext renderContext)
        {
            // triangle mesh data
            var vertices = new Vector3[] {
                new Vector3(-1f, -1.0f, -0.5f),
                new Vector3(-1f,  1.0f, -0.5f),
                new Vector3( 1f, -1.0f, -0.5f),
                new Vector3( 1f,  1.0f,  0.5f)
            };

            var colors = new Vector4[] {
                new Vector4(0.9f, 0.5f, 0.5f, 1.0f),
                new Vector4(0.8f, 0.8f, 0.8f, 1.0f),
                new Vector4(0.8f, 0.8f, 0.8f, 1.0f),
                new Vector4(0.5f, 0.9f, 0.5f, 1.0f)
            };

            // triangle faces
            var indices = new int[] { 0, 1, 2, 1, 2, 3 };

            using var mesh = new OSPMeshGeometry();
            mesh.SetVertexPositions(vertices);
            mesh.SetVertexColors(colors);
            mesh.SetIndices(indices);
            mesh.Commit();

            using var model = new OSPGeometricModel(mesh);
            model.SetMaterials(new OSPPrincipledMaterial());
            model.Commit();

            using var group = new OSPGroup();
            group.SetGeometry(model);
            group.Commit();

            using var instance = new OSPInstance(group);
            instance.Commit();


            using var light1 = new OSPSphereLight();
            light1.SetPosition(new Vector3(0f, 0f, -3f));
            light1.SetRadius(0.1f);
            light1.SetIntensity(2f);
            light1.Commit();

            var light2 = new OSPSphereLight();
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

        protected override void UpdateCore(RenderContext renderContext, int stateChanges)
        {
            // nop
        }
    }
}
