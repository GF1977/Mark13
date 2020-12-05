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
            //List<string> myInput = new List<string>();
            List<int> myInput = new List<int>();
            int nN1 = 0;
            int nN2 = 0;

            while (!file.EndOfStream)
                myInput.Add(int.Parse(file.ReadLine()));


            foreach(int S in myInput)
            {
                nN1 = S;
                nN2 = myInput.Find(n => n  == (2020 - nN1));
                if (nN2 > 0)
                    break;
            }

            Console.WriteLine("Part one:  {0} * {1} = {2}", nN1, nN2, nN1 * nN2);
        }
    }
}
