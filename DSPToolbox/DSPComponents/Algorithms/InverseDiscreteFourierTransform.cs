using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;
using System.Numerics;

namespace DSPAlgorithms.Algorithms
{
    public class InverseDiscreteFourierTransform : Algorithm
    {
        public Signal InputFreqDomainSignal { get; set; }
        public Signal OutputTimeDomainSignal { get; set; }

        public override void Run()
        {
            List<Complex> values = new List<Complex>();
            List<Complex> complexNums = new List<Complex>();

                for (int i = 0; i < InputFreqDomainSignal.FrequenciesAmplitudes.Count; i++)
                {
                complexNums.Add(Complex.FromPolarCoordinates(InputFreqDomainSignal.FrequenciesAmplitudes[i],
                        InputFreqDomainSignal.FrequenciesPhaseShifts[i]));
                }

            double termAnswer = 0;

            for (int i = 0; i < InputFreqDomainSignal.FrequenciesAmplitudes.Count; i++)
            {
                values.Add(termAnswer);

                for (int j = 0; j < InputFreqDomainSignal.FrequenciesAmplitudes.Count; j++)
                {
                    double ePower = (2 * Math.PI * i * j) / InputFreqDomainSignal.FrequenciesAmplitudes.Count;
                    values[i] +=
                        (complexNums[j].Real * Math.Cos(ePower) + Complex.ImaginaryOne
                        * complexNums[j].Real * Math.Sin(ePower))
                        + (complexNums[j].Imaginary * -1 * Math.Cos(ePower) +
                        complexNums[j].Imaginary * -1 * Math.Sin(ePower));
                }

                values[i] /= InputFreqDomainSignal.FrequenciesAmplitudes.Count;
            }

            List<float> realPart = new List<float>();

            for (int i = 0; i < InputFreqDomainSignal.FrequenciesAmplitudes.Count; i++)
            {
                realPart.Add((float)values[i].Real);
            }

            OutputTimeDomainSignal = new Signal(realPart, false);
        }
    }
}
