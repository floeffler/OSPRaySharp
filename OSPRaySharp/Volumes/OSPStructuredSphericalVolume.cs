using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay.Volumes
{
    /// <summary>
    /// The grid dimensions and parameters are defined in terms of radial distance r, 
    /// inclination angle θ, and azimuthal angle ϕ, conforming with the ISO convention 
    /// for spherical coordinate systems.
    /// </summary>
    public class OSPStructuredSphericalVolume : OSPStructuredVolume
    {
        public OSPStructuredSphericalVolume(): base("structuredSpherical")
        {
        }
    }
}
