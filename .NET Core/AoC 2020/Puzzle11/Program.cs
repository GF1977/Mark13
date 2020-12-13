using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Puzzle24
{

    class Program
    {
        public static int nSize = 0;

        public class Level
        {
            public char[,] nSlice;
            public Level()
            {
                nSlice = new char[nSize, nSize];
            }

            public Level(Level Vanile)
            {
                nSlice = new char[nSize, nSize];
                this.CopyTerra(Vanile);
            }

            public void CopyTerra(Level TerraNew)
            {
                for (int r = 0; r < nSize; r++)
                    for (int c = 0; c < nSize; c++)
                        this.nSlice[r, c] = TerraNew.nSlice[r, c];
            }

            public void ShowTerra()
            {
                Console.SetCursorPosition(0, 0);
                for (int r = 0; r < nSize; r++)
                {
                    for (int c = 0; c < nSize; c++)
                        Console.Write(nSlice[r, c]);

                    Console.WriteLine();
                }
            }

            public int GetOccupiedSeats()
            {
                int nRes = 0;

                for (int r = 0; r < nSize; r++)
                    for (int c = 0; c < nSize; c++)
                        if (nSlice[r, c] == '#')
                            nRes++;

                return nRes;
            }

            // Returns 1 if the first visible seat is occupied (#) , 0 if free (L)
            public int CheckDirection(int r, int c, int nRowModifier, int nColModifier)
            {
                int bRes = 0;  
                for (int i = 1; i < nSize; i++)
                {
                    char FloorStatus = GetNeighbor(r + i * nRowModifier, c + i * nColModifier);
                    if (FloorStatus == 'L')
                    {
                        bRes = 0;
                        break;
                    }
                    if (FloorStatus == '#')
                    {
                        bRes = 1;
                        break;
                    }

                }
                    return bRes;
            }

            public int GetVisibleSeats(int r, int c)
            {
                int nRes = 0;
                for (int i = -1; i <= 1; i++)
                    for (int j = -1; j <= 1; j++)
                        if(!(i == 0 && j == 0))
                            nRes += CheckDirection(r, c, i, j);

                return nRes;
            }

            public bool CheckNeighbor(int r, int c)
            {
                bool bResult = false;
                if (IsValidCoordinate(r, c))
                    if (nSlice[r, c] == '#')
                        bResult = true;

                else
                    bResult = false;

                return bResult;
            }


            public int CheckNeighbor2(int r, int c)
            {
                int nRes = 0;
                for (int i = -1; i <= 1; i++)
                    for (int j = -1; j <= 1; j++)
                        if (!(i == 0 && j == 0))
                            if (IsValidCoordinate(r + i, c + j))
                                if (nSlice[r + i, c + j] == '#')
                                    nRes++;

                return nRes;
            }

            public char GetNeighbor(int r, int c)
            {
                    if (IsValidCoordinate(r, c))
                        return nSlice[r, c];
                    else
                        return 'L'; // if the neighbours invalid (out of array) we consider it as empty sit
                
            }


        }


        static void Main()
        {
            bool bStop = false;

            StreamReader file = new StreamReader(@".\data.txt");
            string line = file.ReadLine();
            nSize = line.Length;

            Level TerraVanile = new Level();

            int nRowNumber = 0;
            while (line != null)
            {
                int nColNumber = 0;
                foreach (char c in line)
                {
                    TerraVanile.nSlice[nRowNumber, nColNumber] = c;
                    nColNumber++;
                }

                nRowNumber++;
                line = file.ReadLine();
            }

            Level Terra = new Level(TerraVanile);
            Level TerraNew = new Level();



            int nOccupiedSeats = -1;
            // PART ONE
            while (!bStop)
            {
                //Terra.ShowTerra();
                //Console.ReadKey();

                for (int r = 0; r < nSize; r++)
                    for (int c = 0; c < nSize; c++)
                    {
                        int nNeighbors = Terra.CheckNeighbor2(r,c);
                        TerraNew.nSlice[r, c] = Terra.nSlice[r, c];

                        if (nNeighbors == 0 && Terra.nSlice[r, c] == 'L')
                            TerraNew.nSlice[r, c] = '#';

                        if (nNeighbors >= 4  && Terra.nSlice[r, c] == '#')
                            TerraNew.nSlice[r, c] = 'L';
                    }

                Terra.CopyTerra(TerraNew);
                if (Terra.GetOccupiedSeats() == nOccupiedSeats)
                    break;

                nOccupiedSeats = Terra.GetOccupiedSeats();
            }
            Console.WriteLine("Part one: {0}", Terra.GetOccupiedSeats());


            // PART TWO
            Terra = new Level(TerraVanile);
            //TerraNew = new Level();

            nOccupiedSeats = -1;
            while (!bStop)
            {
                //Terra.ShowTerra();
                //Console.ReadKey();

                for (int r = 0; r < nSize; r++)
                    for (int c = 0; c < nSize; c++)
                    {
                        int nVisibleNeighbors = Terra.GetVisibleSeats(r,c);
                        TerraNew.nSlice[r, c] = Terra.nSlice[r, c];
                        if (nVisibleNeighbors == 0  && Terra.nSlice[r, c] == 'L')
                            TerraNew.nSlice[r, c] = '#';

                        if (nVisibleNeighbors >= 5 && Terra.nSlice[r, c] == '#')
                            TerraNew.nSlice[r, c] = 'L';
                    }

                Terra.CopyTerra(TerraNew);
                if (Terra.GetOccupiedSeats() == nOccupiedSeats)
                    break;

                nOccupiedSeats = Terra.GetOccupiedSeats();
            }
            Console.WriteLine("Part two: {0}", Terra.GetOccupiedSeats());


        }






        public static bool IsValidCoordinate(int x, int y)
        {
            if (x >= 0 && x < nSize && y >= 0 && y < nSize)
                return true;
            else
                return false;
        }








    }
}