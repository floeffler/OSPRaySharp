using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay
{
    internal class OSPInstanceHandle : OSPObjectHandle
    {
        public OSPInstanceHandle() : base() { }
        public OSPInstanceHandle(IntPtr handle): base(handle) { }
    }

    /// <summary>
    /// Instances in OSPRay represent a single group’s placement into the world via a transform.
    /// </summary>
    public class OSPInstance : OSPObject
    {
        private OSPInstanceHandle handle;

        internal OSPInstance(OSPInstanceHandle handle) : base()
        {
            this.handle = handle;
        }

        public OSPInstance(OSPGroup? group): base()
        {
            var groupHandle = group != null ? (OSPGroupHandle)group.Handle : OSPGroupHandle.Empty;
            handle = NativeMethods.ospNewInstance(groupHandle);
        }

        public void SetGroup(OSPGroup group) => SetObjectParam("group", group);
        public void SetTransform(AffineSpace3F transform) => SetParam("transform", transform);
        public void SetMotionTransform(AffineSpace3F[] transform) => SetArrayParam("motion.transform", transform);
        public unsafe void SetTime(float startTime, float endTime) => SetParam("time", OSPDataType.Box1F, startTime, endTime);
        public void SetId(uint id) => SetParam("id", id);


        internal override OSPObjectHandle Handle => handle;
    }
}
