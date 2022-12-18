using OSPRay.TestSuite.Render;
using OSPRay.TestSuite.Scenes.RenderModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay.TestSuite.Scenes
{
    internal class CornellBoxViewModel : SceneViewModel
    {
        private CornellBox renderModel;

        public CornellBoxViewModel() : base("Cornell Box")
        {
            renderModel = new CornellBox();
        }

        public override RenderModel RenderModel => renderModel;
    }
}
