using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Puzzle3
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine(DateTime.Now);
            StreamReader file = new StreamReader(@".\data.txt");

            var vPartTwoAnswer = "";

            List<string> fileInput = new List<string>();

            while (!file.EndOfStream)
            {
                string S = file.ReadLine();
                fileInput.Add(S);
            }

            // Part One
            Int64 nTreeCount  = CheckSlope(fileInput, 3, 1);
            Console.WriteLine("--------------------------");
            Console.WriteLine("PartOne: {0}", nTreeCount);

            // Part Two
            nTreeCount *= CheckSlope(fileInput, 1, 1);
                nTreeCount *= CheckSlope(fileInput, 5, 1);
                nTreeCount *= CheckSlope(fileInput, 7, 1);
                nTreeCount *= CheckSlope(fileInput, 1, 2);
            


            Console.WriteLine("PartTwo: {0}", nTreeCount);

        }

        public static int CheckSlope(List<string> fileInput, int nStepsRight, int nStepsDown)
        {
            int nForrestLen = fileInput[0].Length;

            int nTreeCount = 0;
            int nRow = 0;

            for(int i = 0;i< fileInput.Count;i+= nStepsDown)
            {
                string S = fileInput[i];
                int nXposition = (nStepsRight * (nRow) % nForrestLen);
                if (S[nXposition] == '#')
                    nTreeCount++;
                    
                nRow++;
            }
            // Debug
            //Console.WriteLine("nStepsRight {0}  nStepsDown {1}    -  Trees =  {2}", nStepsRight, nStepsDown, nTreeCount);
            return nTreeCount;
        }
    }
}
