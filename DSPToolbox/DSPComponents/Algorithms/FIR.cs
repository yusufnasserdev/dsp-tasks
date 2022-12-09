using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class FIR : Algorithm
    {

        private enum WINDOW_TYPE
        {
            RECTANGULAR, HANNING, HAMMING, BLACKMAN
        }

        public Signal InputTimeDomainSignal { get; set; }
        public FILTER_TYPES InputFilterType { get; set; }
        public float InputFS { get; set; } // 8 khz
        public float? InputCutOffFrequency { get; set; } // 1.5 khz
        public float? InputF1 { get; set; } // cut-off 1 band pass or reject
        public float? InputF2 { get; set; } // cut-off 2 band pass or reject
        public float InputStopBandAttenuation { get; set; }
        public float InputTransitionBand { get; set; } // 0.5 khz
        public Signal OutputHn { get; set; }
        public Signal OutputYn { get; set; }
        private int N { get; set; }
        private int LeftMostIndex { get; set; }
        private List<float> WindowFunction { get; set; }

        private void FilterLow()
        {
            // Normalizing InputCutOffFrequency
            InputCutOffFrequency /= InputFS;
            InputCutOffFrequency += (InputTransitionBand / 2);

            List<float> HDn = new List<float>();

            // HD(n)
            float Wc = (float)(2 * Math.PI * InputCutOffFrequency);

            for (int i = N / 2; i > 0; i--)
                HDn.Add((float)(2 * InputCutOffFrequency * Math.Sin(i * Wc) / (i * Wc)));

            // HD(0)
            HDn.Add((float)(2 * InputCutOffFrequency));
            
            // Generating samples
            List<float> NewSamples = new List<float>();
            List<float> LeftSideSamples = new List<float>();
            for (int i = 0; i <= N / 2; i++)
                LeftSideSamples.Add(HDn[i] * WindowFunction[i]);

            // Adding the left side samples
            NewSamples.AddRange(LeftSideSamples);

            // Removing the middle sample so it won't be added twice
            LeftSideSamples.RemoveAt(N / 2);

            // Reversomg the left side samples so they would be added as the right ones.
            LeftSideSamples.Reverse();

            // Adding the right side samples
            NewSamples.AddRange(LeftSideSamples);

            // Generating samples indices
            List<int> NewIndices = new List<int>();
            int firstIndex = LeftMostIndex;
            for (; firstIndex <= N / 2; firstIndex++)
                NewIndices.Add(firstIndex);

            // Initializing the filtered signal
            OutputHn = new Signal(NewSamples, NewIndices, false);
        }

        private void FilterHigh()
        {

        }

        private void FilterBandPass()
        {

        }

        private void FilterBandReject()
        {

        }

        public override void Run()
        {
            // Normalizing InputTransitionBand
            InputTransitionBand /= InputFS;

            // Determining window function and initial samples count

            // Minimal case is assumed to be defualt

            WINDOW_TYPE WindowType = WINDOW_TYPE.RECTANGULAR;
            N = (int)(0.9 / InputTransitionBand); // Samples Count

            // Other cases
            if (InputStopBandAttenuation > 22 && InputStopBandAttenuation < 45)
            {
                N = (int)(3.1 / InputTransitionBand);
                WindowType = WINDOW_TYPE.HANNING;
            }
            else if (InputStopBandAttenuation > 44 && InputStopBandAttenuation < 54)
            {
                N = (int)(3.3 / InputTransitionBand);
                WindowType = WINDOW_TYPE.HAMMING;
            }
            else
            {
                N = (int)(5.5 / InputTransitionBand);
                WindowType = WINDOW_TYPE.BLACKMAN;
            }

            // Nearing the N to the nearest bigger odd value
            if (N % 2 == 1)
                N += 2;
            else
                N += 1;


            WindowFunction = new List<float>();
            LeftMostIndex = N / -2;
            double nm = 0, nm1 = 0, nm2 = 0, dn = 0;

            if (WindowType == WINDOW_TYPE.RECTANGULAR)
            {
                for (int i = LeftMostIndex; i < 1; i++)
                    WindowFunction.Add(1);
            }
            else if (WindowType == WINDOW_TYPE.HANNING)
            {
                for (int i = LeftMostIndex; i < 1; i++)
                {
                    nm = (2 * Math.PI * i);
                    WindowFunction.Add((float)(0.5 + (0.5 * Math.Cos(nm / N))));
                }

            }
            else if (WindowType == WINDOW_TYPE.HAMMING)
            {
                for (int i = LeftMostIndex; i < 1; i++)
                {
                    nm = (2 * Math.PI * i);
                    WindowFunction.Add((float)(0.54 + (0.46 * Math.Cos(nm / N))));
                }
            }
            else
            {
                for (int i = LeftMostIndex; i < 1; i++)
                {
                    nm1 = (2 * Math.PI * i);
                    nm2 = (4 * Math.PI * i);
                    dn = N - 1;
                    WindowFunction.Add((float)(0.42 + (0.5 * Math.Cos(nm1 / dn)) + (0.08 * Math.Cos(nm2 / dn))));
                }
            }

            if (InputFilterType == FILTER_TYPES.LOW)
            {
                FilterLow();
            }
            else if (InputFilterType == FILTER_TYPES.HIGH)
            {
                FilterHigh();
            }
            else if (InputFilterType == FILTER_TYPES.BAND_PASS)
            {
                FilterBandPass();
            }
            else
            {
                FilterBandReject();
            }
        }
    }
}
