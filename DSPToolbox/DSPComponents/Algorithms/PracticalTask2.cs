using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace DSPAlgorithms.Algorithms
{
    public class PracticalTask2 : Algorithm
    {
        public String SignalPath { get; set; }
        public float Fs { get; set; }
        public float miniF { get; set; }
        public float maxF { get; set; }
        public float newFs { get; set; }
        public int L { get; set; } //upsampling factor
        public int M { get; set; } //downsampling factor
        public Signal OutputFreqDomainSignal { get; set; }

        private bool NotAliasingFrequency(float nFs, float maxFreq)
        {
            return nFs >= (2 * maxFreq);
        }

        public override void Run()
        {
            Signal InputSignal = LoadSignal(SignalPath);

            // Band Pass filtering the signal
            FIR filter = new FIR();
            filter.InputFilterType = FILTER_TYPES.BAND_PASS;
            filter.InputTimeDomainSignal= InputSignal;
            filter.InputF1 = miniF; filter.InputF2 = maxF;
            filter.InputFS = Fs;
            filter.InputStopBandAttenuation = 50;
            filter.InputTransitionBand = 500;
            filter.Run();

            Signal filterdSignal = filter.OutputYn;
            SaveSignal("\\SignalFiltered.ds", filterdSignal, false, false);

            // Resampling the signal if the new Fs doesn't alias it.
            Signal resampledSignal;
            if (NotAliasingFrequency(newFs, maxF))
            {
                Sampling sampling = new Sampling();
                sampling.L = L;
                sampling.M = M;
                sampling.InputSignal= filterdSignal;
                sampling.Run();
                resampledSignal = sampling.OutputSignal;
                SaveSignal("\\SignalResampled.ds", resampledSignal, false, false);
            } 
            else
            {
                Console.WriteLine("New Sampling Frequency is NOT valid\n");
                resampledSignal = filterdSignal;
            }

            // Removing DC component
            DC_Component dc = new DC_Component();
            dc.InputSignal = resampledSignal;
            dc.Run();

            Signal dcSignal = dc.OutputSignal;
            SaveSignal("\\SignalDCRemoved.ds", dcSignal, false, false);

            // Normalizing the signal
            Normalizer normalizer= new Normalizer();
            normalizer.InputMinRange = -1;
            normalizer.InputMaxRange = 1;
            normalizer.InputSignal = dcSignal;
            normalizer.Run();

            Signal signalNormalized = normalizer.OutputNormalizedSignal;
            SaveSignal("\\SignalNormalized.ds", signalNormalized, false, false);

            // Applying DFT
            DiscreteFourierTransform dft = new DiscreteFourierTransform();
            dft.InputTimeDomainSignal = signalNormalized;
            dft.Run();
            Signal signalFreqDomain = dft.OutputFreqDomainSignal;

            // Calculating the frequencies
            double realPart = 2 * Math.PI / (signalFreqDomain.FrequenciesAmplitudes.Count * (1 / Fs));

            signalFreqDomain.Frequencies = new List<float>();
            for (int i = 0; i < signalFreqDomain.FrequenciesPhaseShifts.Count; i++)
            {
                signalFreqDomain.Frequencies.Add((float)Math.Round(i * realPart, 1));
                signalFreqDomain.Frequencies[i] = (float) Math.Round(signalFreqDomain.Frequencies[i], 1);
            }

            SaveSignal("\\SignalFreqDomain.ds", signalFreqDomain, true, false);

            OutputFreqDomainSignal = signalFreqDomain;
        }

        public Signal LoadSignal(string filePath)
        {
            Stream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var sr = new StreamReader(stream);

            var sigType = byte.Parse(sr.ReadLine());
            var isPeriodic = byte.Parse(sr.ReadLine());
            long N1 = long.Parse(sr.ReadLine());

            List<float> SigSamples = new List<float>(unchecked((int)N1));
            List<int> SigIndices = new List<int>(unchecked((int)N1));
            List<float> SigFreq = new List<float>(unchecked((int)N1));
            List<float> SigFreqAmp = new List<float>(unchecked((int)N1));
            List<float> SigPhaseShift = new List<float>(unchecked((int)N1));

            if (sigType == 1)
            {
                SigSamples = null;
                SigIndices = null;
            }

            for (int i = 0; i < N1; i++)
            {
                if (sigType == 0 || sigType == 2)
                {
                    var timeIndex_SampleAmplitude = sr.ReadLine().Split();
                    SigIndices.Add(int.Parse(timeIndex_SampleAmplitude[0]));
                    SigSamples.Add(float.Parse(timeIndex_SampleAmplitude[1]));
                }
                else
                {
                    var Freq_Amp_PhaseShift = sr.ReadLine().Split();
                    SigFreq.Add(float.Parse(Freq_Amp_PhaseShift[0]));
                    SigFreqAmp.Add(float.Parse(Freq_Amp_PhaseShift[1]));
                    SigPhaseShift.Add(float.Parse(Freq_Amp_PhaseShift[2]));
                }
            }

            if (!sr.EndOfStream)
            {
                long N2 = long.Parse(sr.ReadLine());

                for (int i = 0; i < N2; i++)
                {
                    var Freq_Amp_PhaseShift = sr.ReadLine().Split();
                    SigFreq.Add(float.Parse(Freq_Amp_PhaseShift[0]));
                    SigFreqAmp.Add(float.Parse(Freq_Amp_PhaseShift[1]));
                    SigPhaseShift.Add(float.Parse(Freq_Amp_PhaseShift[2]));
                }
            }

            stream.Close();
            return new Signal(SigSamples, SigIndices, isPeriodic == 1, SigFreq, SigFreqAmp, SigPhaseShift);
        }

        public void SaveSignal(string filePath, Signal signal, bool freqDomain, bool periodic)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                // If it's in the frequency domain, write 1; otherwise 0
                if (freqDomain)
                {
                    writer.WriteLine(1);
                }
                else
                {
                    writer.WriteLine(0);
                }

                // If it's periodic, write 1; otherwise 0
                if (periodic)
                {
                    writer.WriteLine(1);
                }
                else
                {
                    writer.WriteLine(0);
                }

                // If it's in the frequency domain, write the frequencies, amplitudes, and phaseshifts;
                // otherwise the samples and it's indicies
                if (freqDomain)
                {
                    writer.WriteLine(signal.FrequenciesAmplitudes.Count());

                    for (int i = 0; i < signal.FrequenciesAmplitudes.Count(); i++)
                    {
                        writer.Write(signal.Frequencies[i]);
                        writer.Write(" ");
                        writer.Write(signal.FrequenciesAmplitudes[i]);
                        writer.Write(" ");
                        writer.Write(signal.FrequenciesPhaseShifts[i]);
                        writer.WriteLine();
                    }
                }
                else
                {
                    writer.WriteLine(signal.Samples.Count());

                    for (int i = 0; i < signal.Samples.Count(); i++)
                    {
                        writer.Write(signal.SamplesIndices[i]);
                        writer.Write(" ");
                        writer.Write(signal.Samples[i]);
                        writer.WriteLine();

                    }
                }

            }
        }
    }
}
