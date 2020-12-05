using System;
using System.IO;
using System.Collections.Generic;

namespace Puzzle1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine(DateTime.Now);
            StreamReader file = new StreamReader(@".\data.txt");
            
            List<int> myInput = new List<int>();
            int A = 0;

            while (!file.EndOfStream)
                myInput.Add(int.Parse(file.ReadLine()));


            bool bPartOneSolved = false;
            bool bPartTwoSolved = false;

            foreach (int B in myInput)
                foreach (int C in myInput)
                {
                    if (bPartOneSolved && bPartTwoSolved)
                        return;

                    // Part One
                    A = myInput.Find(n => n == (2020 - C));
                    if (A > 0 && !bPartOneSolved)
                    {
                        Console.WriteLine("Part one:  {0} * {1} = {2}", A, C, A * C);
                        bPartOneSolved = true;
                    }
                    // part Two
                    A = myInput.Find(n => n == (2020 - B - C));
                    if (A > 0)
                    {
                        Console.WriteLine("Part two:  {0} * {1} * {2} = {3}", A, B, C, A * B * C);
                        bPartTwoSolved = true;
                    }
                }

        }
    }
}
