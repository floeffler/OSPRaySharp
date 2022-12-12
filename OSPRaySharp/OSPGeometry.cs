using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay
{
    internal class OSPGeometryHandle : OSPObjectHandle
    {
        public new static readonly OSPGeometryHandle Empty = new OSPGeometryHandle();
    }

    public class OSPGeometry : OSPObject
    {
        private OSPGeometryHandle handle;

        public OSPGeometry(string type)
        {
            handle = NativeMethods.ospNewGeometry(type);
            if (handle.IsInvalid)
                OSPDevice.ThrowLastError();
        }

        internal override OSPObjectHandle Handle => handle;
    }
}
