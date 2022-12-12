using OSPRay;
using OSPRay.Cameras;
using OSPRay.Renderers;
using System.Numerics;

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

                    using (var imageOp = new OSPImageOperation("tonemapper"))
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
    }
}