using OSPRay;
using OSPRay.Cameras;
using OSPRay.Geometries;
using OSPRay.ImageOperations;
using OSPRay.Lights;
using OSPRay.Renderers;
using System;
using System.Drawing;
using System.Numerics;
using System.Runtime.InteropServices;

namespace OSPRaySharp.Tests
{
    [TestClass]
    public class BasicTests
    {
        public TestContext? TestContext { get; set; }

        [TestMethod]
        public void Initialize()
        {
            using (var ospray = new OSPLibrary())
            {
                Assert.IsNotNull(ospray);
                Assert.IsNotNull(ospray.Device);
            }
        }

        [TestMethod]
        public void CreateDevice()
        {
            using (var ospray = new OSPLibrary())
            {
                using (var device = ospray.CreateCPUDevice(debugMode: true))
                {
                    Assert.IsNotNull(device);
                    TestContext?.WriteLine(device.Version.ToString());

                    Assert.IsFalse(device.IsCurrent);
                    device.SetCurrent();
                    Assert.IsTrue(device.IsCurrent);
                }
            }
        }

        [TestMethod]
        public void DataTest()
        {
            using (var ospray = new OSPLibrary())
            {
                int[] array = Enumerable.Range(1, 10).ToArray();
                int[] other = new int[array.Length];

                using (var opaqueData = OSPDataFactory.CreateData1D(array))
                {
                    Assert.IsNotNull(opaqueData);
                    Assert.AreEqual(opaqueData.NumItems1, array.Length);
                    Assert.AreEqual(opaqueData.NumItems2, 1);
                    Assert.AreEqual(opaqueData.NumItems3, 1);
                    Assert.IsFalse(opaqueData.IsShared);
                }

                using (var arrayData = OSPDataFactory.CreateSharedData1D(array))
                {
                    Assert.IsNotNull(arrayData);
                    Assert.AreEqual(arrayData.NumItems1, array.Length);
                    Assert.AreEqual(arrayData.NumItems2, 1);
                    Assert.AreEqual(arrayData.NumItems3, 1);
                    Assert.IsTrue(arrayData.IsShared);

                    using (var otherData = OSPDataFactory.CreateSharedData1D(other))
                    {
                        Assert.IsNotNull(otherData);
                        arrayData.CopyTo(otherData);

                        for (int i = 0; i < array.Length; i++)
                        {
                            Assert.AreEqual(array[i], other[i]);
                        }
                    }
                }
            }
        }

        [TestMethod]
        public void RendererTest()
        {
            using (var ospray = new OSPLibrary())
            {
                using (var renderer = new OSPPathTracer())
                {
                    renderer.SetPixelFilter(OSPPixelFilter.Mitchell);
                    renderer.Commit();
                }
            }
        }

        
        [TestMethod]
        public void FrameBufferTest()
        {
            using (var ospray = new OSPLibrary())
            {
                using (var frameBuffer = new OSPFrameBuffer(1920, 1080, OSPFrameBufferFormat.SRGBA, OSPFrameBufferChannel.Color | OSPFrameBufferChannel.Depth | OSPFrameBufferChannel.Variance))
                {

                    frameBuffer.ResetAccumulation();
                    using (var mapped = frameBuffer.Map())
                    {
                        Assert.AreEqual(mapped.SizeInBytes, 4 * frameBuffer.Width * frameBuffer.Height);
                        var span = mapped.Span;
                        for (int i = 0; i < span.Length; i++)
                        {
                            Assert.AreEqual(span[i], 0);
                        }
                    }

                    float variance = frameBuffer.GetVariance();
                    Assert.AreEqual(variance, 0);

                    Assert.ThrowsException<ArgumentException>(() => frameBuffer.Map(OSPFrameBufferChannel.Albedo));
                    Assert.ThrowsException<InvalidOperationException>(() => frameBuffer.Map(OSPFrameBufferChannel.Variance));

                    using (var imageOp = new OSPToneMapper())
                    {
                        frameBuffer.SetImageOperations(new OSPImageOperation[] { imageOp });
                        frameBuffer.Commit();
                    }
                }
            }
        }

        [TestMethod]
        public void CameraTest()
        {
            using (var ospray = new OSPLibrary())
            {
                using (var camera = new OSPPerspectiveCamera())
                {
                    camera.SetFovY(90f);
                    camera.Commit();
                }
            }
        }

