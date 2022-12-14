using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay.Textures
{
    /// <summary>
    /// The Texture2D texture type implements an image-based texture
    /// </summary>
    public class OSPTexture2D : OSPTexture
    {
        public OSPTexture2D(): base("texture2d")
        {
        }

        public void SetFormat(OSPTextureFormat format) => SetParam("format", format);
        public void SetFilter(OSPTextureFilter filter) => SetParam("filter", filter);
        public void SetData<T>(OSPData<T> data) where T : unmanaged => SetObjectParam("data", data);

    }
}
