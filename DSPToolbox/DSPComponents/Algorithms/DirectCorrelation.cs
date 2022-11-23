using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DirectCorrelation : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public List<float> OutputNonNormalizedCorrelation { get; set; }
        public List<float> OutputNormalizedCorrelation { get; set; }

        public override void Run()
        {
            OutputNonNormalizedCorrelation = new List<float>();
            OutputNormalizedCorrelation = new List<float>();

            // Given the testcases, a null InputSignal2 indicates that it's an auto-correlation case.
            if (InputSignal2 == null)
            {

                double sum = 0;
                for (int i = 0; i < InputSignal1.Samples.Count; i++)
                    sum += (InputSignal1.Samples[i] * InputSignal1.Samples[i]);

                // Since multiplying the same sum and then taking the square root is redundant, we'll divide it directly.
                double normalization_sum =(sum / InputSignal1.Samples.Count);
                
                List<float> corr_output = new List<float>();
                List<float> samples_duplicate = InputSignal1.Samples.ToList();


                for (int i = 0; i < InputSignal1.Samples.Count; i++)
                {
                    double iter_sum = 0;

                    for (int j = 0; j < InputSignal1.Samples.Count; j++)
                        iter_sum += InputSignal1.Samples[j] * samples_duplicate[(j + i) % samples_duplicate.Count];

                    if (!InputSignal1.Periodic)
                        samples_duplicate[i] = 0;

                    corr_output.Add((float)(iter_sum / InputSignal1.Samples.Count));
                }

                OutputNonNormalizedCorrelation = corr_output;

                for (int i = 0; i < OutputNonNormalizedCorrelation.Count; i++)
                    OutputNormalizedCorrelation.Add((float)(corr_output[i] / normalization_sum));
            }
            else
            {

                int new_len = InputSignal1.Samples.Count + InputSignal2.Samples.Count - 1;
                for (int i = InputSignal1.Samples.Count; i < new_len; i++) InputSignal1.Samples.Add(0);
                for (int i = InputSignal2.Samples.Count; i < new_len; i++) InputSignal2.Samples.Add(0);

                float sum1 = 0, sum2 = 0;
                for (int i = 0; i < new_len; i++)
                {
                    sum1 += (InputSignal1.Samples[i] * InputSignal1.Samples[i]);
                    sum2 += (InputSignal2.Samples[i] * InputSignal2.Samples[i]);
                }

                float normalization_sum = (float) Math.Sqrt(sum1 * sum2) / new_len;
                List<float> corr_output = new List<float>();

                for (int i = 0; i < new_len; i++)
                {
                    float iter_sum = 0;

                    for (int j = 0; j < new_len; j++)
                        iter_sum += InputSignal1.Samples[j] * InputSignal2.Samples[(j + i) % new_len];

                    if (!InputSignal1.Periodic)
                        InputSignal2.Samples[i] = 0;

                    corr_output.Add(iter_sum / new_len);
                }

                OutputNonNormalizedCorrelation = corr_output;

                for (int i = 0; i < OutputNonNormalizedCorrelation.Count; i++)
                    OutputNormalizedCorrelation.Add(corr_output[i] / normalization_sum);

            }
        }
    }
}