        [TestMethod]
        public void RenderingTest()
        {
            using (var ospray = new OSPLibrary())
            {
                using (var camera = new OSPPerspectiveCamera())
                {
                    using (var world = new OSPWorld())
                    {
                        using (var frameBuffer = new OSPFrameBuffer(200, 200, OSPFrameBufferFormat.RGBA32F))
                        {
                            using (var renderer = new OSPSciVisRenderer())
                            {
                                renderer.SetBackgroundColor(new Vector4(0.5f, 0.5f, 0.5f, 1f));
                                renderer.Commit();

                                var future = renderer.RenderFrame(frameBuffer, camera, world);
                                future.Wait();

                                Assert.IsTrue(future.IsReady());
                                Assert.IsTrue(future.Progress == 1.0);
                                Assert.IsTrue(future.Duration > 0.0);

                                using (var mapped = frameBuffer.Map())
                                {
                                    var span = mapped.GetSpan<float>();
                                    for (int i = 0; i < span.Length; i++)
                                    {
                                        float expected = (i % 4 == 3) ? 1f : 0.5f;
                                        float difference = Math.Abs(span[i] - expected);
                                        Assert.IsTrue(difference < 1e-6f);
                                    }
                                }


                                future.Dispose();
                            }
                        }
                    }
                }
            }
        }

        [TestMethod]
        public void TestHelloWorld()
        {
            const int ImageSizeX = 1024;
            const int ImageSizeY = 768;

            var camPos  = new Vector3(0.0f, 0.0f, -0.5f);
            var camUp   = new Vector3(0.0f, 1.0f, 0.0f);
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
            var indices = new int[] {0, 1, 2, 1, 2, 3 };


            using (var ospray = new OSPLibrary())
            {
                var camera = new OSPPerspectiveCamera();
                camera.SetAspect(ImageSizeX / (float)ImageSizeY);
                camera.SetPosition(camPos);
                camera.SetUp(camUp);
                camera.SetDirection(camView);
                camera.Commit();

                var mesh = new OSPMeshGeometry();
                mesh.SetVertexPositions(vertices);
                mesh.SetVertexColors(colors);
                mesh.SetIndices(indices);
                mesh.Commit();

                var model = new OSPGeometricModel(mesh);
                model.Commit();

                using var group = new OSPGroup();
                group.SetGeometry(model);
                group.Commit();

                using var instance = new OSPInstance(group);
                instance.Commit();


                using var light = new OSPAmbientLight();
                light.Commit();


                using var world = new OSPWorld();
                world.SetInstances(instance);
                world.SetLights(light);
                world.Commit();

                using var renderer = new OSPSciVisRenderer();
                renderer.SetPixelFilter(OSPPixelFilter.Mitchell);
                renderer.SetBackgroundColor(1f);
                renderer.SetAOSamples(1);
                renderer.SetSamplesPerPixel(4);
                renderer.Commit();

                // using var tonemapper = new OSPToneMapper();
                using var frameBuffer = new OSPFrameBuffer(ImageSizeX, ImageSizeY, OSPFrameBufferFormat.SRGBA);
                // frameBuffer.SetImageOperations(tonemapper);
                frameBuffer.Commit();
                
                renderer.ospRenderFrameBlocking(frameBuffer, camera, world);
                ExportFrameBuffer("firstFrame.png", frameBuffer);

                for (int i = 0; i < 10; ++i)
                {
                    renderer.ospRenderFrameBlocking(frameBuffer, camera, world);
                }

                ExportFrameBuffer("finalFrame.png", frameBuffer);
            }
        }

        [TestMethod]
        public void TestAffineSpace()
        {
            var affineSpace = new AffineSpace3F(Matrix4x4.CreateTranslation(1, 2, 3));
            Assert.AreEqual(affineSpace.P.X, 1);
            Assert.AreEqual(affineSpace.P.Y, 2);
            Assert.AreEqual(affineSpace.P.Z, 3);


            var matrix = Matrix4x4.CreateLookAt(Vector3.One, Vector3.Zero, Vector3.UnitY);
            affineSpace = new AffineSpace3F(matrix);

            var v = new Vector3(0.5f, -2f, 3f);

            var r1 = Vector3.Transform(v, matrix);
            var r2 = affineSpace * v;

            Assert.AreEqual(r1, r2);
        }


        private static void ExportFrameBuffer(string filename, OSPFrameBuffer frameBuffer)
        {
            using (var mappedColorBuffer = frameBuffer.Map(OSPFrameBufferChannel.Color))
            {
                using (var bitmap = new Bitmap(frameBuffer.Width, frameBuffer.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
                {
                    var bitmapData = bitmap.LockBits(
                        new Rectangle(0, 0, frameBuffer.Width, frameBuffer.Height),
                        System.Drawing.Imaging.ImageLockMode.ReadWrite,
                        System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                    for (int i = 0; i < frameBuffer.Height; i++)
                    {

                        var row = mappedColorBuffer.GetSpan<byte>().Slice(i * frameBuffer.Width * 4, frameBuffer.Width * 4).ToArray();
                        // swap rgb to bgr
                        for (int j = 0; j < row.Length; j += 4)
                        {
                            var tmp  = row[j];
                            row[j]   = row[j + 2];
                            row[j+2] = tmp;
                        }

                        var dstPtr = bitmapData.Scan0 + (bitmapData.Stride * i);
                        Marshal.Copy(row, 0, dstPtr, row.Length);
                    }

                    bitmap.UnlockBits(bitmapData);
                    bitmap.Save(filename);
                }
            }
        }
    }
}