using System;
using System.Collections.Generic;
using System.IO;

namespace Puzzle10
{
    class Program
    {

        static int nX_Len;
        static int nY_Len;
        static int[,] StarMap;

        static void Main(string[] args)
        {

            var reader = new StreamReader(@".\data.txt");
            string rawdata = reader.ReadToEnd();
            string[] multiLineData = rawdata.Split("\r\n");

            nX_Len = multiLineData[0].Length;
            nY_Len = multiLineData.Length;


            StarMap = new int[nX_Len, nY_Len];

            for (int y = 0; y < nY_Len; y++)
                for (int x = 0; x < nX_Len; x++)
                { 
                    if(multiLineData[y][x] == 46)
                        StarMap[x, y] = 0;
                    else
                        StarMap[x, y] = 1;
                }

            int maxAsteroids=0;
            int bestX =0, bestY=0;
            for (int y = 0; y < nY_Len; y++)
                for (int x = 0; x < nX_Len; x++)
                {
                    int res = -1;
                    if (StarMap[x, y] == 1)
                    {
                        res = GetVisibleAsteroids(x, y);
                        if (res>maxAsteroids)
                        {
                            maxAsteroids = res;
                            bestX = x;
                            bestY = y;
                        }
                        //Console.WriteLine("Station [{0}][{1}] - found {2} asteroids", x, y, res);
                    }
                }
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("Station [{0}][{1}] - found {2} asteroids", bestX, bestY, maxAsteroids);

            

        }

        static int GetVisibleAsteroids(int stationX, int stationY)
        {
            int mapStep = 1000;
            List<Int64> visible_asteroids = new List<Int64>();
            if (stationX == 3 & stationY == 2)
            {
                int a = 0;
            }

            for (int y = 0; y < nY_Len; y++)
                for (int x = 0; x < nX_Len; x++)
                {
                    if (StarMap[x, y] == 1 && !(x== stationX && y == stationY))
                    {
                        int[] pair = { 0, 0 };
                        pair[0] = stationX - x;
                        pair[1] = stationY - y;
                        
                        int[] res = GetMinPair(pair);
                        Int64 coordinates = -1;

                        coordinates = ((mapStep + res[0]) * mapStep) + res[1];

                        if (res[0] == 0 && res[1] > 0)
                            coordinates = 1;
                        if (res[0] == 0 && res[1] < 0)
                            coordinates = 2;
                        if (res[1] == 0 && res[0] > 0)
                            coordinates = 3;
                        if (res[1] == 0 && res[0] < 0)
                            coordinates = 4;

                        

                        if (!visible_asteroids.Exists(x => x == coordinates))
                            visible_asteroids.Add(coordinates);
                    }
                        
                }
            return visible_asteroids.Count;
        }


        private static int[] GetMinPair(int[] pair)
        {
            int[] res = pair;
            int x = 2;
            while (x <= Math.Abs(Math.Min(res[0],res[1])))
            {
                if (res[0] % x == 0 && res[1] % x == 0)
                {
                    res[0] /= x;
                    res[1] /= x;
                    GetMinPair(res);
                }
                x++;
            }

            return res;
        }

    }
}
