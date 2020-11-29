using System;
using System.Collections.Generic;
using System.IO;

namespace Puzzle24
{
    class Program
    {
        static void Main(string[] args)
        {
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



            ShowTerra(Terra, nTerraSize);







            Console.WriteLine("Press any key");
            Console.ReadKey();
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
