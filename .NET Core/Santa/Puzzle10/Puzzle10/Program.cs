using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Puzzle10
{
    class Program
    {

        static int nX_Len;
        static int nY_Len;

        static int nBase = 1;

        static int nX_Laser = 8;
        static int nY_Laser = 16;

        public struct AsteroidDetails
        {
            public Int64 distance;
            public Int64 angle;
            public bool destroyed;
            public int x;
            public int y;

        }

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

            FindThePlace();
            VaporAsteroids();



        }


        static void VaporAsteroids()
        {

            List<AsteroidDetails> Asteroids = new List<AsteroidDetails>();
            for (int y = 0; y < nY_Len; y++)
                for (int x = 0; x < nX_Len; x++)
                {
                    if (StarMap[x, y] == 1 && !(x==nX_Laser && y==nY_Laser))
                    {
                        FindTheBase();
                    }
                }
            Console.WriteLine("Base  {0}", nBase);

            for (int y = 0; y < nY_Len; y++)
                for (int x = 0; x < nX_Len; x++)
                {
                    int myX = x - nX_Laser;
                    int myY = y - nY_Laser;
                    if (StarMap[x, y] == 1 && !(x == nX_Laser && y == nY_Laser))
                    {
                        AsteroidDetails temp;
                        temp.angle = GetAngle(x, y);
                        temp.distance = GetDistance(x, y);
                        temp.destroyed = false;
                        temp.x = x;
                        temp.y = y;
                        Asteroids.Add(temp);
                        //Console.WriteLine("Asteroid  [{0}][{1}] -  Distance {2} Km             Angle {3}", x, y, temp.distance, temp.angle);
                    }
                }

            Asteroids = Asteroids.OrderBy(i => i.angle).ToList();
            int N = 1;
            AsteroidDetails asteroid = Asteroids[0];
            while (Asteroids.Count > 0)
            {
                if (asteroid.angle == Asteroids.Max(x => x.angle))
                    asteroid = Asteroids[0];

                foreach (AsteroidDetails tempA in Asteroids)
                {
                    if(tempA.angle == asteroid.angle)
                    {
                        if (tempA.distance < asteroid.distance)
                            asteroid = tempA;
                    }
                }
                Asteroids.Remove(asteroid);
                Console.WriteLine("Asteroid #{0}  [{1}][{2}] vaporized", N, asteroid.x, asteroid.y);

                foreach (AsteroidDetails tempA in Asteroids)
                {
                    if (tempA.angle > asteroid.angle)
                    {
                            asteroid = tempA;
                            break;
                    }
                }
                N++;
            }
        }
        
        static void FindTheBase()
        {
            List<int> temp = new List<int>();
            for (int y = 0; y < nY_Len; y++)
                for (int x = 0; x < nX_Len; x++)
                {
                    int myX = x - nX_Laser;
                    int myY = y - nY_Laser;
                    if (StarMap[x, y] == 1 && (myX !=0 && myY != 0))
                    {
                        temp.Add(myY);
                    }
                }
            temp = temp.OrderBy(i=>i).ToList();

            foreach (int x in temp)
            {
                if (nBase % x != 0)
                    nBase *= Math.Abs(x);
            }
          //  Console.WriteLine("Base = {0}", nBase);
        }

        static Int64 GetAngle(int x, int y)
        {
            Int64 step = 1000000000;
            Int64[] f = { -1, -1 };
            int myX = x - nX_Laser;
            int myY = y - nY_Laser;

            if      (myX >= 0 && myY <  0)
                { f[0] = 1; f[1] = step - myX * nBase / myY; }
            
            else if (myX >= 0 && myY == 0)
                { f[0] = 2; f[1] = 0; }
            
            else if (myX >= 0 && myY >  0) 
                { f[0] = 2; f[1] = step - myX * nBase / myY; }
            
            else if (myX <= 0 && myY >  0) 
                { f[0] = 3; f[1] = step - myX * nBase / myY; }
            
            else if (myX <= 0 && myY == 0) 
                { f[0] = 4; f[1] = 0;}
            
            else if (myX <= 0 && myX <  0) 
                { f[0] = 4; f[1] = step - myX * nBase / myY; }
            
            else
                Console.WriteLine("Something wrong: myY = {0}, myX = {1}",myX,myY); //something wrong

            return f[0]* step * step + Math.Abs(f[1]);

            // *(360 / Math.PI);
        }

        static Int64 GetDistance(int x, int y)
        {
            return (nX_Laser - x) * (nX_Laser - x) + (nY_Laser - y) * (nY_Laser - y);
        }

        static void FindThePlace()
        {
            int maxAsteroids = 0;
            int bestX = 0, bestY = 0;
            for (int y = 0; y < nY_Len; y++)
                for (int x = 0; x < nX_Len; x++)
                {
                    int res = -1;
                    if (StarMap[x, y] == 1)
                    {
                        res = GetVisibleAsteroids(x, y);
                        if (res > maxAsteroids)
                        {
                            maxAsteroids = res;
                            bestX = x;
                            bestY = y;
                        }
                    }
                }
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("Station [{0}][{1}] - found {2} asteroids", bestX, bestY, maxAsteroids);

       }

        static int GetVisibleAsteroids(int stationX, int stationY)
        {
            int mapStep = 100;
            List<Int64> visible_asteroids = new List<Int64>();

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
