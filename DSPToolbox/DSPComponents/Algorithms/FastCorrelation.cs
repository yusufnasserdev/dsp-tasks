using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class FastCorrelation : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public List<float> OutputNonNormalizedCorrelation { get; set; }
        public List<float> OutputNormalizedCorrelation { get; set; }

        public override void Run()
        {
            int new_len = InputSignal1.Samples.Count;
            double normalization_sum = 0;
         

            if (InputSignal2 == null)
            {
                InputSignal2 = new Signal(InputSignal1.Samples.ToList(), false);

                double sum = 0;
                for (int i = 0; i < InputSignal1.Samples.Count; i++)
                    sum += (InputSignal1.Samples[i] * InputSignal1.Samples[i]);

                normalization_sum = sum / new_len;

            }
            else
            {
                float sum1 = 0, sum2 = 0;
                for (int i = 0; i < new_len; i++)
                {
                    sum1 += (InputSignal1.Samples[i] * InputSignal1.Samples[i]);
                    sum2 += (InputSignal2.Samples[i] * InputSignal2.Samples[i]);
                }

                normalization_sum = (float)Math.Sqrt(sum1 * sum2) / new_len;
            }

            DiscreteFourierTransform dft1 = new DiscreteFourierTransform();
            dft1.InputTimeDomainSignal = InputSignal1;
            dft1.Run();

            DiscreteFourierTransform dft2 = new DiscreteFourierTransform();
            dft2.InputTimeDomainSignal = InputSignal2;
            dft2.Run();


            List<Complex> NewHarmonics = new List<Complex>();

            for (int i = 0; i < dft1.ImaginaryParts.Count; i++)
            {
                double img = dft1.Harmonics[i].Imaginary * -1;
                dft1.Harmonics[i] = new Complex(dft1.Harmonics[i].Real, img);
            }


            for (int i = 0; i < dft1.RealParts.Count; i++)
                NewHarmonics.Add(dft1.Harmonics[i] * dft2.Harmonics[i]);

            List<float> Amps = new List<float>();
            List<float> PS = new List<float>();

            for (int i = 0; i < NewHarmonics.Count; i++)
            {
                double amplitude = Math.Sqrt(Math.Pow(NewHarmonics[i].Real, 2) + Math.Pow(NewHarmonics[i].Imaginary, 2));
                Amps.Add((float)amplitude);
                double phaseShift = Math.Atan2(NewHarmonics[i].Imaginary, NewHarmonics[i].Real);
                PS.Add((float)phaseShift);
            }

            Signal inter_signal = new Signal(new List<float>(), false);
            inter_signal.FrequenciesPhaseShifts = PS;
            inter_signal.FrequenciesAmplitudes = Amps;

            InverseDiscreteFourierTransform idft = new InverseDiscreteFourierTransform();
            idft.InputFreqDomainSignal = inter_signal;

            idft.Run();

            for (int i = 0; i < idft.OutputTimeDomainSignal.Samples.Count; i++)
            {
                idft.OutputTimeDomainSignal.Samples[i] = idft.OutputTimeDomainSignal.Samples[i]/ new_len;
            }

            OutputNonNormalizedCorrelation = idft.OutputTimeDomainSignal.Samples;

            OutputNormalizedCorrelation = new List<float>();

            for (int i = 0; i < OutputNonNormalizedCorrelation.Count; i++)
                OutputNormalizedCorrelation.Add((float)(idft.OutputTimeDomainSignal.Samples[i] / normalization_sum));
        }
    }
}