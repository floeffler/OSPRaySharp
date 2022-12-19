using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPRay.TestSuite.Scenes.RenderModels
{
    internal class UniformDistribution
    {
        public UniformDistribution(double min, double max)
        {
            Min = min;
            Max = max; 
            Range = max - min;
        }

        private double Min { get; }
        private double Max { get; }
        private double Range { get; }

        public double Sample(Random random) => (double)(random.NextDouble() * Range + Min);
        public int SampleInt(Random random) => (int)Sample(random);
        public float SampleSingle(Random random) => (float)Sample(random);


    }
}
