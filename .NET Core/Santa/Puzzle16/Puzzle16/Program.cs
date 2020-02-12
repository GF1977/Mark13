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

            // 80871224585914546619083218645595 becomes 24176176.
            // 19617804207202209144916044189917 becomes 73745418.
            // 69317163492948606335995924319873 becomes 52432133.

            //line = "69317163492948606335995924319873";

            int[] nLine = new int[line.Length];
            for (int i = 0; i < line.Length; i++)
                nLine[i] = int.Parse(line[i].ToString());

            //Part One

            for (int ii = 0; ii < PHASES_COUNT; ii++)
            {
                nLine = RunPhase(nLine);
            }

            string answer = "";
            for (int i = 0; i < 8; i++)
                answer += nLine[i].ToString();

            Console.WriteLine();
            Console.WriteLine("Answer: {0}", answer);


            //Part Two
            line = "123";

                string lineX10000 = "";
                for (int i = 0; i < 4; i++)
                    lineX10000 += line;

                int[] nLine2 = new int[lineX10000.Length];
                for (int i = 0; i < lineX10000.Length; i++)
                    nLine2[i] = int.Parse(lineX10000[i].ToString());

                int nOffset = int.Parse(lineX10000.Substring(0, 7));

            DateTime time = DateTime.Now;
            for (int i = 0; i < PHASES_COUNT; i++)
            {
                nLine2 = RunPhase(nLine2);
                //Console.WriteLine("Round {0} = {1} Min {2} Sec", i, (DateTime.Now - time).Minutes, (DateTime.Now - time).Seconds);
                string a = "";
                for (int x = 0; x < 12; x++)
                    a += nLine2[x];

                Console.WriteLine(a);
            }

                //answer = "";
                //for (int i = 0; i < 8; i++)
                // answer += nLine2[nOffset + i].ToString();

                ////for (int i = 0; i < 8; i++)
                // //answer += nLine2[i].ToString();

                //Console.WriteLine("Answer: {0}", answer);
            
        }

        static int[] RunPhase(int[] nLine)
        {
            int[] nRes = new int[nLine.Length];
            for (int n = 1; n <= nLine.Length; n++)
            {
                int a = 0;
                for (int i = 1; i <= nLine.Length; i++)
                {
                    //int p = (2 - ((i+1) % (4*n + 4)) / (n+1)) % 2;
                    int p = (2 - (i/n % 4)) % 2;
                    a += (nLine[i-1] * p);
                   
                }
                nRes[n-1] = Math.Abs(a) % 10;
            }
            return nRes;
        }
        
    }
}
