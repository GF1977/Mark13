using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Puzzle18
{

    public static class Counter
    {
    private static int counter = 0;
    public static int New
        {
            get { return counter++; }
        }
    }

    public class Object
    {
        public int nID;
        public int X;
        public int Y;
        public Dictionary<string, int> Connection;
        public bool[] bExplored;
        public char cValue;
        public bool bOpened;
        private int  nRouteCost;
        private int  nClosestID;
        public bool bVisited;


        public Object()
        {
            nID = Counter.New;
            this.X = 0;
            this.Y = 0;
            this.bVisited = false;
            this.nRouteCost = 0;
            this.nClosestID = -1;

            Connection = new Dictionary<string, int>();
            bExplored = new bool[4] { false, false, false, false };
            this.cValue = (char)0;

            if (cValue >= 65 && cValue <= 90)
                bOpened = false;
            else
                bOpened = true;
        }

        public Object(int X, int Y, char cValue) : this()
        {

            this.X = X;
            this.Y = Y;
            this.cValue = cValue;
           
        }
        public void SetRouteCost(int nRouteCost)
        {
            this.nRouteCost = nRouteCost;
        }
        public int GetRouteCost()
        {
           return this.nRouteCost;
        }

        public void SetClosestID(int nClosestID)
        {
            this.nClosestID = nClosestID;
        }
        public int GetClosestID()
        {
            return this.nClosestID;
        }

        public void Visited(bool bVisited)
        {
            this.bVisited = bVisited;
        }

        public bool isVisited()
        {
            return this.bVisited;
        }

    }

    class Program
    {
        static int nRoomDimensionX;
        static int nRoomDimensionY;
        static char[,] Labirint;
        static List<Object> Nodes = new List<Object>();
        
        static char[] Directions = { 'W', 'E', 'N', 'S' };
        static void Main(string[] args)
        {
            List<string> myInput = new List<string>();
            StreamReader file = new StreamReader(@".\data.txt");
               
            while( !file.EndOfStream)
                myInput.Add(file.ReadLine());

            nRoomDimensionX = myInput[0].Length;
            nRoomDimensionY = myInput.Count; 

            Labirint = new char[nRoomDimensionX, nRoomDimensionY];
            Nodes.Add(new Object()); // dummy object into [0]


            for (int y = 0; y < nRoomDimensionY; y++)
                for (int x = 0; x < nRoomDimensionX; x++)
                {
                    Labirint[x, y] = char.Parse(myInput[y].Substring(x, 1));

                    if (Labirint[x, y] != '.' && Labirint[x, y] != '#')
                        Nodes.Add(new Object(x, y, Labirint[x, y]));
                }

            for (int y = 0; y < nRoomDimensionY; y++)
            {
                for (int x = 0; x < nRoomDimensionX; x++)
                {
                    if (IsIntersection(x, y))
                    {
                        if (Labirint[x, y] != '.' && Labirint[x, y] != '#')
                        {
                            Console.ForegroundColor = System.ConsoleColor.Red;
                            Console.Write(Labirint[x, y].ToString());
                        }
                        else
                        {
                            Console.ForegroundColor = System.ConsoleColor.Yellow;
                            Console.Write("+");
                        }

                        Console.ForegroundColor = System.ConsoleColor.Gray;
                        Object C = new Object(x,y,'.');
                        Nodes.Add(C);
                    }
                    else
                        Console.Write(Labirint[x, y].ToString());
                }
                Console.WriteLine();
            }
    
            for(int i = 1; i < Nodes.Count;i++)
            {
                Object Ctemp = Nodes[i];
                ExploreNode(ref Ctemp);
                Nodes[i] = Ctemp;
            }

            Console.WriteLine("");
            List<Object> Res = new List<Object>();
            Res = GetRoute(Nodes[1], Nodes[25]);
            foreach (Object O in Res)
            {
                Console.SetCursorPosition(O.X, O.Y);
                Console.ForegroundColor = System.ConsoleColor.Red;
                Console.Write("*");
            }
            Console.SetCursorPosition(0, nRoomDimensionY + 2);
            Console.WriteLine();
            Console.WriteLine("Steps: {0}", Nodes[2].GetRouteCost());


        }

        private static List<Object> GetRoute(Object Start, Object End)
        {
            List <Object> Res = new List<Object>();

            List<Object> NextNodes = new List<Object>();
            NextNodes.Add(Start);
            bool bStop = false;
            while (NextNodes.Count > 0 && !bStop)
            {
                Start = NextNodes[0];
                foreach (KeyValuePair<string,int> Connection in Start.Connection)
                {
                    if (Connection.Value > 0 && !Nodes.Find(n=>n.nID == Connection.Value).isVisited())
                    {
                        int nIndex = Nodes.FindIndex(n => n.nID == Connection.Value);
                        int nTotalCost = Connection.Key.Length + Start.GetRouteCost();
                        if (Nodes[nIndex].GetRouteCost() == 0 || Nodes[nIndex].GetRouteCost() > nTotalCost)
                        {
                            Nodes[nIndex].SetRouteCost(nTotalCost);
                            Nodes[nIndex].SetClosestID(Start.nID);
                        }
                        if(!Nodes[nIndex].isVisited())
                            NextNodes.Add(Nodes[nIndex]);
                    }
                }
                Start.Visited(true);
                NextNodes.RemoveAt(0);
            }

            Object Temp = End;
            while (!(Temp is null))
            {
                Res.Add(Temp);
                Temp = Nodes.Find(n => n.nID == Temp.GetClosestID());
            }

            return Res;
        }

 


        private static Object GetNodebyID(int nID)
        {
            Object res = new Object();
            foreach (Object N in Nodes)
                    if (N.nID == nID)
                        res = N;

            return res;
        }

        private static char ReadRoom(int x, int y)
        {
            if (x >= 0 && x < nRoomDimensionX && y >= 0 && y < nRoomDimensionY)
                return Labirint[x, y];
            else
                return (char)0;
        }

        private static char NextStep(int X, int Y, char cDirection)
        {
            char cNewDirection = '*';

            if (ReadRoom(X - 1, Y) != '#' && cDirection != 'E') cNewDirection = 'W';
            if (ReadRoom(X + 1, Y) != '#' && cDirection != 'W') cNewDirection = 'E';
            if (ReadRoom(X, Y - 1) != '#' && cDirection != 'S') cNewDirection = 'N';
            if (ReadRoom(X, Y + 1) != '#' && cDirection != 'N') cNewDirection = 'S';


            return cNewDirection;
        }

        private static void ExploreNode (ref Object C)
        {
            if (ReadRoom(C.X - 1, C.Y) != '#') C.bExplored[0] = false; else {C.bExplored[0] = true; C.Connection.Add("W", 0); };
            if (ReadRoom(C.X + 1, C.Y) != '#') C.bExplored[1] = false; else {C.bExplored[1] = true; C.Connection.Add("E", 0); };
            if (ReadRoom(C.X, C.Y - 1) != '#') C.bExplored[2] = false; else {C.bExplored[2] = true; C.Connection.Add("N", 0); };
            if (ReadRoom(C.X, C.Y + 1) != '#') C.bExplored[3] = false; else {C.bExplored[3] = true; C.Connection.Add("S", 0); };

            for(int i = 0; i< 4; i++)
            {
                string sPath = "";
                int x = C.X;
                int y = C.Y;

                if (C.bExplored[i] == true) continue;
                
                char cDirection = Directions[i];
                do
                {
                    switch (cDirection)
                    {
                        case 'W': x--; break;
                        case 'E': x++; break;
                        case 'N': y--; break;
                        case 'S': y++; break;
                        default: break;
                    }

                    sPath += cDirection;
                    cDirection = NextStep(x,y, cDirection);

                    //if (cDirection == '*')
                    //{
                    //    C.bExplored[i] = true;
                    //    C.Connection.Add(Directions[i].ToString(), 0);
                    //    //break;
                    //}

                    if(IsIntersection(x, y) || (ReadRoom(x,y)!= '#' && ReadRoom(x,y) != '.'))
                    {
                        C.bExplored[i] = true;
                        int nNextNode = Nodes.Find(node => node.X == x && node.Y == y).nID;
                        C.Connection.Add(sPath, nNextNode);
                        break;
                    }
                }
                while (cDirection != '*');
            }
        }

        private static bool IsIntersection(int x, int y)
        {
            int n = 0;
            if (ReadRoom(x - 1, y) != '#') n++;
            if (ReadRoom(x + 1, y) != '#') n++;
            if (ReadRoom(x, y - 1) != '#') n++;
            if (ReadRoom(x, y + 1) != '#') n++;
                
            if(n>=3 & ReadRoom(x,y) != '#')
                return true;
            else
                return false;
        }


    }
}
