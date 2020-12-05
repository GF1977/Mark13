using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
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
            public int nLevel;
            public int X;
            public int Y;
            public Dictionary<string, int> Connection;
            public bool[] bExplored;
            public char cValue;
            private int nRouteCost;
            private int nClosestID;
            public bool bVisited;
            public string sGate;

            public Node(Node N)
            {
                this.nID = N.nID;
                this.nLevel = N.nLevel;
                this.X = N.X;
                this.Y = N.Y;
                this.bExplored = N.bExplored;
                this.cValue = N.cValue;
 
                this.nRouteCost = N.nRouteCost;
                this.nClosestID = N.nClosestID;
                this.bVisited = N.bVisited;
                this.sGate = N.sGate;
                Connection = new Dictionary<string, int>();

                foreach (KeyValuePair<string, int> Connection in N.Connection)
                    this.Connection.Add(Connection.Key,Connection.Value);

            }


            public Node()
            {
                nID = Counter.New;
                this.nLevel = 0;
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
                this.nLevel = 0;
                this.X = X;
                this.Y = Y;
                this.cValue = cValue;
                Connection = new Dictionary<string, int>();

            }

            public Node Clone()
            {
                return (Node)this.MemberwiseClone();
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

            public bool IsVisited()
            {
                return this.bVisited;
            }

            public bool IsInner()
            {
                bool bRes = false;
                if ((this.X > 3 && this.X < nRoomDimensionX - 3) && (this.Y > 3 && this.Y < nRoomDimensionY - 3))
                    bRes = true;

                return bRes;
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


        static void Main()
        {
            Console.Clear();
            Console.WriteLine(DateTime.Now);

            StreamReader file = new StreamReader(@".\data.txt");

            while (!file.EndOfStream)
                myInput.Add(file.ReadLine());

            nRoomDimensionX = myInput[0].Length ;
            nRoomDimensionY = myInput.Count;

            Labirint = new char[nRoomDimensionX, nRoomDimensionY];
            LabirintPrefill(myInput);

            //creating copies of Nodes.



            int nMaxLevel = 15;
            for (int i = 0; i <= nMaxLevel; i++)
            {
                foreach (Node N in NodesVanila)
                {
                    Node X = new Node(N);

                    X.nLevel = i;
                    X.nID = i * 10000 + N.nID;
                    Dictionary<string, int> newConnection = new Dictionary<string, int>();
                    foreach (KeyValuePair<string, int> Connection in N.Connection)
                    {
                        string sKey = Connection.Key;
                        int nValue = Connection.Value;

                        if (Connection.Key.Length == 1) // 1 means it is a teleport
                        {
                            if (X.IsInner())
                                nValue = (i + 1) * 10000 + Connection.Value;
                            else if (!X.IsInner() && X.nLevel != 0)
                                nValue = (i - 1) * 10000 + Connection.Value;
                            else
                                nValue = 0; // outer gate on the0 level
                        }
                        else
                        {
                            sKey = Connection.Key;
                            nValue = Connection.Value + 10000 * X.nLevel;
                        }

                        X.Connection.Remove(sKey);
                        newConnection.Add(sKey, nValue);

                    }

                    if (i < nMaxLevel || !X.IsInner())
                        foreach (KeyValuePair<string, int> Connection in newConnection)
                            X.Connection.Add(Connection.Key, Connection.Value);

                    Nodes.Add(X);
                }
            }




            //foreach (Node N in Nodes)
            //{
            //    Console.WriteLine("Node {0} id {1} connected to:", N.sGate, N.nID);
            //    foreach (KeyValuePair<string, int> Connection in N.Connection)
            //    {
            //        string sGate = Nodes.Find(n => n.nID == Connection.Value).sGate;
            //        Console.WriteLine("- {0} id {1}", sGate, Connection.Value);
            //    }
            //    Console.WriteLine();
            //}



            //foreach (Node N in NodesVanila)
            //Nodes.Add(N);

            Node nStart  = NodesVanila.Find(n => n.sGate == "AA");
            Node nFinish = NodesVanila.Find(n => n.sGate == "ZZ");
            List<Node>  NodesResult = new List<Node>();

            NodesResult = GetRoute(nStart, nFinish);
            Console.WriteLine(NodesResult[0].GetRouteCost().ToString());


            foreach (Node N in NodesResult)
            {
                if(N.cValue == 'G' && Nodes[N.GetClosestID() % 100].cValue == 'G')
                 Console.WriteLine("Walk from {0} to {1} ({2} steps)", N.sGate, Nodes[N.GetClosestID()%100].sGate,N.GetRouteCost());
            }

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


            // Gate recognising
            for (int y = 0; y < nRoomDimensionY; y++)
                for (int x = 0; x < nRoomDimensionX; x++)
                {
                    char cCenter    = Labirint[x, y];
                    char cRight     = (char)0;
                    char cDown      = (char)0;
                    char cLeft      = (char)0;
                    char cUp        = (char)0;

                    if (cCenter == '.')
                    {
                        if (x < nRoomDimensionX - 2)
                        {
                            // Gates on the Right side
                            cRight = Labirint[x + 1, y];
                            if (cRight != '.' && cRight != '#' && cRight != '+')
                            {
                                Node N = new Node(x , y, 'G');
                                N.sGate = cRight.ToString() + Labirint[x + 2, y];
                                Labirint[x + 1  , y] = '#';
                                NodesVanila.Add(N);
                            }
                            
                        }

                        if (x >= 2)
                        {
                            // Gates on the Left side
                            cLeft = Labirint[x - 1, y];
                            if (cLeft != '.' && cLeft != '#' && cLeft != '+')
                            {
                                Node N = new Node(x, y, 'G');
                                N.sGate = Labirint[x - 2, y] + cLeft.ToString();
                                Labirint[x - 1 , y] = '#';
                                NodesVanila.Add(N);
                            }
                        }

                        if (y < nRoomDimensionY - 2)
                        {
                            // Gates on the Top side
                            cUp = Labirint[x, y + 1];
                            if (cUp != '.' && cUp != '#' && cUp != '+')
                            {
                                Node N = new Node(x, y, 'G');
                                N.sGate = cUp.ToString() + Labirint[x, y + 2];
                                Labirint[x, y + 1] = '#';
                                NodesVanila.Add(N);
                            }
                        }
                        if (y >= 2)
                        {
                            // Gates on the Bottom side
                            cDown = Labirint[x, y - 1];
                            if (cDown != '.' && cDown != '#' && cDown != '+')
                            {
                                Node N = new Node(x, y, 'G');
                                N.sGate = Labirint[x, y - 2] + cDown.ToString();
                                Labirint[x, y - 1] = '#';
                                NodesVanila.Add(N);
                            }
                        }
                    }
                }

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


        private static bool IsGate(int x, int y)
        {
            if (NodesVanila.Find(node => node.X == x && node.Y == y) == null)
                return false;

            else
            {
                char cvalue = NodesVanila.Find(node => node.X == x && node.Y == y).cValue;

                if (cvalue == 'G')
                    return true;
                else
                    return false;
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

                    if (IsIntersection(x, y) || IsGate(x,y) || (ReadRoom(x, y) != '#' && ReadRoom(x, y) != '.' )) //&& ReadRoom(x, y) != '+'))
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

            // if this is a Gate
            if (C.cValue == 'G' && C.sGate != "AA" && C.sGate != "ZZ" && C.Connection.Count > 0)
            {
                string cDirToGate = "";
                char cDirection = C.Connection.Last().Key.First();
                if (cDirection == 'S') cDirToGate = "N";
                if (cDirection == 'N') cDirToGate = "S";
                if (cDirection == 'W') cDirToGate = "E";
                if (cDirection == 'E') cDirToGate = "W";

                string sCsGate = C.sGate;
                int nCnID = C.nID;

                int nNextNode = NodesVanila.Find(n => n.sGate == sCsGate && n.nID != nCnID).nID;
                C.Connection.Add(cDirToGate, nNextNode);
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

        private static List<Node> GetRoute(Node Start, Node End)
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
                    foreach (KeyValuePair<string, int> Connection in Start.Connection)
                    {

                        int nIndex = Nodes.FindIndex(n => n.nID == Connection.Value);
                        int nTotalCost = Connection.Key.Length + Start.GetRouteCost();
                        if (nIndex >= 0)
                        if (Nodes[nIndex].GetRouteCost() > nTotalCost)
                        {
                            Nodes[nIndex].SetRouteCost(nTotalCost);
                            Nodes[nIndex].SetClosestID(Start.nID);
                            Nodes[nIndex].Visited(false);
                        }
                        if (nIndex >= 0)
                        if (!Nodes[nIndex].IsVisited())
                            NextNodes.Add(Nodes[nIndex]);

                    }
                Start.Visited(true);
                NextNodes.RemoveAt(0);
                NextNodes.Sort((x,y)=>x.nLevel.CompareTo(y.nLevel));

            }


            Node Temp = Start;
            while (Temp.sGate != "AA")
            {
                Res.Add(Temp);
                // if (Temp.cValue == '@') break;
                Temp = Nodes.Find(n => n.nID == Temp.GetClosestID());
            }
            Res.Add(Temp);

            return Res;
        }


    }
}
