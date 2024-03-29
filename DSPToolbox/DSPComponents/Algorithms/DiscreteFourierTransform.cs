﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DiscreteFourierTransform : Algorithm
    {
        public Signal InputTimeDomainSignal { get; set; }
        public float InputSamplingFrequency { get; set; }
        public Signal OutputFreqDomainSignal { get; set; }
        public List<Complex> Harmonics { get; set; }
        public override void Run()
        {
            List<float> amplitudes = new List<float>();
            List<float> phaseShifts = new List<float>();
            Harmonics = new List<Complex>();
            

            double ePower, realPart, imaginaryPart;

            for (int i = 0; i < InputTimeDomainSignal.Samples.Count; i++)
            {
                realPart = 0;
                imaginaryPart = 0;

                for (int j = 0; j < InputTimeDomainSignal.Samples.Count; j++)
                {
                    ePower = (2 * Math.PI * i * j) / InputTimeDomainSignal.Samples.Count;
                    realPart += InputTimeDomainSignal.Samples[j] * Math.Cos(ePower);
                    imaginaryPart += -InputTimeDomainSignal.Samples[j] * Math.Sin(ePower);
                }

                double amplitude = Math.Sqrt(Math.Pow(realPart, 2) + Math.Pow(imaginaryPart, 2));
                amplitudes.Add((float)amplitude);
                double phaseShift = Math.Atan2(imaginaryPart, realPart);
                phaseShifts.Add((float)phaseShift);

                Harmonics.Add(new Complex(realPart, imaginaryPart));
            }

            OutputFreqDomainSignal = new Signal(InputTimeDomainSignal.Samples, false);
            OutputFreqDomainSignal.FrequenciesAmplitudes = amplitudes;
            OutputFreqDomainSignal.FrequenciesPhaseShifts = phaseShifts;

        }
    }
}
