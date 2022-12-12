using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay
{
    public enum OSPSyncEvent
    {
        None = 0,
        WorldRendered = 10,
        WorldCommitted = 20,
        FrameFinished = 30,
        TaskFinished = 100000,
    }
}
