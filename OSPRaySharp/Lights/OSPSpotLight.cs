using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay.Lights
{
    /// <summary>
    /// The spotlight is a light emitting into a cone of directions.
    /// </summary>
    public class OSPSpotLight : OSPLight
    {
        public OSPSpotLight() : base("spot")
        {
        }

        public void SetPosition(Vector3 position) => SetParam("position", position);
        public void SetDirection(Vector3 direction) => SetParam("direction", direction);
        public void SetOpeningAngle(float openingAngle) => SetParam("openingAngle", openingAngle);
        public void SetPenumbraAngle(float penumbraAngle) => SetParam("penumbraAngle", penumbraAngle);
        public void SetRadius(float radius) => SetParam("radius", radius);
        public void SetInnerRadius(float innerRadius) => SetParam("innerRadius", innerRadius);
    }
}
