using OSPRay.TestSuite.Render;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay.TestSuite.Scenes
{
    internal class SimpleSceneViewModel : SceneViewModel
    {
        private SimpleSceneRenderModel renderModel;

        public SimpleSceneViewModel() : base("Simple Scene")
        {
            renderModel = new SimpleSceneRenderModel();
        }

        public override RenderModel RenderModel => renderModel;
    }
}
