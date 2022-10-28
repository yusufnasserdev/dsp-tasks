using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class QuantizationAndEncoding : Algorithm
    {
        // You will have only one of (InputLevel or InputNumBits), the other property will take a negative value
        // If InputNumBits is given, you need to calculate and set InputLevel value and vice versa
        public int InputLevel { get; set; }
        public int InputNumBits { get; set; }
        public Signal InputSignal { get; set; }
        public Signal OutputQuantizedSignal { get; set; }
        public List<int> OutputIntervalIndices { get; set; }
        public List<string> OutputEncodedSignal { get; set; }
        public List<float> OutputSamplesError { get; set; }

        public override void Run()
        {
            List<float> quantizedSamples = new List<float>();
            List<Tuple<float, float>> intervals = new List<Tuple<float, float>>();

            OutputEncodedSignal = new List<string>();
            OutputIntervalIndices = new List<int>();
            OutputSamplesError = new List<float>();

            if (InputLevel > 0)
                InputNumBits = (int) Math.Log(InputLevel, 2);
            else if (InputNumBits > 0)
                InputLevel = (int) Math.Pow(2, InputNumBits);


            float maxA = InputSignal.Samples.Max(),
                minA = InputSignal.Samples.Min();

            float delta = (maxA - minA) / InputLevel;

            for (float i = minA; i < maxA; i+= delta)
            {
                intervals.Add(new Tuple<float, float>(i, i + delta));
            }

            foreach (float sample in InputSignal.Samples)
            {
                for (int i = 0; i < intervals.Count; i++)
                {
                    if (sample >= intervals[i].Item1 && sample <= intervals[i].Item2 + 0.001)
                    {
                        float midPoint = (intervals[i].Item1 + intervals[i].Item2) / (float) 2.0;
                        quantizedSamples.Add(midPoint);

                        OutputIntervalIndices.Add(i + 1);
                        OutputSamplesError.Add(midPoint - sample);

                        int length = Convert.ToString(i, 2).Length;
                        string zeros = "";

                        while (length < (int) Math.Log(InputLevel, 2))
                        {
                            zeros += "0";
                            length++;
                        }

                        OutputEncodedSignal.Add(zeros + Convert.ToString(i, 2));
                        break;
                    } 
                }
            }

            OutputQuantizedSignal = new Signal(quantizedSamples, InputSignal.Periodic);
            Console.WriteLine("OutputQuantizedSignal: " + OutputQuantizedSignal);
            Console.WriteLine("OutputEncodedSignal: " + OutputEncodedSignal);
            Console.WriteLine("OutputSamplesError: " + OutputSamplesError);
        }
    }
}
