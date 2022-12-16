using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay
{
    internal struct OSPPickResultNative
    {
        public int hasHit;
        public float worldPositionX;
        public float worldPositionY;
        public float worldPositionZ;
        public IntPtr instance;
        public IntPtr model;
        public uint primID;
    }

    public class OSPPickResult : IDisposable
    {
        internal OSPPickResult(OSPPickResultNative nativeResult)
        {
            HasHit = nativeResult.hasHit == 1;
            WorldPosition = new Vector3(nativeResult.worldPositionX, nativeResult.worldPositionY, nativeResult.worldPositionZ);

            if (nativeResult.instance != IntPtr.Zero)
            {
                Instance = new OSPInstance(new OSPInstanceHandle(nativeResult.instance));
            }
            if (nativeResult.model != IntPtr.Zero)
            {
                Model = new OSPGeometricModel(new OSPGeometricModelHandle(nativeResult.model));
            }
        }

        public bool HasHit { get; }

        public Vector3 WorldPosition { get; }

        public OSPInstance? Instance { get; }

        public OSPGeometricModel? Model { get; }

        public uint PimitiveId { get; }

        public void Dispose()
        {
            Instance?.Dispose();
            Model?.Dispose();
        }

    }
}
