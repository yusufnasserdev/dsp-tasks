using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Shifter : Algorithm
    {
        public Signal InputSignal { get; set; }
        public bool Folded { get; set; }
        public int ShiftingValue { get; set; }
        public Signal OutputShiftedSignal { get; set; }

        public override void Run()
        {

            if (Folded) ShiftingValue *= -1;

            List<int> outputSamplesIndices = new List<int>();
            for (int i = 0; i < InputSignal.SamplesIndices.Count; i++)
                outputSamplesIndices.Add(InputSignal.SamplesIndices[i] - ShiftingValue);

            OutputShiftedSignal = new Signal(InputSignal.Samples, outputSamplesIndices, false);
        }
    }
}
