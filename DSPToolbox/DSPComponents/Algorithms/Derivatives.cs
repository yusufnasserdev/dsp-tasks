using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class Derivatives: Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal FirstDerivative { get; set; }
        public Signal SecondDerivative { get; set; }

        public override void Run()
        {
            List<float> firstDerivtaveSamples = new List<float>();
            List<float> secondDerivtaveSamples = new List<float>();

            for (int i = 1; i < InputSignal.Samples.Count; i++)
            {
                firstDerivtaveSamples.Add(InputSignal.Samples[i] - InputSignal.Samples[i - 1]);

                if (i + 1 == InputSignal.Samples.Count) continue;
                
                secondDerivtaveSamples.Add(InputSignal.Samples[i + 1] - (2 * InputSignal.Samples[i]) + InputSignal.Samples[i - 1]);
                
            }

            FirstDerivative = new Signal(firstDerivtaveSamples, false);
            SecondDerivative = new Signal(secondDerivtaveSamples, false);

        }
    }
}
