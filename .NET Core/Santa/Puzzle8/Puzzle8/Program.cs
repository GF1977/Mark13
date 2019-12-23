using System;
using System.Collections.Generic;
using System.IO;

//The image you received is 25 pixels wide and 6 pixels tall.
//To make sure the image wasn't corrupted during transmission, the Elves would like you to find the layer that contains the fewest 0 digits.
//On that layer, what is the number of 1 digits multiplied by the number of 2 digits?

namespace Puzzle8
{
    class Program
    {
        class SIP
        {
            private struct strSIP
            {
                public int LayerNumber;
                public int[] LayerData;
            }

            private List<strSIP> theSIP = new List<strSIP>();

            public void SetDataInLayer(int nLayer, int[] RawData)
            {
                strSIP tempSIP;
                tempSIP.LayerData = new int[150];
                RawData.CopyTo(tempSIP.LayerData,0);
                tempSIP.LayerNumber = nLayer;
                theSIP.Add(tempSIP);
            }

            public int GetNumberOfDigints(int nLayer, int Digit)
            {
                int res = 0;
                for (int pos = 0; pos < 150; pos++)
                        if (theSIP[nLayer].LayerData[pos] == Digit)
                            res++;
                return res;
            }

            public int GetLayersCount()
            {
                return theSIP.Count;
            }
        }

        static void Main(string[] args)
        {
            StreamReader file = new StreamReader(@".\data.txt");
            string line = file.ReadToEnd();
            char[] cRawData = line.ToCharArray();
            int[] tempData = new int[150];
            int nLayer = 0;
            SIP password = new SIP();
            while(nLayer < line.Length/150)
            {
                for (int pos = 0; pos < 150; pos++)
                    {
                        char pixel = cRawData[nLayer*pos+pos];
                        tempData[pos] = int.Parse(pixel.ToString());
                    }
                password.SetDataInLayer(nLayer, tempData);
                nLayer++;

            }
            int minZeros = password.GetNumberOfDigints(0,0);
            int minZerosLayer = 0;
            for(int i = 1;i < password.GetLayersCount();i++)
            {
                int z = password.GetNumberOfDigints(i, 0);
                if (minZeros > z)
                {
                    minZeros = z;
                    minZerosLayer = i;
                }
                Console.WriteLine("Layer: {0}     0: {1}", i, password.GetNumberOfDigints(minZerosLayer, 0));
                Console.WriteLine("Layer: {0}     1: {1}", i, password.GetNumberOfDigints(minZerosLayer, 1));
                Console.WriteLine("Layer: {0}     2: {1}", i, password.GetNumberOfDigints(minZerosLayer, 2));
            }

            int res = password.GetNumberOfDigints(minZerosLayer, 1) * password.GetNumberOfDigints(minZerosLayer, 2);
            Console.WriteLine("----------------------------");
            Console.WriteLine("Layer: {0}     Res: {1}", minZerosLayer,res);
        }
    }
}
