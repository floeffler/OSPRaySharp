using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OSPRay.TestSuite.Render
{
    internal class SceneModel : Model
    {
        public SceneModel()
        {
            Camera = new CameraModel();
        }

        public CameraModel Camera { get; }

        internal override void Update(RenderContext renderContext)
        {
            Camera.Update(renderContext);
            base.Update(renderContext);
        }

        internal virtual void Setup(RenderContext renderContext)
        {
        }

        internal virtual void Free(RenderContext renderContext)
        {
        }


        protected override void UpdateCore(RenderContext renderContext, int stateChanges)
        {
            // nop
        }
    }
}
