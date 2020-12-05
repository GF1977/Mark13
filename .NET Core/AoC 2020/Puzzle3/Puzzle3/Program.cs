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

           
            int nStepsRight = 3;
            int nStepsDown  = 1;
           

            int nTreeCount = CheckSlope(fileInput, nStepsRight, nStepsDown);



            Console.WriteLine("--------------------------");
            Console.WriteLine("PartOne: {0}", nTreeCount);
            Console.WriteLine("PartTwo: {0}", vPartTwoAnswer);

        }

        public static int CheckSlope(List<string> fileInput, int nStepsRight, int nStepsDown)
        {
            int nForrestLen = fileInput[0].Length;

            int nTreeCount = 0;
            int nRow = 0;

            foreach (string S in fileInput)
            {
                int nXposition = (nStepsRight * nRow) % nForrestLen;
                if (S[nXposition] == '#')
                    nTreeCount++;

                nRow++;
            }

            return nTreeCount;
        }
    }
}
