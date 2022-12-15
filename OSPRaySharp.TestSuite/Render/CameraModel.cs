using OSPRay.Cameras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay.TestSuite.Render
{
    internal class CameraModel : Model
    {
        private const int POSE_BIT = 1 << 0;
        private const int THINLENS_BIT = 1 << 1;
        private const int FOV_BIT = 1 << 2;


        private Pose pose = Pose.Identity;
        private double focalDistance = 0;
        private double lensRadius = 0;
        private double fovInDegree = 60;

        public CameraModel()
        {
        }

        public Pose Pose
        {
            get => pose;
            set
            {
                if (pose != value)
                {
                    pose = value;
                    NotifyChanged(POSE_BIT);
                }
            }
        }

        public double FocalDistance
        {
            get => focalDistance;
            set
            {
                if (focalDistance != value)
                {
                    focalDistance = value;
                    NotifyChanged(THINLENS_BIT);
                }
            }
        }

        public double LensRadius
        {
            get => lensRadius;
            set
            {
                if (lensRadius != value)
                {
                    lensRadius = value;
                    NotifyChanged(THINLENS_BIT);
                }
            }
        }

        public double FovInDegree
        {
            get => fovInDegree;
            set
            {
                if (fovInDegree != value)
                {
                    fovInDegree = value;
                    NotifyChanged(FOV_BIT);
                }
            }
        }

        protected override void UpdateCore(RenderContext renderContext, int stateChanges)
        {
            if ((stateChanges & POSE_BIT) == POSE_BIT)
            {
                if (renderContext.Camera != null)
                {
                    var matrix = pose.ToMatrix4x4();
                    renderContext.Camera.SetTransform(new AffineSpace3F(matrix));

                }
            }

            if ((stateChanges & THINLENS_BIT) == THINLENS_BIT)
            {
                if (renderContext.Camera is OSPPerspectiveCamera perspectiveCamera)
                {
                    perspectiveCamera.SetApertureRadius((float)LensRadius);
                    perspectiveCamera.SetFocusDistance((float)FocalDistance);
                }
            }

            if ((stateChanges & FOV_BIT) == FOV_BIT)
            {
                if (renderContext.Camera is OSPPerspectiveCamera perspectiveCamera)
                {
                    perspectiveCamera.SetFovY((float)FovInDegree);
                }
            }

            // commit changes
            renderContext.Camera?.Commit();
        }
    }
}
