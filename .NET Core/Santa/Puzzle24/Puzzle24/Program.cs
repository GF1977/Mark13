using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Puzzle24
{
    class Program
    {
        static void Main(string[] args)
        {
            bool bStop = false;

            StreamReader file = new StreamReader(@".\data.txt");
            string line = file.ReadLine();

            int nTerraSize = line.Length;
            char[,] Terra = new char[nTerraSize, nTerraSize];
            char[,] TerraNew = new char[nTerraSize, nTerraSize];

            int nRowNumber = 0;
            while (line != null)
            {
                int nColNumber = 0;
                foreach (char c in line)
                {
                    Terra[nRowNumber, nColNumber] = c;
                    nColNumber++;
                }

                nRowNumber++;
                line = file.ReadLine();
            }


            Int64 nBioDiversity = 0;
            List<Int64> lBioDiversity = new List<Int64>();


            while (!bStop)
            {
                //ShowTerra(Terra, nTerraSize);
                //Console.ReadKey();
                nBioDiversity = GetBiodiversity(Terra, nTerraSize);
                Int64 x = lBioDiversity.FindIndex(n => n == nBioDiversity); 
                if (x>=0)
                    {
                        Console.WriteLine("Part ONE: {0}", nBioDiversity);
                        bStop = true;
                    }

                lBioDiversity.Add(nBioDiversity);


                for (int r = 0; r < nTerraSize; r++)
                    for (int c = 0; c < nTerraSize; c++)
                    {
                        int nNeighbors = 0;

                        //if (IsValidCoordinate(r + 1, c + 1, nTerraSize) && Terra[r + 1, c + 1] == '#') nNeighbors++;
                        if (IsValidCoordinate(r + 1, c + 0, nTerraSize) && Terra[r + 1, c + 0] == '#') nNeighbors++;
                        //if (IsValidCoordinate(r + 1, c - 1, nTerraSize) && Terra[r + 1, c - 0]  == '#') nNeighbors++;

                        if (IsValidCoordinate(r + 0, c + 1, nTerraSize) && Terra[r + 0, c + 1] == '#') nNeighbors++;
                        if (IsValidCoordinate(r + 0, c - 1, nTerraSize) && Terra[r + 0, c - 1] == '#') nNeighbors++;



                        //if (IsValidCoordinate(r - 1, c + 1, nTerraSize) && Terra[r - 1, c + 1] == '#') nNeighbors++;
                        if (IsValidCoordinate(r - 1, c + 0, nTerraSize) && Terra[r - 1, c + 0] == '#') nNeighbors++;
                        //if (IsValidCoordinate(r - 1, c - 1, nTerraSize) && Terra[r - 1, c - 1] == '#') nNeighbors++;


                        TerraNew[r, c] = Terra[r, c];

                        if (nNeighbors != 1 && Terra[r, c] == '#')
                            TerraNew[r, c] = '.';

                        if ((nNeighbors == 1 || nNeighbors == 2) && Terra[r, c] == '.')
                            TerraNew[r, c] = '#';
                    }

                Terra = CopyTerra(TerraNew, nTerraSize);
                
            }










            Console.WriteLine("Press any key");
            Console.ReadKey();
        }

        private static Int64 GetBiodiversity(char[,] Terra, int nTerraSize)
        {
            Int64 Result =0;

            for (int r = 0; r < nTerraSize; r++)
                for (int c = 0; c < nTerraSize; c++)
                    if (Terra[r, c] == '#')
                        Result += (Int64)Math.Pow(2, r * nTerraSize + c);

            return Result;
        }


        private static bool IsValidCoordinate(int x, int y, int nArraySize)
        {
            if (x >= 0 && x < nArraySize && y >= 0 && y < nArraySize)
                return true;
            else
                return false;
        }


        private static char[,] CopyTerra(char[,] TerraNew, int nTerraSize)
        {
            char[,] Result = new char[nTerraSize, nTerraSize];

            for (int r = 0; r < nTerraSize; r++)
                for (int c = 0; c < nTerraSize; c++)
                    Result[r, c] = TerraNew[r, c];

            return Result;
        }


        private static void ShowTerra(char[,] Terra, int nTerraSize)
        {
            Console.SetCursorPosition(0, 0);
            for (int r = 0; r < nTerraSize; r++)
            {
                for (int c = 0; c < nTerraSize; c++)
                    Console.Write(Terra[r, c]);

                Console.WriteLine();
            }
        }



    }
}
