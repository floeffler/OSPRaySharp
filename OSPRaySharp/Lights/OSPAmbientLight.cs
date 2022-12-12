using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay.Lights
{
    /// <summary>
    /// The ambient light surrounds the scene and illuminates it from infinity with 
    /// constant radiance (determined by combining the parameters color and intensity).
    /// </summary>
    public class OSPAmbientLight : OSPLight
    {
        public OSPAmbientLight() : base("ambient")
        {
        }
    }
}
