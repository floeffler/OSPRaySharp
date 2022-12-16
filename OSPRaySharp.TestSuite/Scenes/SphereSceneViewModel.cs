using OSPRay.TestSuite.Render;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay.TestSuite.Scenes
{
    internal class SphereSceneViewModel : SceneViewModel
    {
        private SphereSceneRenderModel renderModel;

        public SphereSceneViewModel() : base("Sphere Scene")
        {
            renderModel = new SphereSceneRenderModel();
        }

        public int SphereCount
        {
            get => renderModel.SphereCount;
            set
            {
                renderModel.SphereCount = value;
                NotifyPropertyChanged();
            }
        }

        public override RenderModel RenderModel => renderModel;
    }
}
