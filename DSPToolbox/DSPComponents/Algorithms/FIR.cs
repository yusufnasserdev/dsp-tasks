using System;
using System.Collections.Generic;
using System.Linq;
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
        private int HalfWay { get; set; }
        private List<float> WindowFunction { get; set; }

        private void FilterLow()
        {
            // Normalizing InputCutOffFrequency
            InputCutOffFrequency /= InputFS;
            InputCutOffFrequency += (InputCutOffFrequency / 2);

            List<float> HDn = new List<float>();

            HDn.Add((float)(2 * InputCutOffFrequency));

            for (int i = 1; i < HalfWay; i++)
            {
                float Wc = (float)(2 * Math.PI * InputCutOffFrequency);
                HDn.Add((float)(2 * InputCutOffFrequency * (Math.Sin(i * Wc) / (i * Wc))));
            }

            List<int> NewIndices = new List<int>();
            int firstIndex = (-1 * HalfWay) + 1;
            for (; firstIndex < HalfWay; firstIndex++)
                NewIndices.Add(firstIndex);

            List<float> NewSamples = new List<float>();




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

            WINDOW_TYPE? WindowType = WINDOW_TYPE.RECTANGULAR;
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
            HalfWay = (N - 1) / 2; HalfWay++;

            if (WindowType == WINDOW_TYPE.RECTANGULAR)
            {
                for (int i = 0; i < HalfWay; i++)
                    WindowFunction.Add(1);
            }
            else if (WindowType == WINDOW_TYPE.HANNING)
            {
                for (int i = 0; i < HalfWay; i++)
                    WindowFunction.Add((float)(0.5 + (0.5 * Math.Cos((2 * Math.PI * i) / N))));

            }
            else if (WindowType == WINDOW_TYPE.HAMMING)
            {
                for (int i = 0; i < HalfWay; i++)
                    WindowFunction.Add((float)(0.54 + (0.46 * Math.Cos((2 * Math.PI * i) / N))));
            }
            else
            {
                for (int i = 0; i < HalfWay; i++)
                    WindowFunction.Add((float)(0.42 +
                        (0.5 * Math.Cos((2 * Math.PI * i) / (N - 1))) +
                        (0.08 * Math.Cos((4 * Math.PI * i) / (N - 1)))));
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
