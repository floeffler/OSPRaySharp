using Avalonia.Media;
using Avalonia.Rendering;
using OSPRay.Cameras;
using OSPRay.Renderers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay.TestSuite.Render
{
    public enum RendererType
    {
        SciVis,
        AO,
        PathTracer,
    }

    internal class RenderSettings : Model
    {
        private const int RENDERER_BIT = 1 << 0;
        private const int RENDERER_PARAM_BIT = 1 << 1;
        private const int CAMERA_PARAM_BIT = 1 << 2;
        private const int CAMERA_TRANSFORM_BIT = 1 << 3;

        private RendererType rendererType = RendererType.SciVis;
        private int samplesPerPixel = 1;
        private int aoSamples = 1;
        private OSPPixelFilter pixelFilter = OSPPixelFilter.Gaussian;

        private float fovInDegree = 60;
        private float focusDistance = 1f;
        private float apertureRadius = 0f;
        private Pose cameraPose = Pose.Identity;

        public RenderSettings()
        {
        }

        public RendererType RendererType
        {
            get => rendererType;
            set
            {
                if (rendererType != value)
                {
                    rendererType = value;
                    NotifyChanged(RENDERER_BIT);
                }
            }
        }

        public int SamplesPerPixel
        {
            get => samplesPerPixel;
            set
            {
                if (samplesPerPixel != value)
                {
                    samplesPerPixel = value;
                    NotifyChanged(RENDERER_PARAM_BIT);
                }
            }
        }

        public int AOSamples
        {
            get => aoSamples;
            set
            {
                if (aoSamples != value)
                {
                    aoSamples = value;
                    NotifyChanged(RENDERER_PARAM_BIT);
                }
            }
        }

        public OSPPixelFilter PixelFilter
        {
            get => pixelFilter;
            set
            {
                if (pixelFilter != value)
                {
                    pixelFilter = value;
                    NotifyChanged(RENDERER_PARAM_BIT);
                }
            }
        }

        public float FovInDegree
        {
            get => fovInDegree;
            set
            {
                if (fovInDegree != value)
                {
                    fovInDegree = value;
                    NotifyChanged(CAMERA_PARAM_BIT);
                }
            }
        }

        public float ApertureRadius
        {
            get => apertureRadius;
            set
            {
                if (apertureRadius != value)
                {
                    apertureRadius = value;
                    NotifyChanged(CAMERA_PARAM_BIT);
                }
            }
        }

        public float FocusDistance
        {
            get => focusDistance;
            set
            {
                if (focusDistance != value)
                {
                    focusDistance = value;
                    NotifyChanged(CAMERA_PARAM_BIT);
                }
            }
        }

        public Pose CameraPose
        {
            get => cameraPose;
            set
            {
                if (cameraPose != value)
                {
                    cameraPose = value;
                    NotifyChanged(CAMERA_TRANSFORM_BIT);
                }
            }
        }


        internal override void Setup(RenderContext renderContext)
        {
            renderContext.Camera = new OSPPerspectiveCamera();
            UpdateCore(renderContext, ALL_STATES_BIT);
        }

        internal override void Free(RenderContext renderContext)
        {
            renderContext.Camera = null;
            renderContext.Renderer = null;
        }

        protected override void UpdateCore(RenderContext renderContext, int stateChanges)
        {
            if ((stateChanges & RENDERER_BIT) == RENDERER_BIT)
            {
                OSPRenderer renderer;
                switch (rendererType)
                {
                    case RendererType.AO:
                        renderer = new OSPAmbientOcclusionRenderer();
                        break;
                    case RendererType.PathTracer:
                        renderer = new OSPAmbientOcclusionRenderer();
                        break;
                    default:
                        renderer = new OSPSciVisRenderer();
                        break;
                }

                renderContext.Renderer = renderer;
                stateChanges = stateChanges | RENDERER_PARAM_BIT; // update all params
            }

            if ((stateChanges & RENDERER_PARAM_BIT) == RENDERER_PARAM_BIT)
            {
                var renderer = renderContext.Renderer;
                if (renderer != null)
                {
                    renderer.SetBackgroundColor(new Vector4(0f, 0f, 0f, 1f));
                    renderer.SetSamplesPerPixel(samplesPerPixel);
                    renderer.SetPixelFilter(pixelFilter);
                    if (renderer is OSPSciVisRenderer scivis)
                        scivis.SetAOSamples(aoSamples);
                    else if (renderer is OSPAmbientOcclusionRenderer ao)
                        ao.SetAOSamples(aoSamples);
                    
                    renderer.Commit();
                }
            }

            if ((stateChanges & CAMERA_PARAM_BIT) == CAMERA_PARAM_BIT)
            {
                if (renderContext.Camera is OSPPerspectiveCamera camera)
                {
                    camera.SetApertureRadius(apertureRadius);
                    camera.SetFocusDistance(focusDistance);
                    camera.SetFovY(fovInDegree);
                    camera.Commit();
                }
            }

            if ((stateChanges & CAMERA_TRANSFORM_BIT) == CAMERA_TRANSFORM_BIT)
            {
                var camera = renderContext.Camera;
                if (camera != null)
                {
                    var frame = cameraPose.ToFrame();
                    camera.SetPosition(frame.Position);
                    camera.SetDirection(-frame.Front);
                    camera.SetUp(frame.Up);
                    camera.Commit();
                }
            }

            // changes always requires a redraw
            renderContext.ResetAccumulation();
        }
    }
}
