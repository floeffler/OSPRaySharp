using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay
{
    internal class OSPCameraHandle : OSPObjectHandle
    {
    }

    public enum OSPShutterType
    {
        Global,
        RollingRight,
        RollingLeft,
        RollingDown,
        RollingUp,
    }

    public abstract class OSPCamera : OSPObject
    {
        private OSPCameraHandle handle;

        public OSPCamera(string type): base()
        {
            handle = NativeMethods.ospNewCamera(type);
            if (handle.IsInvalid)
                OSPDevice.ThrowLastError();
        }

        public void SetTransform(AffineSpace3F transform) => SetParam("transform", transform);
        public void SetPosition(Vector3 position) => SetParam("position", position);
        public void SetDirection(Vector3 direction) => SetParam("direction", direction);
        public void SetUp(Vector3 up) => SetParam("up", up);
        public void SetNearClip(float nearClip) => SetParam("nearClip", nearClip);
        public void SetImageRegion(Vector2 start, Vector2 end)
        {
            SetParam("imageStart", start);
            SetParam("imageEnd", end);
        }



        public void SetShutterType(OSPShutterType shutterType) => SetParam("shutterType", shutterType);
        public void SetShutter(float shutterOpen, float shutterClose) => SetParam("shutter", OSPDataType.Box1F, shutterOpen, shutterClose);
        public void SetRollingShutterDurationr(float rollingShutterDuration) => SetParam("rollingShutterDuration", rollingShutterDuration);

        public void SetMotionTransform(AffineSpace3F[] motion) => SetArrayParam("motion.transform", motion);

        internal override OSPObjectHandle Handle => handle;
    }
}
