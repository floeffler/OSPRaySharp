using OSPRay.TestSuite.Render;
using OSPRay.TestSuite.Scenes.RenderModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay.TestSuite.Scenes
{
    internal class RandomSpheresViewModel : SceneViewModel
    {
        private RandomSpheres renderModel;

        public RandomSpheresViewModel() : base("Random Spheres")
        {
            renderModel = new RandomSpheres();
        }

        public int NumSpheres
        {
            get => renderModel.NumSpheres;
            set
            {
                if (renderModel.NumSpheres != value)
                {
                    renderModel.NumSpheres = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public int RandomSeed
        {
            get => renderModel.RandomSeed;
            set
            {
                if (renderModel.RandomSeed != value)
                {
                    renderModel.RandomSeed = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool ShowGroundPlane
        {
            get => renderModel.ShowGroundPlane;
            set
            {
                if (renderModel.ShowGroundPlane != value)
                {
                    renderModel.ShowGroundPlane = value;
                    NotifyPropertyChanged();
                }
            }
        }


        public override RenderModel RenderModel => renderModel;
    }
}
