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


            int nForrestLen = fileInput[0].Length;
            int nForrestWid = fileInput.Count;
            
            int nStepsRight = 3;
            int nStepsDown  = 1;

            int nTreeCount = 0;
            int nRow = 0;
            
            foreach(string S in fileInput)
            {
                int nXposition = (nStepsRight*nRow) % nForrestLen;
                if (S[nXposition] == '#')
                    nTreeCount++;

                nRow++;
            }



            Console.WriteLine("--------------------------");
            Console.WriteLine("PartOne: {0}", nTreeCount);
            Console.WriteLine("PartTwo: {0}", vPartTwoAnswer);

        }
    }
}
