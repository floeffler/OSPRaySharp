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

namespace OSPRay.TestSuite.Scenes.RenderModels
{
    internal class CornellBox : RenderModelBase
    {
        // quad mesh data
        private static readonly Vector3[] vertices = new Vector3[] {
            // Floor
            new Vector3(1.00f, -1.00f, -1.00f),
            new Vector3(-1.00f, -1.00f, -1.00f),
            new Vector3(-1.00f, -1.00f, 1.00f),
            new Vector3(1.00f, -1.00f, 1.00f),
            // Ceiling
            new Vector3(1.00f, 1.00f, -1.00f),
            new Vector3(1.00f, 1.00f, 1.00f),
            new Vector3(-1.00f, 1.00f, 1.00f),
            new Vector3(-1.00f, 1.00f, -1.00f),
            // Backwall
            new Vector3(1.00f, -1.00f, 1.00f),
            new Vector3(-1.00f, -1.00f, 1.00f),
            new Vector3(-1.00f, 1.00f, 1.00f),
            new Vector3(1.00f, 1.00f, 1.00f),
            // RightWall
            new Vector3(-1.00f, -1.00f, 1.00f),
            new Vector3(-1.00f, -1.00f, -1.00f),
            new Vector3(-1.00f, 1.00f, -1.00f),
            new Vector3(-1.00f, 1.00f, 1.00f),
            // LeftWall
            new Vector3(1.00f, -1.00f, -1.00f),
            new Vector3(1.00f, -1.00f, 1.00f),
            new Vector3(1.00f, 1.00f, 1.00f),
            new Vector3(1.00f, 1.00f, -1.00f),
            // ShortBox Top Face
            new Vector3(-0.53f, -0.40f, -0.75f),
            new Vector3(-0.70f, -0.40f, -0.17f),
            new Vector3(-0.13f, -0.40f, -0.00f),
            new Vector3(0.05f, -0.40f, -0.57f),
            // ShortBox Left Face
            new Vector3(0.05f, -1.00f, -0.57f),
            new Vector3(0.05f, -0.40f, -0.57f),
            new Vector3(-0.13f, -0.40f, -0.00f),
            new Vector3(-0.13f, -1.00f, -0.00f),
            // ShortBox Front Face
            new Vector3(-0.53f, -1.00f, -0.75f),
            new Vector3(-0.53f, -0.40f, -0.75f),
            new Vector3(0.05f, -0.40f, -0.57f),
            new Vector3(0.05f, -1.00f, -0.57f),
            // ShortBox Right Face
            new Vector3(-0.70f, -1.00f, -0.17f),
            new Vector3(-0.70f, -0.40f, -0.17f),
            new Vector3(-0.53f, -0.40f, -0.75f),
            new Vector3(-0.53f, -1.00f, -0.75f),
            // ShortBox Back Face
            new Vector3(-0.13f, -1.00f, -0.00f),
            new Vector3(-0.13f, -0.40f, -0.00f),
            new Vector3(-0.70f, -0.40f, -0.17f),
            new Vector3(-0.70f, -1.00f, -0.17f),
            // ShortBox Bottom Face
            new Vector3(-0.53f, -1.00f, -0.75f),
            new Vector3(-0.70f, -1.00f, -0.17f),
            new Vector3(-0.13f, -1.00f, -0.00f),
            new Vector3(0.05f, -1.00f, -0.57f),
            // TallBox Top Face
            new Vector3(0.53f, 0.20f, -0.09f),
            new Vector3(-0.04f, 0.20f, 0.09f),
            new Vector3(0.14f, 0.20f, 0.67f),
            new Vector3(0.71f, 0.20f, 0.49f),
            // TallBox Left Face
            new Vector3(0.53f, -1.00f, -0.09f),
            new Vector3(0.53f, 0.20f, -0.09f),
            new Vector3(0.71f, 0.20f, 0.49f),
            new Vector3(0.71f, -1.00f, 0.49f),
            // TallBox Front Face
            new Vector3(0.71f, -1.00f, 0.49f),
            new Vector3(0.71f, 0.20f, 0.49f),
            new Vector3(0.14f, 0.20f, 0.67f),
            new Vector3(0.14f, -1.00f, 0.67f),
            // TallBox Right Face
            new Vector3(0.14f, -1.00f, 0.67f),
            new Vector3(0.14f, 0.20f, 0.67f),
            new Vector3(-0.04f, 0.20f, 0.09f),
            new Vector3(-0.04f, -1.00f, 0.09f),
            // TallBox Back Face
            new Vector3(-0.04f, -1.00f, 0.09f),
            new Vector3(-0.04f, 0.20f, 0.09f),
            new Vector3(0.53f, 0.20f, -0.09f),
            new Vector3(0.53f, -1.00f, -0.09f),
            // TallBox Bottom Face
            new Vector3(0.53f, -1.00f, -0.09f),
            new Vector3(-0.04f, -1.00f, 0.09f),
            new Vector3(0.14f, -1.00f, 0.67f),
            new Vector3(0.71f, -1.00f, 0.49f)
        };

