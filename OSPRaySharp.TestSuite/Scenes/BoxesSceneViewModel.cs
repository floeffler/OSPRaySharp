using OSPRay.TestSuite.Render;
using OSPRay.TestSuite.Scenes.RenderModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay.TestSuite.Scenes
{
    internal class BoxesSceneViewModel : SceneViewModel
    {
        private Boxes renderModel;

        public BoxesSceneViewModel() : base("Boxes")
        {
            renderModel = new Boxes();
        }

        public int Dimensions
        {
            get => renderModel.Dimensions;
            set
            {
                renderModel.Dimensions = value;
                NotifyPropertyChanged();
            }
        }

        public bool UseLight
        {
            get => renderModel.UseLight;
            set
            {
                renderModel.UseLight = value;
                NotifyPropertyChanged();
            }
        }


        public override RenderModel RenderModel => renderModel;
    }
}
