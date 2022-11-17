using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Folder : Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputFoldedSignal { get; set; }

        public override void Run()
        {
            List<float> outputSamples = new List<float>();
            for (int i = InputSignal.Samples.Count - 1; i > -1; i--)
                outputSamples.Add(InputSignal.Samples[i]);

            OutputFoldedSignal = new Signal(outputSamples, InputSignal.SamplesIndices, false);
        }
    }
}
