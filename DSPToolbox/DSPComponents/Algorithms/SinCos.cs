using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class SinCos : Algorithm
    {
        public string type { get; set; }
        public float A { get; set; }
        public float PhaseShift { get; set; }
        public float AnalogFrequency { get; set; }
        public float SamplingFrequency { get; set; }
        public List<float> samples { get; set; }
        public override void Run()
        {
            samples = new List<float>();
            float digitalFrequency = AnalogFrequency / SamplingFrequency;

            if (type.Equals("sin"))
            {
                for (int i = 0; i < SamplingFrequency; i++)
                {
                    double result = A * Math.Sin(2 * Math.PI * digitalFrequency * i + PhaseShift);
                    samples.Add((float)result);
                }
            }
            else if (type.Equals("cos"))
            {
                for (int i = 0; i < SamplingFrequency; i++)
                {
                    double result = A * Math.Cos(2 * Math.PI * digitalFrequency * i + PhaseShift);
                    samples.Add((float)result);

                }
            }
            else
            {
                throw new Exception("Unknown Type");
            }

        }
    }
}
