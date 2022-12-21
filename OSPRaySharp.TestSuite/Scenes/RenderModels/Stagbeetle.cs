using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay.TestSuite.Scenes.RenderModels
{
    internal class Stagbeetle : RenderModelBase
    {
        public Stagbeetle()
        {
        }

        public string VolumeFilePath
        {
            get
            {
                var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var dataPath = System.IO.Path.Combine(appDataPath, "OSPRaySharp");
                return System.IO.Path.Combine(dataPath, "stagbeetle832x832x494.dat");
            }
        }

        protected override OSPGroup BuildGroup()
        {
            return new OSPGroup();
        }
    }
}
