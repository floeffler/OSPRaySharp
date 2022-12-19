using OSPRay.TestSuite.Render;
using OSPRay.TestSuite.Scenes.RenderModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay.TestSuite.Scenes
{
    internal class StreamlinesViewModel : SceneViewModel
    {
        private Streamlines renderModel;

        public StreamlinesViewModel() : base("Streamlines")
        {
            renderModel = new Streamlines();
        }

        public int NumLines
        {
            get => renderModel.NumLines;
            set
            {
                if (renderModel.NumLines != value)
                {
                    renderModel.NumLines = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool UseLight
        {
            get => renderModel.UseLight;
            set
            {
                if (renderModel.UseLight != value)
                {
                    renderModel.UseLight = value;
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
