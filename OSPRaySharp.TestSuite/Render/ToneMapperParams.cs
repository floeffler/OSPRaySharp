using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay.TestSuite.Render
{
    public struct ToneMapperParams
    {
        public float Exposure { get; set; }
        public float Contrast { get; set; }
        public float Shoulder { get; set; }
        public float MidIn { get; set; }
        public float MidOut { get; set; }
        public float HdrMax { get; set; }
        public bool AcesColor { get; set; }

        public static readonly ToneMapperParams Default = new ToneMapperParams()
        {
            Exposure = 1f,
            Contrast = 1.6773f,
            Shoulder = 0.9714f,
            MidIn = 0.18f,
            MidOut = 0.18f,
            HdrMax = 11.0785f,
            AcesColor = true,
        };
    }
}
