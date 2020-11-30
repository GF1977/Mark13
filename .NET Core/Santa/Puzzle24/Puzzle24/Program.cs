using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Puzzle24
{
 
    class Program
    {

        public static int nSize = 5;
        public static Level[] AllTerras = new Level[200];

        public class Level
        {
            public char[,] nSlice;
            public Level()
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
            int nLevelID = 0;


            StreamReader file = new StreamReader(@".\data.txt");
            string line = file.ReadLine();

            Level Terra    = new Level();
            Level TerraNew = new Level();
            
            for(int i=0;i<200;i++)
                AllTerras[i] = new Level();
            
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

            AllTerras[100].CopyTerra(Terra);

            AllTerras[99].CopyTerra(TerraNew);
            AllTerras[101].CopyTerra(TerraNew);


            //the main cycle - works till the goal is achieved

            while (!bStop)
            {
                int nTerraID = 100;
                ShowTerra(nTerraID);
               // Console.ReadKey();

                // Part one = What is the biodiversity rating for the first layout that appears twice?
                nBioDiversity = GetBiodiversity(nTerraID);
                Int64 x = lBioDiversity.FindIndex(n => n == nBioDiversity); 
                if (x>=0)
                    {
                        Console.WriteLine("Part ONE: {0}", nBioDiversity);
                        bStop = true;
                    }

                lBioDiversity.Add(nBioDiversity);
                // Part one - end


                while (nTerraID - 100 <= nLevelID)
                {
                    for (int r = 0; r < nSize; r++)
                        for (int c = 0; c < nSize; c++)
                        {
                            int nNeighbors = 0;

                            nNeighbors += CheckNeighbors(r + 1, c + 0,  nTerraID);
                            nNeighbors += CheckNeighbors(r + 0, c + 1,  nTerraID);
                            nNeighbors += CheckNeighbors(r + 0, c - 1,  nTerraID);
                            nNeighbors += CheckNeighbors(r - 1, c + 0,  nTerraID);

                            TerraNew.nSlice[r, c] = AllTerras[nTerraID].nSlice[r, c];

                            if (nNeighbors != 1 && AllTerras[nTerraID].nSlice[r, c] == '#')
                                TerraNew.nSlice[r, c] = '.';

                            if ((nNeighbors == 1 || nNeighbors == 2) && AllTerras[nTerraID].nSlice[r, c] == '.')
                                TerraNew.nSlice[r, c] = '#';
                        }
                    nTerraID++;
                }

                AllTerras[nTerraID - 1].CopyTerra(TerraNew);
                
            }
            Console.WriteLine("Press any key");
            Console.ReadKey();
        }


        private static int CheckNeighbors(int r, int c, int nTerraID)
        {
            int bResult = 0;
            
            if (IsValidCoordinate(r, c))
            {
                // to check inner level  (nTerraID-1)
                if ((r==1 && c==2) || (r == 2 && c == 1) || (r == 2 && c == 3) || (r == 3 && c == 2))
                {
                    if (r == 1 && c == 2)
                        bResult += GetNeighboursCount(nTerraID-1, "top");
                    if (r == 2 && c == 1)
                        bResult += GetNeighboursCount(nTerraID-1, "left");
                    if (r == 2 && c == 3)
                        bResult += GetNeighboursCount(nTerraID-1, "right");
                    if (r == 3 && c == 2)
                        bResult += GetNeighboursCount(nTerraID-1, "down");
                }

                // no connection to recursive levels
                if (AllTerras[nTerraID].nSlice[r, c] == '#')
                    bResult ++;

            }
            else //  to check outer level
            {
                if (r < 0)
                    if(AllTerras[nTerraID+1].nSlice[1, 2] == '#')
                        bResult++;

                if (r >= nSize)
                    if (AllTerras[nTerraID+1].nSlice[3, 2] == '#')
                        bResult++;

                if (c < 0)
                    if (AllTerras[nTerraID+1].nSlice[2, 1] == '#')
                        bResult++;

                if (r >= nSize)
                    if (AllTerras[nTerraID+1].nSlice[2, 3] == '#')
                        bResult++;
            }


            return bResult;
        }

        private static int GetNeighboursCount(int nTerraID, string position)
        {
            int result = 0;

            if(position == "top")
                for (int i = 0; i < nSize; i++)
                    if (AllTerras[nTerraID].nSlice[0, i] == '#') result++;

            if (position == "down")
                for (int i = 0; i < nSize; i++)
                    if (AllTerras[nTerraID].nSlice[4, i] == '#') result++;

            if (position == "left")
                for (int i = 0; i < nSize; i++)
                    if (AllTerras[nTerraID].nSlice[i, 0] == '#') result++;

            if (position == "right")
                for (int i = 0; i < nSize; i++)
                    if (AllTerras[nTerraID].nSlice[i, 4] == '#') result++;


            return result;
        }


        private static Int64 GetBiodiversity(int nTerraID)
        {
            Int64 Result =0;

            for (int r = 0; r < nSize; r++)
                for (int c = 0; c < nSize; c++)
                    if (AllTerras[nTerraID].nSlice[ r, c] == '#')
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




        private static void ShowTerra(int nTerraID)
        {
            Console.SetCursorPosition(0, 0);
            for (int r = 0; r < nSize ; r++)
            {
                for (int c = 0; c < nSize; c++)
                    Console.Write(AllTerras[nTerraID].nSlice[r, c]);

                Console.WriteLine();
            }
        }



    }
}
