using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Subtractor : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public Signal OutputSignal { get; set; }

        /// <summary>
        /// To do: Subtract Signal2 from Signal1 
        /// i.e OutSig = Sig1 - Sig2 
        /// </summary>
        public override void Run()
        {
            MultiplySignalByConstant m = new MultiplySignalByConstant();

            m.InputSignal = InputSignal2;
            m.InputConstant = -1;
            m.Run();

            InputSignal2 = m.OutputMultipliedSignal;

            Adder a = new Adder();
            a.InputSignals = new List<Signal>();
            a.InputSignals.Add(InputSignal1);
            a.InputSignals.Add(InputSignal2);

            a.Run();

            OutputSignal = new Signal(a.OutputSignal.Samples, false);
        }
    }
}