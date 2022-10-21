using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Adder : Algorithm
    {
        public List<Signal> InputSignals { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            List<float> new_samples = new List<float>();

            int samples_count = InputSignals[0].Samples.Count,
                signals_count = InputSignals.Count;

            for (int i = 0; i < samples_count; i++)
            {
                float current_sample = 0;

                for (int j = 0; j < signals_count; j++)
                    current_sample += InputSignals[j].Samples[i];

                new_samples.Add(current_sample);
            }

            OutputSignal = new Signal(new_samples, true);
        }
    }
}