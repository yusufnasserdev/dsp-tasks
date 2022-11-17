using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;


namespace DSPAlgorithms.Algorithms
{
    public class AccumulationSum : Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            List<float> outputSamples = new List<float>();
            float samplesSum = 0;

            foreach (var sample in InputSignal.Samples)
            {
                samplesSum += sample;
                outputSamples.Add(samplesSum);
            }

            OutputSignal = new Signal(outputSamples, false);

        }
    }
}
