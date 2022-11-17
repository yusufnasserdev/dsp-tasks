using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DirectConvolution : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public Signal OutputConvolvedSignal { get; set; }

        /// <summary>
        /// Convolved InputSignal1 (considered as X) with InputSignal2 (considered as H)
        /// </summary>
        public override void Run()
        {
            int newSamplesCount = InputSignal1.Samples.Count() + InputSignal2.Samples.Count() - 1;

           float[] outputSums = new float[newSamplesCount];

            for (int i = 0; i < InputSignal1.Samples.Count(); i++)
            {
                for (int j = 0; j < InputSignal2.Samples.Count(); j++)
                {
                    float sample1 = InputSignal1.Samples[i];
                    float sample2 = InputSignal2.Samples[j];
                    outputSums[i + j] += sample1 * sample2;
                }
            }

            int startIndex = Math.Min(InputSignal1.SamplesIndices[0], InputSignal2.SamplesIndices[0]);
            List<int> newIndices= new List<int>();

            for (int i = 0; i < newSamplesCount; i++)
                newIndices.Add(startIndex + i);



            OutputConvolvedSignal = new Signal(outputSums.ToList(), newIndices, false);
        }
    }
}
