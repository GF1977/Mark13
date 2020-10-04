using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace Puzzle20
{
    class Program
    {
        public static class Counter
        {
            private static int counter = 0;
            public static int New
            {
                get { return counter++; }
            }
            public static void Reset()
            {
                counter = 0;
            }
        }

        public class Node
        {
            public int nID;
            public int X;
            public int Y;
            public Dictionary<string, int> Connection;
            public bool[] bExplored;
            public char cValue;
            public char cValueBackup;
            public bool bOpened;
            private int nRouteCost;
            private int nClosestID;
            public bool bVisited;
            public string sGate;


            public Node(Node N)
            {
                this.nID = N.nID;
                this.X = N.X;
                this.Y = N.Y;
                this.Connection = N.Connection;
                this.bExplored = N.bExplored;
                this.cValue = N.cValue;
                this.bOpened = N.bOpened;
                this.nRouteCost = N.nRouteCost;
                this.nClosestID = N.nClosestID;
                this.bVisited = N.bVisited;
                this.sGate = N.sGate;
            }


            public Node()
            {
                nID = Counter.New;
                this.X = 0;
                this.Y = 0;
                this.bVisited = false;
                this.nRouteCost = int.MaxValue;
                this.nClosestID = -1;
                this.sGate = "";

                Connection = new Dictionary<string, int>();
                bExplored = new bool[4] { false, false, false, false };
                this.cValue = (char)0;
            }

            public Node(int X, int Y, char cValue) : this()
            {

                this.X = X;
                this.Y = Y;
                this.cValue = cValue;

                if (cValue >= 65 && cValue <= 90)
                    bOpened = false;
                else
                    bOpened = true;

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

            public void OpenDoor()
            {
                this.bOpened = true;
            }

            public bool isKey()
            {
                if (this.cValue >= 97 && this.cValue <= 122)
                    return true;
                else
                    return false;
            }

            public bool isDoor()
            {
                if (this.cValue >= 65 && this.cValue <= 90)
                    return true;
                else
                    return false;
            }



            public char pickKey()
            {
                char cKey = '.';
                if (this.isKey())
                {
                    cKey = this.cValue;
                    this.cValue = '@';
                    this.cValueBackup = cKey;

                }
                return cKey;
            }


            public bool isTunnel()
            {
                if (this.cValue == '@' || (this.cValue >= 97 && this.cValue <= 122) || (this.cValue >= 65 && this.cValue <= 90))
                    return false;
                else
                    return true;
            }



        }

        static List<Node> Nodes = new List<Node>();
        static List<Node> NodesVanila = new List<Node>();
        static int nRoomDimensionX;
        static int nRoomDimensionY;
        static char[,] Labirint;
        static int nX = 0;
        static int nY = 0;
        static char[] Directions = { 'W', 'E', 'N', 'S' };
        static List<string> myInput = new List<string>();


        static void Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine(DateTime.Now);

            StreamReader file = new StreamReader(@".\data.txt");

            while (!file.EndOfStream)
                myInput.Add(file.ReadLine());

            nRoomDimensionX = myInput[0].Length-1;
            nRoomDimensionY = myInput.Count;

            Labirint = new char[nRoomDimensionX, nRoomDimensionY];
            LabirintPrefill(myInput);

        }



        private static void LabirintPrefill(List<string> myInput)
        {
            Counter.Reset();
            NodesVanila = new List<Node>();
            NodesVanila.Add(new Node()); // dummy Node into [0]

            // filling the array by data from the input
            for (int y = 0; y < nRoomDimensionY; y++)
                for (int x = 0; x < nRoomDimensionX; x++)
                {
                    char cCell = char.Parse(myInput[y].Substring(x, 1));
                    if (cCell == 32) cCell = '#';
                    Labirint[x, y] = cCell;
                }



            for (int y = 0; y < nRoomDimensionY; y++)
                for (int x = 0; x < nRoomDimensionX; x++)
                {
                    char cCenter = Labirint[x, y];
                    char cRight = (char)0;
                    char cDown = (char)0;

                    if (cCenter != '.' && cCenter != '#')
                    {
                        if(x< nRoomDimensionX-1)
                            cRight = Labirint[x + 1, y];

                        if (y < nRoomDimensionY - 1)
                            cDown = Labirint[x, y + 1];

                        if (cRight != '.' && cRight != '#')
                        {
                            Labirint[x + 1, y] = '+';
                            Node N = new Node(x+1, y, Labirint[x+1, y]);
                            N.sGate = cCenter.ToString() + cRight.ToString();
                            NodesVanila.Add(N);
                        }
                        if (cDown != '.' && cDown != '#' && cDown != 0)
                        {
                            int nUporDown = 1;
                            string sGate = cCenter.ToString() + cDown.ToString();
                            if (cCenter == 'Z')
                            {
                                nUporDown = -2;
                                sGate = "ZZ";
                            }

                            Labirint[1, y+nUporDown] = '+';
                            Node N = new Node(x , y + nUporDown, Labirint[x , y + nUporDown]);
                            N.sGate = sGate;
                            NodesVanila.Add(N);
                        }

                    }
                }



            // adding intersections modes
            for (int y = 0; y < nRoomDimensionY; y++)
                for (int x = 0; x < nRoomDimensionX; x++)
                    if (IsIntersection(x, y))
                    {
                        NodesVanila.Add(new Node(x, y, '.'));
                    }

            // Explore each node
            for (int i = 1; i < NodesVanila.Count; i++)
            {
                Node Ctemp = NodesVanila[i];
                ExploreNode(ref Ctemp);
                NodesVanila[i] = Ctemp;
            }
        }

        private static bool IsIntersection(int x, int y)
        {
            int n = 0;
            if (ReadRoom(x - 1, y) != '#') n++;
            if (ReadRoom(x + 1, y) != '#') n++;
            if (ReadRoom(x, y - 1) != '#') n++;
            if (ReadRoom(x, y + 1) != '#') n++;

            if (n >= 3 & ReadRoom(x, y) != '#')
                return true;
            else
                return false;
        }
        private static char ReadRoom(int x, int y)
        {
            if (x >= 0 && x < nRoomDimensionX && y >= 0 && y < nRoomDimensionY)
                return Labirint[x, y];
            else
                return (char)0;
        }
        private static void ExploreNode(ref Node C)
        {
            if (ReadRoom(C.X - 1, C.Y) == '.') C.bExplored[0] = false; else { C.bExplored[0] = true; C.Connection.Add("W", 0); };
            if (ReadRoom(C.X + 1, C.Y) == '.') C.bExplored[1] = false; else { C.bExplored[1] = true; C.Connection.Add("E", 0); };
            if (ReadRoom(C.X, C.Y - 1) == '.') C.bExplored[2] = false; else { C.bExplored[2] = true; C.Connection.Add("N", 0); };
            if (ReadRoom(C.X, C.Y + 1) == '.') C.bExplored[3] = false; else { C.bExplored[3] = true; C.Connection.Add("S", 0); };

            for (int i = 0; i < 4; i++)
            {
                string sPath = "";
                int x = C.X;
                int y = C.Y;

                // if a room is already explored, or it is locked door - skip this one
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
                    cDirection = NextStep(x, y, cDirection);

                    if (IsIntersection(x, y) || (ReadRoom(x, y) != '#' && ReadRoom(x, y) != '.' && ReadRoom(x, y) != '+'))
                    {
                        C.bExplored[i] = true;
                        int nNextNode = NodesVanila.Find(node => node.X == x && node.Y == y).nID;
                        C.Connection.Add(sPath, nNextNode);
                        break;
                    }
                }
                while (cDirection != '*');
            }

            foreach (KeyValuePair<string, int> Connection in C.Connection)
            {
                if (Connection.Value == 0)
                    C.Connection.Remove(Connection.Key);
            }
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

        private static List<Node> GetRoute(Node Start, Node End, bool bCheckDoor = true)
        {
            List<Node> Res = new List<Node>();
            if (Start == End)
                return Res;

            foreach (Node N in Nodes)
            {
                N.SetRouteCost(int.MaxValue);
                N.Visited(false);
                N.SetClosestID(-1);
            }

            Start.SetRouteCost(0);
            List<Node> NextNodes = new List<Node>();
            NextNodes.Add(Start);
            while (NextNodes.Count > 0) //(NextNodes.Count > 0 && !bStop)
            {
                Start = NextNodes[0];
                if (Start.bOpened || !bCheckDoor) // only if it is opened
                    foreach (KeyValuePair<string, int> Connection in Start.Connection)
                    {

                        int nIndex = Nodes.FindIndex(n => n.nID == Connection.Value);
                        int nTotalCost = Connection.Key.Length + Start.GetRouteCost();
                        if (Nodes[nIndex].GetRouteCost() > nTotalCost && (Nodes[nIndex].bOpened || !bCheckDoor))
                        {
                            Nodes[nIndex].SetRouteCost(nTotalCost);
                            Nodes[nIndex].SetClosestID(Start.nID);
                        }
                        if (!Nodes[nIndex].isVisited())
                            NextNodes.Add(Nodes[nIndex]);

                    }
                Start.Visited(true);
                NextNodes.RemoveAt(0);
            }


            Node Temp = End;
            while (!(Temp is null))
            {
                Res.Add(Temp);
                // if (Temp.cValue == '@') break;
                Temp = Nodes.Find(n => n.nID == Temp.GetClosestID());
            }

            return Res;
        }


    }
}
