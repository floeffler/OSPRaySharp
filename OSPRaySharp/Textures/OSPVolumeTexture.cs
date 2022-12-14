using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay.Textures
{
    /// <summary>
    /// The VolumeTexture type implements texture lookups based on 3D object coordinates of 
    /// the surface hit point on the associated geometry. If the given hit point is within 
    /// the attached volume, the volume is sampled and classified with the transfer function 
    /// attached to the volume.
    /// </summary>
    public class OSPVolumeTexture : OSPTexture
    {
        public OSPVolumeTexture(): base("volume")
        {
        }

    }
}
