using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Normalizer : Algorithm
    {
        public Signal InputSignal { get; set; }
        public float InputMinRange { get; set; }
        public float InputMaxRange { get; set; }
        public Signal OutputNormalizedSignal { get; set; }

        private float getNormalizedSample(float sample)
        {
            return (((InputMaxRange - InputMinRange) * (sample - InputSignal.Samples.Min()))
                / (InputSignal.Samples.Max() - InputSignal.Samples.Min()))
                + InputMinRange;
        }

        public override void Run()
        {
            List<float> new_samples = new List<float>();

            int samples_count = InputSignal.Samples.Count;

            for (int i = 0; i < samples_count; i++)
                new_samples.Add(getNormalizedSample(InputSignal.Samples[i]));

            OutputNormalizedSignal = new Signal(new_samples, true);
        }
    }
}
