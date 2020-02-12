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
            Console.WriteLine("Part One - Answer: {0}", answer);


            //Part Two
            const int MULTI_10K = 10000;
            //line = "03081770884921959731165446850517";
            Int64 nOffset = int.Parse(line.Substring(0, 7));

            int[] nLineShortP2 = new int[line.Length];
            for (int i = 0; i < line.Length; i++)
                nLineShortP2[i] = int.Parse(line[i].ToString());

            int nLine10KLen = nLineShortP2.Length * MULTI_10K;
            int[] nLine10k = new int[nLine10KLen];

            for (int i = 0; i < MULTI_10K; i++)
                Array.Copy(nLineShortP2,0,nLine10k, line.Length*i,line.Length);

            //DateTime time = DateTime.Now;
            for (int i = 0; i < PHASES_COUNT; i++)
            {
                int aa = 0;
                answer = "";
                for (int x = nLine10KLen - 1 ; x >= nOffset; x--)
                {
                    aa = (aa + nLine10k[x]) % 10;
                    nLine10k[x] = aa;
                    if (x < nOffset + 8)
                        answer = aa.ToString() + answer;
                }
            }

                Console.WriteLine("Part Two - Answer: {0}", answer);
            
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
