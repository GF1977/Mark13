using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Puzzle24
{
 
    class Program
    {
        public static int nSize = 5;

        public class Level
        {
            public char[,] nSlice;
            public Level(int size)
            {
                nSlice = new char[nSize, nSize];
            }

            public void CopyTerra(Level TerraNew)
            {
                for (int r = 0; r < nSize ; r++)
                    for (int c = 0; c < nSize  ; c++)
                        this.nSlice[r, c] = TerraNew.nSlice[r, c];
            }


            public void EmptyTerra()
            {
                for (int r = 0; r < nSize; r++)
                    for (int c = 0; c < nSize; c++)
                        this.nSlice[r, c] = '.';
            }
        }


        static void Main(string[] args)
        {
            bool bStop = false;

            StreamReader file = new StreamReader(@".\data.txt");
            string line = file.ReadLine();


            Level Terra    = new Level(nSize);
            Level TerraNew = new Level(nSize);


            TerraNew.EmptyTerra();


            int nRowNumber = 0;
            while (line != null)
            {
                int nColNumber = 0;
                foreach (char c in line)
                {
                    Terra.nSlice[nRowNumber, nColNumber] = c;
                    nColNumber++;
                }

                nRowNumber++;
                line = file.ReadLine();
            }

            Int64 nBioDiversity = 0;
            List<Int64> lBioDiversity = new List<Int64>();




            while (!bStop)
            {
                int nTerraID = 0;
                //ShowTerra(Terra, );
                //Console.ReadKey();
                nBioDiversity = GetBiodiversity(Terra, nTerraID);
                Int64 x = lBioDiversity.FindIndex(n => n == nBioDiversity); 
                if (x>=0)
                    {
                        Console.WriteLine("Part ONE: {0}", nBioDiversity);
                        bStop = true;
                    }

                lBioDiversity.Add(nBioDiversity);


                for (int r = 0; r < nSize; r++)
                    for (int c = 0; c < nSize; c++)
                    {
                        int nNeighbors = 0;

                        if (CheckNeighbors(r + 1, c + 0, Terra )) nNeighbors++;
                        if (CheckNeighbors(r + 0, c + 1, Terra )) nNeighbors++;
                        if (CheckNeighbors(r + 0, c - 1, Terra )) nNeighbors++;
                        if (CheckNeighbors(r - 1, c + 0, Terra )) nNeighbors++;

                        TerraNew.nSlice[r, c] = Terra.nSlice[r, c];

                        if (nNeighbors != 1 && Terra.nSlice[r, c] == '#')
                            TerraNew.nSlice[r, c] = '.';

                        if ((nNeighbors == 1 || nNeighbors == 2) && Terra.nSlice[r, c] == '.')
                            TerraNew.nSlice[r, c] = '#';
                    }

                Terra.CopyTerra(TerraNew);
                
            }
            Console.WriteLine("Press any key");
            Console.ReadKey();
        }


        private static bool CheckNeighbors(int r, int c, Level Terra )
        {
            bool bResult = false;
            if (IsValidCoordinate(r, c))
            {
                
                // it is center - need to check outer level
                if(r==2 && c==2)
                {

                }


                if (Terra.nSlice[r, c] == '#')
                    bResult = true;

            }
            else
                bResult = false;

            return bResult;
        }

        private static Int64 GetBiodiversity(Level Terra, int nTerraID =0 )
        {
            Int64 Result =0;

            for (int r = 0; r < nSize; r++)
                for (int c = 0; c < nSize; c++)
                    if (Terra.nSlice[ r, c] == '#')
                        Result += (Int64)Math.Pow(2, r * nSize + c);

            return Result;
        }


        private static bool IsValidCoordinate(int x, int y)
        {
            if (x >= 0 && x < nSize && y >= 0 && y < nSize)
                return true;
            else
                return false;
        }




        private static void ShowTerra(char[,] Terra)
        {
            Console.SetCursorPosition(0, 0);
            for (int r = 0; r < nSize ; r++)
            {
                for (int c = 0; c < nSize; c++)
                    Console.Write(Terra[r, c]);

                Console.WriteLine();
            }
        }



    }
}
