using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay.Volumes
{
    public class OSPParticleVolume : OSPVolume
    {
        /// <summary>
        /// Particle volumes consist of a set of points in space. 
        /// Each point has a position, a radius, and a weight typically associated with an attribute.
        /// </summary>
        public OSPParticleVolume() : base("particle")
        {
        }
    }
}
