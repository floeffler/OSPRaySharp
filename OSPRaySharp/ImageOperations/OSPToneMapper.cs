using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay.ImageOperations
{
    public class OSPToneMapper : OSPImageOperation
    {
        public OSPToneMapper(): base("tonemapper")
        {
        }
    }
}
