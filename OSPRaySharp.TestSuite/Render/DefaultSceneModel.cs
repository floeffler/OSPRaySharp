using OSPRay.Cameras;
using OSPRay.Renderers;
using OSPRay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using OSPRay.Geometries;
using OSPRay.Lights;

namespace OSPRay.TestSuite.Render
{
    internal class DefaultSceneModel : SceneModel
    {
        public DefaultSceneModel()
        {
        }

        internal override void Free(RenderContext renderContext)
        {
            renderContext.Renderer = null;
            renderContext.World = null;
        }

        internal override void Setup(RenderContext renderContext)
        {
            var camPos = new Vector3(0.0f, 0.0f, -0.5f);
            var camUp = new Vector3(0.0f, 1.0f, 0.0f);
            var camView = new Vector3(0.1f, 0.0f, 1.0f);

            // triangle mesh data
            var vertices = new Vector3[] {
                new Vector3(-1.0f, -1.0f, 3.0f),
                new Vector3(-1.0f,  1.0f, 3.0f),
                new Vector3( 1.0f, -1.0f, 3.0f),
                new Vector3( 0.1f,  0.1f, 0.3f)
            };

            var colors = new Vector4[] {
                new Vector4(0.9f, 0.5f, 0.5f, 1.0f),
                new Vector4(0.8f, 0.8f, 0.8f, 1.0f),
                new Vector4(0.8f, 0.8f, 0.8f, 1.0f),
                new Vector4(0.5f, 0.9f, 0.5f, 1.0f)
            };

            // triangle faces
            var indices = new int[] { 0, 1, 2, 1, 2, 3 };


            var camera = new OSPPerspectiveCamera();
            camera.SetPosition(camPos);
            camera.SetUp(camUp);
            camera.SetDirection(camView);
            camera.SetAspect(renderContext.AspectRatio);
            camera.Commit();
            renderContext.Camera = camera;

            var mesh = new OSPMeshGeometry();
            mesh.SetVertexPositions(vertices);
            mesh.SetVertexColors(colors);
            mesh.SetIndices(indices);
            mesh.Commit();

            var model = new OSPGeometricModel(mesh);
            model.Commit();

            var group = new OSPGroup();
            group.SetGeometry(model);
            group.Commit();

            using var instance = new OSPInstance(group);
            instance.Commit();


            var light = new OSPAmbientLight();
            light.Commit();


            var world = new OSPWorld();
            world.SetInstances(instance);
            world.SetLights(light);
            world.Commit();
            renderContext.World = world;

            var renderer = new OSPSciVisRenderer();
            renderer.SetPixelFilter(OSPPixelFilter.Mitchell);
            renderer.SetBackgroundColor(new Vector4(0f, 0f, 0f, 1f));
            renderer.SetAOSamples(1);
            renderer.SetSamplesPerPixel(4);
            renderer.Commit();
            renderContext.Renderer = renderer;
        }
    }
}
