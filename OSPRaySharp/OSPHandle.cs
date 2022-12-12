using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay
{
    internal abstract class OSPHandle : SafeHandle, IEquatable<OSPHandle>
    {
        public OSPHandle(): base(IntPtr.Zero, false) 
        { 
        }

        public bool Equals(OSPHandle? other)
        {
            if (other == null)
                return false;

            return other.handle == this.handle;
        }

        public override bool IsInvalid => handle == IntPtr.Zero;

        
    }
}
