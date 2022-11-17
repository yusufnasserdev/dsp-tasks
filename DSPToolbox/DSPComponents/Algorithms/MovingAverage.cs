using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class MovingAverage : Algorithm
    {
        public Signal InputSignal { get; set; }
        public int InputWindowSize { get; set; }
        public Signal OutputAverageSignal { get; set; }
 
        public override void Run()
        {
            List<float> outputSamples = new List<float>();

            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                if (i + InputWindowSize > InputSignal.Samples.Count)
                    break;

                float windowSum = 0;

                for (int j = 0; j < InputWindowSize; j++)
                        windowSum += InputSignal.Samples[i + j];

                outputSamples.Add(windowSum / InputWindowSize);
            }

            OutputAverageSignal = new Signal(outputSamples, false);
        }
    }
}
