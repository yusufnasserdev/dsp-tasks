using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class FastConvolution : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public Signal OutputConvolvedSignal { get; set; }

        /// <summary>
        /// Convolved InputSignal1 (considered as X) with InputSignal2 (considered as H)
        /// </summary>
        public override void Run()
        {
            int new_len = InputSignal1.Samples.Count + InputSignal2.Samples.Count - 1;

            for (int i = 0; i < new_len; i++)
            {
                if (i == InputSignal1.Samples.Count)
                    InputSignal1.Samples.Add(0);

                if (i == InputSignal2.Samples.Count)
                    InputSignal2.Samples.Add(0);
            }

            DiscreteFourierTransform dft1 = new DiscreteFourierTransform();
            dft1.InputTimeDomainSignal = InputSignal1;
            dft1.Run();

            DiscreteFourierTransform dft2 = new DiscreteFourierTransform();
            dft2.InputTimeDomainSignal = InputSignal2;
            dft2.Run();

            List<Complex> NewHarmonics = new List<Complex>();

            for (int i = 0; i < dft1.Harmonics.Count; i++)
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

            int startIndex = InputSignal1.SamplesIndices[0] + InputSignal2.SamplesIndices[0];
            List<int> newIndices = new List<int>();

            for (int i = 0; i < InputSignal1.Samples.Count; i++)
                newIndices.Add(startIndex + i);

            for (int i = 0; i < idft.OutputTimeDomainSignal.Samples.Count; i++)
            {
                idft.OutputTimeDomainSignal.Samples[i] = (float) Math.Round(idft.OutputTimeDomainSignal.Samples[i], 1, MidpointRounding.AwayFromZero);
            }

            OutputConvolvedSignal = new Signal(idft.OutputTimeDomainSignal.Samples, newIndices, false);
        }
    } 
}
