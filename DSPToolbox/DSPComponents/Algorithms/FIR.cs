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
        private int HalfWay { get; set; }
        private List<float> WindowFunction { get; set; }

        private void FilterSignal()
        {
            DirectConvolution dc = new DirectConvolution();
            dc.InputSignal1 = InputTimeDomainSignal;
            dc.InputSignal2 = OutputHn;
            dc.Run();
            OutputYn = dc.OutputConvolvedSignal;
        }

        private void FilterLow()
        {
            // Normalizing InputCutOffFrequency
            InputCutOffFrequency /= InputFS;
            InputCutOffFrequency += (InputTransitionBand / 2);

            List<float> HDn = new List<float>
            {
                (float)(2 * InputCutOffFrequency)
            };

            float Wc = (float)(2 * Math.PI * InputCutOffFrequency);

            for (int i = 1; i < HalfWay; i++)
                HDn.Add((float)(2 * InputCutOffFrequency * (Math.Sin(i * Wc) / (i * Wc))));
            
            // Generating samples
            List<float> NewSamples = new List<float>();
            List<float> RightSideSamples = new List<float>();

            for (int i = 1; i < HalfWay; i++)
                RightSideSamples.Add(HDn[i] * WindowFunction[i]);

            // Adding the right side samples then reversing them to represent the left side samples
            NewSamples.AddRange(RightSideSamples);
            NewSamples.Reverse();

            // Adding the middle sample 
            NewSamples.Add(HDn[0] * WindowFunction[0]);

            // Adding the right side samples
            NewSamples.AddRange(RightSideSamples);

            // Generating samples indices
            List<int> NewIndices = new List<int>();
            int firstIndex = (-1 * HalfWay) + 1;
            for (; firstIndex < HalfWay; firstIndex++)
                NewIndices.Add(firstIndex);

            // Initializing the filtered signal
            OutputHn = new Signal(NewSamples, NewIndices, false);

            // Return filtered signal if inputed
            if (InputTimeDomainSignal!= null)
                FilterSignal();
        }

        private void FilterHigh()
        {
            // Normalizing InputCutOffFrequency
            InputCutOffFrequency /= InputFS;
            InputCutOffFrequency += (InputTransitionBand / 2);

            List<float> HDn = new List<float>
            {
                (float)(1 - (2 * InputCutOffFrequency))
            };

            float Wc = (float)(2 * Math.PI * InputCutOffFrequency);

            for (int i = 1; i < HalfWay; i++)
                HDn.Add((float)(-2 * InputCutOffFrequency * (Math.Sin(i * Wc) / (i * Wc))));

            // Generating samples
            List<float> NewSamples = new List<float>();
            List<float> RightSideSamples = new List<float>();

            for (int i = 1; i < HalfWay; i++)
                RightSideSamples.Add(HDn[i] * WindowFunction[i]);

            // Adding the right side samples then reversing them to represent the left side samples
            NewSamples.AddRange(RightSideSamples);
            NewSamples.Reverse();

            // Adding the middle sample 
            NewSamples.Add(HDn[0] * WindowFunction[0]);

            // Adding the right side samples
            NewSamples.AddRange(RightSideSamples);

            // Generating samples indices
            List<int> NewIndices = new List<int>();
            int firstIndex = (-1 * HalfWay) + 1;
            for (; firstIndex < HalfWay; firstIndex++)
                NewIndices.Add(firstIndex);

            // Initializing the filtered signal
            OutputHn = new Signal(NewSamples, NewIndices, false);

            // Return filtered signal if inputed
            if (InputTimeDomainSignal != null)
                FilterSignal();
        }

        private void FilterBandPass()
        {
            // Normalizing InputCutOffFrequency 1 and 2 
            InputF1 /= InputFS;
            InputF1 -= (InputTransitionBand / 2);
            InputF2 /= InputFS;
            InputF2 += (InputTransitionBand / 2);

            List<float> HDn = new List<float>
            {
                (float)(2 * (InputF2 - InputF1))
            };

            float Wc1 = (float)(2 * Math.PI * InputF1);
            float Wc2 = (float)(2 * Math.PI * InputF2);

            for (int i = 1; i < HalfWay; i++)
            {
                float FirstHalf = (float)(2 * InputF2 * (Math.Sin(i * Wc2) / (i * Wc2)));
                float SecondHalf = (float)(-2 * InputF1 * (Math.Sin(i * Wc1) / (i * Wc1)));
                HDn.Add(FirstHalf + SecondHalf);
            }

            // Generating samples
            List<float> NewSamples = new List<float>();
            List<float> RightSideSamples = new List<float>();

            for (int i = 1; i < HalfWay; i++)
                RightSideSamples.Add(HDn[i] * WindowFunction[i]);

            // Adding the right side samples then reversing them to represent the left side samples
            NewSamples.AddRange(RightSideSamples);
            NewSamples.Reverse();

            // Adding the middle sample 
            NewSamples.Add(HDn[0] * WindowFunction[0]);

            // Adding the right side samples
            NewSamples.AddRange(RightSideSamples);

            // Generating samples indices
            List<int> NewIndices = new List<int>();
            int firstIndex = (-1 * HalfWay) + 1;
            for (; firstIndex < HalfWay; firstIndex++)
                NewIndices.Add(firstIndex);

            // Initializing the filtered signal
            OutputHn = new Signal(NewSamples, NewIndices, false);

            // Return filtered signal if inputed
            if (InputTimeDomainSignal != null)
                FilterSignal();
        }

        private void FilterBandReject()
        {
            // Normalizing InputCutOffFrequency 1 and 2 
            InputF1 /= InputFS;
            InputF1 -= (InputTransitionBand / 2);
            InputF2 /= InputFS;
            InputF2 += (InputTransitionBand / 2);

            List<float> HDn = new List<float>
            {
                (float)(1 - (2 * (InputF2 - InputF1)))
            };

            float Wc1 = (float)(2 * Math.PI * InputF1);
            float Wc2 = (float)(2 * Math.PI * InputF2);

            for (int i = 1; i < HalfWay; i++)
            {
                float FirstHalf = (float)(2 * InputF1 * (Math.Sin(i * Wc1) / (i * Wc1)));
                float SecondHalf = (float)(-2 * InputF2 * (Math.Sin(i * Wc2) / (i * Wc2)));
                HDn.Add(FirstHalf + SecondHalf);
            }

            // Generating samples
            List<float> NewSamples = new List<float>();
            List<float> RightSideSamples = new List<float>();

            for (int i = 1; i < HalfWay; i++)
                RightSideSamples.Add(HDn[i] * WindowFunction[i]);

            // Adding the right side samples then reversing them to represent the left side samples
            NewSamples.AddRange(RightSideSamples);
            NewSamples.Reverse();

            // Adding the middle sample 
            NewSamples.Add(HDn[0] * WindowFunction[0]);

            // Adding the right side samples
            NewSamples.AddRange(RightSideSamples);

            // Generating samples indices
            List<int> NewIndices = new List<int>();
            int firstIndex = (-1 * HalfWay) + 1;
            for (; firstIndex < HalfWay; firstIndex++)
                NewIndices.Add(firstIndex);

            // Initializing the filtered signal
            OutputHn = new Signal(NewSamples, NewIndices, false);

            // Return filtered signal if inputed
            if (InputTimeDomainSignal != null)
                FilterSignal();
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
            HalfWay = N / 2; HalfWay++;
            double nm = 0, nm1 = 0, nm2 = 0, dn = 0;

            if (WindowType == WINDOW_TYPE.RECTANGULAR)
            {
                for (int i = 0; i < HalfWay; i++)
                    WindowFunction.Add(1);
            }
            else if (WindowType == WINDOW_TYPE.HANNING)
            {
                for (int i = 0; i < HalfWay; i++)
                {
                    nm = (2 * Math.PI * i);
                    WindowFunction.Add((float)(0.5 + (0.5 * Math.Cos(nm / N))));
                }

            }
            else if (WindowType == WINDOW_TYPE.HAMMING)
            {
                for (int i = 0; i < HalfWay; i++)
                {
                    nm = (2 * Math.PI * i);
                    WindowFunction.Add((float)(0.54 + (0.46 * Math.Cos(nm / N))));
                }
            }
            else
            {
                for (int i = 0; i < HalfWay; i++)
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
