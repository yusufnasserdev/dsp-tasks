using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class MultiplySignalByConstant : Algorithm
    {
        public Signal InputSignal { get; set; }
        public float InputConstant { get; set; }
        public Signal OutputMultipliedSignal { get; set; }

        public override void Run()
        {
            List<float> new_samples = new List<float>();

            int samples_count = InputSignal.Samples.Count;

            for (int i = 0; i < samples_count; i++)
            {
                float current_sample = InputSignal.Samples[i] * InputConstant;
                new_samples.Add(current_sample);
            }

            OutputMultipliedSignal = new Signal(new_samples, true);
        }
    }
}
