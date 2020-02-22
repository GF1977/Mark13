using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Puzzle18
{

    public static class Counter
    {
    private static int counter = 1;
    public static int New
        {
            get { return counter++; }
        }
    }

    struct Crossroads
    {
        public int nID;
        public int X;
        public int Y;
        public List<Crossroads> Nodes;
        public Dictionary<char, int> Connection;
        public bool bExplored;

        public Crossroads(int x, int y)
        {
            nID = Counter.New;
            X = x;
            Y = y;
            Nodes = new List<Crossroads>();
            Connection = new Dictionary<char, int>();
            bExplored = false;
        }
    }

    struct Object
    {
        public int x;
        public int y;
        public char cValue;

        public Object(int x, int y, char cValue)
        {
            this.x = x;
            this.y = y;
            this.cValue = cValue;
        }
    }


    class Program
    {
        static int nRoomDimensionX;
        static int nRoomDimensionY;
        static char[,] Labirint;
        static List<Object> RoomsAndKeys = new List<Object>();
        static List<Crossroads> Nodes = new List<Crossroads>();
        static void Main(string[] args)
        {
            List<string> myInput = new List<string>();
            StreamReader file = new StreamReader(@".\data.txt");
               
            while( !file.EndOfStream)
                myInput.Add(file.ReadLine());

            nRoomDimensionX = myInput.Count;
            nRoomDimensionY = myInput[0].Length;

            Labirint = new char[nRoomDimensionX, nRoomDimensionY];

            for (int x = 0; x < nRoomDimensionX; x++)
                for (int y = 0; y < nRoomDimensionY; y++)
                {
                    Labirint[x, y] = char.Parse(myInput[x].Substring(y, 1));

                    if (Labirint[x, y] != '.' && Labirint[x, y] != '#')
                        RoomsAndKeys.Add(new Object(x, y, Labirint[x, y]));
                }

            for (int x = 0; x < nRoomDimensionX; x++)
            {
                Console.WriteLine();
                for (int y = 0; y < nRoomDimensionY; y++)
                {
                    if (IsIntersection(x, y))
                    {
                        Nodes.Add(new Crossroads(x, y));
                        Console.ForegroundColor = System.ConsoleColor.Red;
                        Console.Write("+");
                        Console.ForegroundColor = System.ConsoleColor.Gray;
                    }
                    else
                        Console.Write(Labirint[x, y].ToString());
                }
            }

        }

        private static char ReadRoom(int x, int y)
        {
            if (x >= 0 && x < nRoomDimensionX && y >= 0 && y < nRoomDimensionY)
                return Labirint[x, y];
            else
                return (char)0;
        }

        private static string WhereTheRoad(int x, int y, string sDirection)
        {
            string res = "";

            if (ReadRoom(x - 1, y) == 35 && sDirection != "E") res = "W";
            else if (ReadRoom(x + 1, y) == 35 && sDirection != "W") res = "E";
            else if (ReadRoom(x, y - 1) == 35 && sDirection != "S") res = "N";
            else if (ReadRoom(x, y + 1) == 35 && sDirection != "N") res = "S";

            return res;
        }

        private static bool IsIntersection(int x, int y)
        {
            int n = 0;
            if (ReadRoom(x - 1, y) == '.') n++;
            if (ReadRoom(x + 1, y) == '.') n++;
            if (ReadRoom(x, y - 1) == '.') n++;
            if (ReadRoom(x, y + 1) == '.') n++;
                
            if(n>=3 & ReadRoom(x,y) == '.')
                return true;
            else
                return false;
        }


    }
}
