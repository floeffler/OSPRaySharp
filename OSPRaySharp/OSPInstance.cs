using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay
{
    internal class OSPInstanceHandle : OSPObjectHandle
    {
    }

    /// <summary>
    /// Instances in OSPRay represent a single group’s placement into the world via a transform.
    /// </summary>
    public class OSPInstance : OSPObject
    {
        private OSPInstanceHandle handle;

        public OSPInstance(OSPGroup? group): base()
        {
            var groupHandle = group != null ? (OSPGroupHandle)group.Handle : OSPGroupHandle.Empty;
            handle = NativeMethods.ospNewInstance(groupHandle);
        }

        public void SetGroup(OSPGroup group) => SetObjectParam("group", group);
        public void SetId(uint id) => SetParam("id", id);
        public unsafe void SetTime(float startTime, float endTime) => SetParam("time", OSPDataType.OSP_BOX1F, startTime, endTime);


        internal override OSPObjectHandle Handle => handle;
    }
}
