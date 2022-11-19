using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Folder : Algorithm
    {
        public Signal InputSignal { get; set; }
        public bool Shifted { get; set; }
        public Signal OutputFoldedSignal { get; set; }

        public override void Run()
        {
            List<float> outputSamples = new List<float>();
            List<int> outputIndices = new List<int>();

            int IndexAdjuster = Shifted ? -1 : 1;

            for (int i = InputSignal.Samples.Count - 1; i > -1; i--)
                outputSamples.Add(InputSignal.Samples[i]);

            foreach (var index in InputSignal.SamplesIndices)
                outputIndices.Add(index * IndexAdjuster);

            outputIndices.Sort();

            OutputFoldedSignal = new Signal(outputSamples, outputIndices, false);
        }
    }
}
