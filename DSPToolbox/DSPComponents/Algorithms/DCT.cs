using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class DCT : Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            int N = InputSignal.Samples.Count;
            double sqrt2n = Math.Sqrt(2.0 / N);
            List<float> samples = new List<float>();
            for (int k = 0; k < N; k++)
            {
                double sum = 0;
                for (int n = 0; n < N; n++)
                    sum += InputSignal.Samples[n] * Math.Cos((Math.PI / (4 * N)) * (2 * n - 1) * (2 * k - 1));

                samples.Add((float)sqrt2n * (float)sum);
            }

            OutputSignal = new Signal(samples, false);

        }
    }
}
