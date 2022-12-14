using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay.Volumes
{
    /// <summary>
    /// Regular grid
    /// </summary>
    public class OSPStructuredRegularVolume : OSPStructuredVolume
    {
        public OSPStructuredRegularVolume(): base("structuredRegular")
        {
        }
    }
}
