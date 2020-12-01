using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Puzzle24
{
     class Program
    {
        public const  int nArraySize = 500;
        public const  int nTerra0    = nArraySize/2;
        
        // Terra size (5x5)
        public static int nSize = 5;
        public static int nCenter = (nSize - 1)/2;

        public static Level[] AllTerras    = new Level[nArraySize];
        public static Level[] AllTerrasNew = new Level[nArraySize];

        public class Level
        {
            public char[,] nSlice;
            public Level()
            {
                nSlice = new char[nSize, nSize];
                this.EmptyTerra();
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

                this.nSlice[nCenter, nCenter] = '?';
            }
        }


        static void Main(string[] args)
        {
            bool bStop = false;
            Level TerraVanilla = new Level();

            StreamReader file = new StreamReader(@".\data.txt");
            string line = file.ReadLine();


            int nRowNumber = 0;
            while (line != null)
            {
                int nColNumber = 0;
                foreach (char c in line)
                {
                    TerraVanilla.nSlice[nRowNumber, nColNumber] = c;
                    nColNumber++;
                }
                nRowNumber++;
                line = file.ReadLine();
            }

            for (int i = 0; i < nArraySize; i++)
            {
                AllTerras[i] = new Level();
                AllTerrasNew[i] = new Level();
            }


            AllTerras[nTerra0].CopyTerra(TerraVanilla);

            Int64 nBioDiversity = 0;
            List<Int64> lBioDiversity = new List<Int64>();

            //the main cycle - works till the goal is achieved

            int nDeltaMax = 1;
            int z = 0;

            bool bPartOneSolved = false;
            AllTerras[nTerra0].nSlice[nCenter, nCenter] = '.';


            while (!bStop)
            {
                int nTerraID = nTerra0;
                //ShowTerra(nTerraID);


                //Console.ReadKey();

                // Part one = What is the biodiversity rating for the first layout that appears twice?
                if (!bPartOneSolved)
                {
                    nBioDiversity = GetBiodiversity(nTerraID);
                    Int64 x = lBioDiversity.FindIndex(n => n == nBioDiversity);
                    if (x >= 0)
                    {
                        Console.WriteLine("Part ONE: {0}", nBioDiversity);
                        bPartOneSolved = true;
                        AllTerras[nTerra0].CopyTerra(TerraVanilla);
                        nDeltaMax = 3;
                        z = 1;
                    }
                    lBioDiversity.Add(nBioDiversity);
                }
                // Part one - end


                // Part two
                if (z - 1 == 200)
                {
                    Console.WriteLine("Part TWO: {0}", GetBugsCount());
                    bStop = true;
                }
                // Part two end


                for (int nDelta = -z; nDelta < nDeltaMax - z;nDelta++)  // trick here is to get consequence -1,0,1    -2,-1,0,1,2   -3,-2,-1,0,1,2,3  etc
                    for (int r = 0; r < nSize; r++)
                        for (int c = 0; c < nSize; c++)
                        {
                            int nNeighbors = 0;

                            nNeighbors += CheckNeighbors(r, c, 'n', nTerraID + nDelta);
                            nNeighbors += CheckNeighbors(r, c, 's', nTerraID + nDelta);
                            nNeighbors += CheckNeighbors(r, c, 'e', nTerraID + nDelta) ;
                            nNeighbors += CheckNeighbors(r, c, 'w', nTerraID + nDelta);

                            AllTerrasNew[nTerraID + nDelta].nSlice[r, c] = AllTerras[nTerraID + nDelta].nSlice[r, c];

                            if (nNeighbors != 1 && AllTerras[nTerraID + nDelta ].nSlice[r, c] == '#')
                                AllTerrasNew[nTerraID + nDelta].nSlice[r, c] = '.';

                            if ((nNeighbors == 1 || nNeighbors == 2) && AllTerras[nTerraID + nDelta].nSlice[r, c] == '.')
                                AllTerrasNew[nTerraID + nDelta].nSlice[r, c] = '#';
                        }

                if (bPartOneSolved)
                {
                    z++;
                    nDeltaMax += 2;
                }

                for (int m = -z; m < z ; m++)
                    AllTerras[nTerraID+m].CopyTerra(AllTerrasNew[nTerraID + m]);


            }
            //Console.WriteLine("Press any key");
            //Console.ReadKey();
        }

        private static int GetBugsCount()
        {
            int nResult = 0;
            for (int m = 0; m < nArraySize; m++)
                for (int r = 0; r < nSize; r++)
                    for (int c = 0; c < nSize; c++)
                        if (AllTerras[m].nSlice[r, c] == '#')
                            nResult++;


           return nResult;
        }

        private static int CheckNeighbors(int r, int c, char direction, int nTerraID)
        {
            int bResult = 0;

            if (direction == 'n') r--;
            if (direction == 's') r++;
            if (direction == 'w') c--;
            if (direction == 'e') c++;


            if (IsValidCoordinate(r, c))
            {
                // Inner level
                if (r == 2 && c == 2) 
                {
                    if (direction == 's')
                        bResult += GetNeighboursCount(nTerraID + 1, 'n');
                    if (direction == 'e')
                        bResult += GetNeighboursCount(nTerraID + 1, 'w');
                    if (direction == 'w')
                        bResult += GetNeighboursCount(nTerraID + 1, 'e');
                    if (direction == 'n')
                        bResult += GetNeighboursCount(nTerraID + 1, 's');
                }

                // no connection to recursive levels
                if (AllTerras[nTerraID].nSlice[r, c] == '#')
                    bResult++;
            }
            //Outer level
            else 
            { 
                if (r < 0       && AllTerras[nTerraID - 1].nSlice[1, 2] == '#')
                        bResult++;

                if (r >= nSize  && AllTerras[nTerraID - 1].nSlice[3, 2] == '#')
                        bResult++;

                if (c < 0       && AllTerras[nTerraID - 1].nSlice[2, 1] == '#')
                        bResult++;

                if (c >= nSize  && AllTerras[nTerraID - 1].nSlice[2, 3] == '#')
                        bResult++;
            }


            return bResult;
        }

        private static int GetNeighboursCount(int nTerraID, char position)
        {
            int result = 0;

            if (position == 'n')
                for (int i = 0; i < nSize; i++)
                    if (AllTerras[nTerraID].nSlice[0, i] == '#') result++;

            if (position == 's')
                for (int i = 0; i < nSize; i++)
                    if (AllTerras[nTerraID].nSlice[4, i] == '#') result++;

            if (position == 'w')
                for (int i = 0; i < nSize; i++)
                    if (AllTerras[nTerraID].nSlice[i, 0] == '#') result++;

            if (position == 'e')
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
            Console.WriteLine("Terra: {0}", nTerraID);
            for (int r = 0; r < nSize ; r++)
            {
                for (int c = 0; c < nSize; c++)
                    Console.Write(AllTerras[nTerraID].nSlice[r, c]);
                Console.WriteLine();
            }
            Console.WriteLine();
        }



    }
}
