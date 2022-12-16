using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay.TestSuite.Interaction
{
    public interface ICameraPoseProvider
    {
        public Pose CameraPose
        {
            get;
            set;
        }

        public Vector3? GetSceneCoordinate(float x, float y);

        public void AnimateCameraTo(Pose pose, Action completed);
    }

    public class TransformInteractor : Interactor
    {
        private const int KEY_SHIFT = 0;
        private const int KEY_CTRL = 1;
        private const int KEY_ALT = 2;

        private const float ZoomSensitivity = 0.05f;
        private const float MoveSensitivity = 0.005f;

        private bool isPressed;
        private Vector2 position;
        private Vector3? centerOfInteraction;
        private Vector3? scenePosition;

        public TransformInteractor(ICameraPoseProvider cameraPoseProvider)
        {
            PoseProvider = cameraPoseProvider;
            Reset();
        }


        public ICameraPoseProvider PoseProvider
        {
            get;
        }

        public void InjectMouseEvent(MouseEvent mouseEvent)
        {
            BitVector32 keys = new BitVector32();
            keys[KEY_SHIFT] = mouseEvent.ShiftKey;
            keys[KEY_CTRL] = mouseEvent.CtrlKey;
            keys[KEY_ALT] = mouseEvent.AltKey;

            switch (mouseEvent.EventType)
            {
                case MouseEventType.DblClick:
                    OnMouseDblClick(mouseEvent.Button, mouseEvent.X, mouseEvent.Y);
                    break;
                case MouseEventType.Down:
                    OnMouseDown(mouseEvent.Button, mouseEvent.X, mouseEvent.Y);
                    break;
                case MouseEventType.Up:
                    OnMouseUp(mouseEvent.Button);
                    break;
                case MouseEventType.Move:
                    OnMouseMove(mouseEvent.X, mouseEvent.Y, keys);
                    break;
                case MouseEventType.Wheel:
                    OnMouseWheel(mouseEvent.Delta);
                    break;
            }
        }

        public void Refocus()
        {
            RefocusCore(null);
        }

        private void RefocusCore(float? previousLength)
        {
            var cameraPose = PoseProvider.CameraPose;
            var cameraFrame = cameraPose.ToFrame();

            var center = centerOfInteraction.GetValueOrDefault();
            var v = center - cameraPose.Position;
            var len = v.Length();
            if (len > 0f)
            {

                var pos = cameraPose.Position;
                var front = Vector3.Normalize(v);
                if (previousLength.HasValue && len > previousLength)
                    pos = center - front * previousLength.Value;

                var up = Math.Abs(Vector3.Dot(v, cameraFrame.Up)) < 0.998 ? cameraFrame.Up : cameraFrame.Right;

                cameraPose = new Pose(pos, pos + front, up);
                PoseProvider.AnimateCameraTo(cameraPose, () => centerOfInteraction = center);
            }
        }

        public void Reset()
        {
            centerOfInteraction = Vector3.Zero;
        }

        private void ApplyMouseDelta(float dx, float dy, BitVector32 keys)
        {
            if (keys.Data == 0)
            {
                ApplyMouseDeltaRotate(dx, -dy, keys);
            }
            else if (keys[KEY_SHIFT])
            {
                ApplyMouseDeltaPan(dx, dy, keys);
            }
            else if (keys[KEY_CTRL])
            {
                ApplyMouseDeltaZoom(dx, dy, keys);
            }
        }


        private void ApplyMouseDeltaRotate(float dx, float dy, BitVector32 keys)
        {
            // update camera pose 
            var cameraPose = PoseProvider.CameraPose;
            var cameraFrame = cameraPose.ToFrame();

            // check for valid center of rotion
            if (centerOfInteraction.HasValue)
            {
                var center = centerOfInteraction.Value;
                var axis = Vector3.Normalize(cameraPose.Position - center);
                if (Vector3.Dot(axis, cameraFrame.Front) < 0f)
                {
                    centerOfInteraction = null; // clear center of rotation
                }
            }

            // orbit rotation
            if (centerOfInteraction.HasValue)
            {

                var qx = Quaternion.CreateFromAxisAngle(cameraFrame.Up, dx * MoveSensitivity);
                var qy = Quaternion.CreateFromAxisAngle(cameraFrame.Right, dy * MoveSensitivity);
                var q = qy * qx;

                var v = cameraPose.Position - centerOfInteraction.Value;
                var l = v.Length();
                v /= l;

                v = Vector3.Transform(v, q);
                v = Vector3.Normalize(v);
                var p = centerOfInteraction.Value + v * l;

                cameraPose = new Pose(p, p - v, cameraFrame.Up);
            }
            else
            {
                // first person mode
                var qx = Quaternion.CreateFromAxisAngle(cameraFrame.Up, dx * MoveSensitivity);
                var qy = Quaternion.CreateFromAxisAngle(cameraFrame.Right, dy * MoveSensitivity);
                var qr = qx * qy;

                cameraPose.Rotation = qr * cameraPose.Rotation;
            }

            PoseProvider.CameraPose = cameraPose;
        }

        private void ApplyMouseDeltaPan(float dx, float dy, BitVector32 keys)
        {
            var cameraPose = PoseProvider.CameraPose;
          
            if (scenePosition.HasValue)
            {

                var depth = Vector3.Distance(scenePosition.Value, cameraPose.Position);
                //var pos1 = PoseProvider.GetSceneCoordinate(position.X, position.Y, depth);
                //var pos2 = PoseProvider.GetSceneCoordinate(position.X + dx, position.Y + dy, depth);

                // compute diff
                var diff = new Vector3(dx * 0.1f, dy * 0.1f, 0f);
                cameraPose.Position += diff;
                if (centerOfInteraction.HasValue)
                    centerOfInteraction = centerOfInteraction.Value + diff;

                PoseProvider.CameraPose = cameraPose;
            }
        }

        private void ApplyMouseDeltaZoom(float dx, float dy, BitVector32 keys)
        {
            // same code like mouse wheel
            OnMouseWheel(dy);
        }

        private void OnMouseDblClick(int button, float x, float y)
        {
            var pos = PoseProvider.GetSceneCoordinate(x, y);
            if (pos.HasValue)
            {
                float? length = null;
                if (centerOfInteraction.HasValue)
                {
                    length = Vector3.Distance(centerOfInteraction.Value, PoseProvider.CameraPose.Position);
                }

                centerOfInteraction = pos;
                RefocusCore(length);
            }
        }

        private void OnMouseMove(float x, float y, BitVector32 keys)
        {
            if (!isPressed)
                return;


            float dx = position.X - x;
            float dy = position.Y - y;

            // apply mouse delta
            ApplyMouseDelta(dx, dy, keys);

            position.X = x;
            position.Y = y;
        }

        private void OnMouseDown(int button, float x, float y)
        {
            isPressed = true;
            position = new Vector2(x, y);
            scenePosition = PoseProvider.GetSceneCoordinate(x, y);
        }

        private void OnMouseUp(int button)
        {
            isPressed = false;
        }

        private void OnMouseWheel(float delta)
        {
            // update camera pose 
            var cameraPose = PoseProvider.CameraPose;
            var cameraFrame = cameraPose.ToFrame();

            float scale = 1f;
            if (centerOfInteraction.HasValue)
            {
                var center = centerOfInteraction.Value;
                var dist = Vector3.Distance(center, cameraPose.Position);
                scale = (float)Math.Sqrt(dist) / 5f + 0.01f;
            }

            if (delta < 0f)
                cameraPose.Position += (cameraFrame.Front * ZoomSensitivity * scale);
            else
                cameraPose.Position -= (cameraFrame.Front * ZoomSensitivity * scale);

            PoseProvider.CameraPose = cameraPose;
        }
    }
}
