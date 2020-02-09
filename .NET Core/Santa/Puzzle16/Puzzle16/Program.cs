using System;
using System.Collections.Generic;
using System.IO;

namespace Puzzle16
{
    class Program
    {
        static void Main(string[] args)
        {
            const int PHASES_COUNT = 100;
            StreamReader file = new StreamReader(@".\data.txt");
            string line = file.ReadLine();

            // line = "19617804207202209144916044189917";

            for (int i = 0; i < PHASES_COUNT; i++)
            {
                line = RunPhase(line);
                Console.WriteLine("Phase {0}:   {1}",i,line);
            }

            Console.WriteLine();
            Console.WriteLine("Answer: {0}", line.Substring(0,8));

        }

        static string RunPhase(string line)
        {
            string res = "";
            for (int n = 0; n < line.Length; n++)
            {
                List<int> Pattern = GetPattern(n, line.Length);
                int a = 0;
                for (int i = 0; i < line.Length; i++)
                     a += (int.Parse(line[i].ToString()) * Pattern[i]);

                res += (Math.Abs(a) % 10).ToString();
            }
            return res;
        }

        static List<int> GetPattern(int nElement, int nPatternLen)
        {
            int[] Pattern = new int[] { 0, 1, 0, -1 };
            List<int> Res = new List<int>();

            while(nPatternLen >= Res.Count)
            for(int n = 0;n<4;n++)
            for(int i = 0;i<=nElement;i++)
                    Res.Add(Pattern[n]);

            Res.RemoveAt(0);
            return Res;
        }
    }
}
