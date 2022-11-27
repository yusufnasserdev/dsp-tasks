using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class DC_Component : Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {

            float average = InputSignal.Samples.Average();
            List<float> outputSamples = new List<float>();
            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                if (average > 0)
                    outputSamples.Add(InputSignal.Samples[i] - average);
                else
                    outputSamples.Add(InputSignal.Samples[i] + average);
            }

            OutputSignal = new Signal(outputSamples, false);

        }
    }
}
