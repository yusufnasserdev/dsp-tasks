﻿using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class Sampling : Algorithm
    {
        public int L { get; set; } //upsampling factor
        public int M { get; set; } //downsampling factor
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }




        public override void Run()
        {
            // if L != 0 and M = 0 this is a up Sampling
            // add values and zeros then make filter
            if(L != 0 && M == 0)
            {

                //list to save the sample and the zeros that i add
                List<float> newList = new List<float>();
                //loop on sample and add the value of sample in newList then add zeros by number of L-1
                for(int i = 0; i <InputSignal.Samples.Count; i++)
                {
                    newList.Add(InputSignal.Samples[i]);
                    //to check if yhe last element in samples do not add zeros and break from loop
                    if(InputSignal.Samples.Count - i == 1)
                    {
                        break;
                    }
                    else
                    {
                        //loop on number of L-1 to add zeros
                        for (int j = 0; j < L - 1; j++)
                        {
                            newList.Add(0);
                        }
                    }

                }

                FIR lowPassFiter = new FIR();
                Signal signal = new Signal(newList, false);
                lowPassFiter.InputFilterType = DSPAlgorithms.DataStructures.FILTER_TYPES.LOW;
                lowPassFiter.InputCutOffFrequency = 1500;
                lowPassFiter.InputStopBandAttenuation = 50;
                lowPassFiter.InputTransitionBand = 500;
                lowPassFiter.InputFS = 8000;
                lowPassFiter.InputTimeDomainSignal = signal;

                lowPassFiter.Run();


                OutputSignal = lowPassFiter.OutputYn;

            }
            // if M != 0 and L = 0 this is a down Sampling
            //make FIR then meke down sampling
            else if (M != 0 && L == 0)
            {
                //MAKE FIR
                FIR lowPassFiter = new FIR();
                lowPassFiter.InputFilterType = DSPAlgorithms.DataStructures.FILTER_TYPES.LOW;
                lowPassFiter.InputCutOffFrequency = 1500;
                lowPassFiter.InputStopBandAttenuation = 50;
                lowPassFiter.InputTransitionBand = 500;
                lowPassFiter.InputFS = 8000;
                lowPassFiter.InputTimeDomainSignal = InputSignal;
                lowPassFiter.Run();

                //MAKE DOWN SAMPLING

                //list to save the value
                List<float> newList = new List<float>();
                for(int i = 0; i < lowPassFiter.OutputYn.Samples.Count; i += M)
                {
                    newList.Add(lowPassFiter.OutputYn.Samples[i]);
                }
                Signal output = new Signal(newList, false);
                OutputSignal = output;
            }
            // if M != 0 and L != 0 then change sample rate 
            // do up sampling then filter then down sapling
            else if(L !=0 && M !=0)
            {
                List<float> newList = new List<float>();
                for (int i = 0; i < InputSignal.Samples.Count; i++)
                {
                    newList.Add(InputSignal.Samples[i]);
                    //to check if yhe last element in samples do not add zeros and break from loop
                    if (InputSignal.Samples.Count - i == 1)
                    {
                        break;
                    }
                    else
                    {
                        //loop on number of L-1 to add zeros
                        for (int j = 0; j < L - 1; j++)
                        {
                            newList.Add(0);
                        }
                    }
                }

                FIR lowPassFiter = new FIR();
                Signal signal = new Signal(newList, false);
                lowPassFiter.InputFilterType = DSPAlgorithms.DataStructures.FILTER_TYPES.LOW;
                lowPassFiter.InputCutOffFrequency = 1500;
                lowPassFiter.InputStopBandAttenuation = 50;
                lowPassFiter.InputTransitionBand = 500;
                lowPassFiter.InputFS = 8000;
                lowPassFiter.InputTimeDomainSignal = signal;

                lowPassFiter.Run();


                List<float> downSampling = new List<float>();
                for (int i = 0; i < lowPassFiter.OutputYn.Samples.Count; i += M)
                {
                    downSampling.Add(lowPassFiter.OutputYn.Samples[i]);
                }
                Signal output = new Signal(downSampling, false);
                OutputSignal = output;
            }
            else
            {
                Console.WriteLine("ERROR");
            }



        }
    }

}