        private static readonly int[] indices = new int[] {
            0, 1, 2, 3, // Floor
            4, 5, 6, 7, // Ceiling
            8, 9, 10, 11, // Backwall
            12, 13, 14, 15, // RightWall
            16, 17, 18, 19, // LeftWall
            20, 21, 22, 23, // ShortBox Top Face
            24, 25, 26, 27, // ShortBox Left Face
            28, 29, 30, 31, // ShortBox Front Face
            32, 33, 34, 35, // ShortBox Right Face
            36, 37, 38, 39, // ShortBox Back Face
            40, 41, 42, 43, // ShortBox Bottom Face
            44, 45, 46, 47, // TallBox Top Face
            48, 49, 50, 51, // TallBox Left Face
            52, 53, 54, 55, // TallBox Front Face
            56, 57, 58, 59, // TallBox Right Face
            60, 61, 62, 63, // TallBox Back Face
            64, 65, 66, 67 // TallBox Bottom Face
        };

        private static readonly Vector4[] colors = new Vector4[] {
            // Floor
            new Vector4(0.725f, 0.710f, 0.68f, 1.0f),
            new Vector4(0.725f, 0.710f, 0.68f, 1.0f),
            new Vector4(0.725f, 0.710f, 0.68f, 1.0f),
            new Vector4(0.725f, 0.710f, 0.68f, 1.0f),
            // Ceiling
            new Vector4(0.725f, 0.710f, 0.68f, 1.0f),
            new Vector4(0.725f, 0.710f, 0.68f, 1.0f),
            new Vector4(0.725f, 0.710f, 0.68f, 1.0f),
            new Vector4(0.725f, 0.710f, 0.68f, 1.0f),
            // Backwall
            new Vector4(0.725f, 0.710f, 0.68f, 1.0f),
            new Vector4(0.725f, 0.710f, 0.68f, 1.0f),
            new Vector4(0.725f, 0.710f, 0.68f, 1.0f),
            new Vector4(0.725f, 0.710f, 0.68f, 1.0f),
            // RightWall
            new Vector4(0.140f, 0.450f, 0.091f, 1.0f),
            new Vector4(0.140f, 0.450f, 0.091f, 1.0f),
            new Vector4(0.140f, 0.450f, 0.091f, 1.0f),
            new Vector4(0.140f, 0.450f, 0.091f, 1.0f),
            // LeftWall
            new Vector4(0.630f, 0.065f, 0.05f, 1.0f),
            new Vector4(0.630f, 0.065f, 0.05f, 1.0f),
            new Vector4(0.630f, 0.065f, 0.05f, 1.0f),
            new Vector4(0.630f, 0.065f, 0.05f, 1.0f),
            // ShortBox Top Face
            new Vector4(0.725f, 0.710f, 0.68f, 1.0f),
            new Vector4(0.725f, 0.710f, 0.68f, 1.0f),
            new Vector4(0.725f, 0.710f, 0.68f, 1.0f),
            new Vector4(0.725f, 0.710f, 0.68f, 1.0f),
            // ShortBox Left Face
            new Vector4(0.725f, 0.710f, 0.68f, 1.0f),
            new Vector4(0.725f, 0.710f, 0.68f, 1.0f),
            new Vector4(0.725f, 0.710f, 0.68f, 1.0f),
            new Vector4(0.725f, 0.710f, 0.68f, 1.0f),
            // ShortBox Front Face
            new Vector4(0.725f, 0.710f, 0.68f, 1.0f),
            new Vector4(0.725f, 0.710f, 0.68f, 1.0f),
            new Vector4(0.725f, 0.710f, 0.68f, 1.0f),
            new Vector4(0.725f, 0.710f, 0.68f, 1.0f),
            // ShortBox Right Face
            new Vector4(0.725f, 0.710f, 0.68f, 1.0f),
            new Vector4(0.725f, 0.710f, 0.68f, 1.0f),
            new Vector4(0.725f, 0.710f, 0.68f, 1.0f),
            new Vector4(0.725f, 0.710f, 0.68f, 1.0f),
            // ShortBox Back Face
            new Vector4(0.725f, 0.710f, 0.68f, 1.0f),
            new Vector4(0.725f, 0.710f, 0.68f, 1.0f),
            new Vector4(0.725f, 0.710f, 0.68f, 1.0f),
            new Vector4(0.725f, 0.710f, 0.68f, 1.0f),
            // ShortBox Bottom Face
            new Vector4(0.725f, 0.710f, 0.68f, 1.0f),
            new Vector4(0.725f, 0.710f, 0.68f, 1.0f),
            new Vector4(0.725f, 0.710f, 0.68f, 1.0f),
            new Vector4(0.725f, 0.710f, 0.68f, 1.0f),
            // TallBox Top Face
            new Vector4(0.725f, 0.710f, 0.68f, 1.0f),
            new Vector4(0.725f, 0.710f, 0.68f, 1.0f),
            new Vector4(0.725f, 0.710f, 0.68f, 1.0f),
            new Vector4(0.725f, 0.710f, 0.68f, 1.0f),
            // TallBox Left Face
            new Vector4(0.725f, 0.710f, 0.68f, 1.0f),
            new Vector4(0.725f, 0.710f, 0.68f, 1.0f),
            new Vector4(0.725f, 0.710f, 0.68f, 1.0f),
            new Vector4(0.725f, 0.710f, 0.68f, 1.0f),
            // TallBox Front Face
            new Vector4(0.725f, 0.710f, 0.68f, 1.0f),
            new Vector4(0.725f, 0.710f, 0.68f, 1.0f),
            new Vector4(0.725f, 0.710f, 0.68f, 1.0f),
            new Vector4(0.725f, 0.710f, 0.68f, 1.0f),
            // TallBox Right Face
            new Vector4(0.725f, 0.710f, 0.68f, 1.0f),
            new Vector4(0.725f, 0.710f, 0.68f, 1.0f),
            new Vector4(0.725f, 0.710f, 0.68f, 1.0f),
            new Vector4(0.725f, 0.710f, 0.68f, 1.0f),
            // TallBox Back Face
            new Vector4(0.725f, 0.710f, 0.68f, 1.0f),
            new Vector4(0.725f, 0.710f, 0.68f, 1.0f),
            new Vector4(0.725f, 0.710f, 0.68f, 1.0f),
            new Vector4(0.725f, 0.710f, 0.68f, 1.0f),
            // TallBox Bottom Face
            new Vector4(0.725f, 0.710f, 0.68f, 1.0f),
            new Vector4(0.725f, 0.710f, 0.68f, 1.0f),
            new Vector4(0.725f, 0.710f, 0.68f, 1.0f),
            new Vector4(0.725f, 0.710f, 0.68f, 1.0f)
        };

        protected override OSPGroup BuildGroup()
        {
            using var quadMesh = new OSPMeshGeometry();

            quadMesh.SetVertexPositions(vertices);
            quadMesh.SetVertexColors(colors);
            quadMesh.SetIndices(indices, true);
            quadMesh.Commit();

            using var model = new OSPGeometricModel(quadMesh);
            using (var material = new OSPObjMaterial())
            {
                material.Commit();
                model.SetMaterials(material);
            }
            model.Commit();


            var group = new OSPGroup();
            group.SetGeometry(model);
            group.Commit();

            return group;
        }

        protected override OSPWorld BuildWorld()
        {
            var world = base.BuildWorld();

            var light = new OSPQuadLight();
            light.SetColor(new Vector3(0.78f, 0.551f, 0.183f));
            light.SetIntensity(47f);
            light.SetPosition(new Vector3(-0.23f, 0.98f, -0.16f));
            light.SetEdge1(new Vector3(0.47f, 0.0f, 0.0f));
            light.SetEdge2(new Vector3(0.0f, 0.0f, 0.38f));
            light.Commit();

            world.SetLights(light);
            return world;
        }
    }
}
