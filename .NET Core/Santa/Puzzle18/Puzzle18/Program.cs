﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
    public static void Reset()
        {
            counter = 0;
        }
    }

    public class TheMap
    {
        public int nId; // node id
        public List<Route> Routes = new List<Route>();

        public TheMap(int nId)
        {
            this.nId = nId;
        }
    }

    public class Route
    {
        public int nId;
        public int nSteps;
        public List<char> Doors = new List<char>();

       
        public Route(int nId)
        {
            this.nId = nId;
            nSteps = 0;
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
        public bool bOpened;
        private int  nRouteCost;
        private int  nClosestID;
        public bool bVisited;


        public Node()
        {
            nID = Counter.New;
            this.X = 0;
            this.Y = 0;
            this.bVisited = false;
            this.nRouteCost = int.MaxValue;
            this.nClosestID = -1;

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
            if (this.cValue >= 65 && this.cValue <= 90 ) 
                return true;
            else
                return false;
        }



        public void pickKey()
        {
            if (this.isKey())
                this.cValue = '@';

        }


    }

    class Program
    {
        static int nRoomDimensionX;
        static int nRoomDimensionY;
        static char[,] Labirint;
        static List<Node> Nodes = new List<Node>();
        static int nX = 0;
        static int nY = 0;
        static int nSteps = 0;
        static int nKeysNumber = 0;
        static int nAbsoluteMin = int.MaxValue;
        static List<string> myInput = new List<string>();

        static char[] Directions = { 'W', 'E', 'N', 'S' };
        static void Main(string[] args)
        {
            
            StreamReader file = new StreamReader(@".\data.txt");

            while (!file.EndOfStream)
                myInput.Add(file.ReadLine());

            nRoomDimensionX = myInput[0].Length;
            nRoomDimensionY = myInput.Count;

            Labirint = new char[nRoomDimensionX, nRoomDimensionY];

            Console.WindowHeight = 70;
            Console.WindowWidth = nRoomDimensionX + 50;

            LabirintPrefill(myInput);

            //ShowLabirint();
            //FromAtoB();
            //ShowLabirint();
            //FromAtoB();
            //ShowLabirint();

            //RunForestRun();

            List<TheMap> Map = new List<TheMap>();
            DrawTheMap(ref Map);


        }


        private static void DrawTheMap(ref List<TheMap> Map)
        {
            foreach (Node NodeStart in Nodes)
            {
                TheMap map = new TheMap(NodeStart.nID);
                foreach (Node NodeFinish in Nodes)
                {
                    List<Node> Path = new List<Node>();
                    Route route = new Route(0);
                    if (NodeStart.nID > 0 && NodeFinish.nID > 0 && NodeStart.nID != NodeFinish.nID)
                    {
                        route.nId = NodeFinish.nID;
                        Path = GetRoute(NodeStart, NodeFinish, false);
                        if (Path.Count > 1)
                        {
                            route.nSteps = Path[0].GetRouteCost();
                            foreach(Node n in Path)
                            {
                                if(n.isDoor())
                                    route.Doors.Add(n.cValue);
                            }
                        }

                    }
                    map.Routes.Add(route);
                }
                Map.Add(map);
            }
        }


        private static IEnumerable<string> Permutate(string source)
        {
            if (source.Length == 1) return new List<string> { source };

            var permutations = from c in source
                               from p in Permutate(new String(source.Where(x => x != c).ToArray()))
                               select c + p;
            return permutations;
        }

        private static void RunForestRun()
        {
            int nKeysNumberEtalon = 0;
            foreach (Node N in Nodes)
                if (N.isKey())
                    nKeysNumberEtalon++;

            nKeysNumber = nKeysNumberEtalon;
            Node nodeStartOrigin = Nodes.Find(n => n.cValue == '@');
            List<Node> Res = new List<Node>();
            List<Node> RouteOptions = new List<Node>();

            Node nodeEnd = new Node();
            List<int>[] lOptions = new List<int>[31];
            for (int i = 0; i < 31; i++)
                lOptions[i] = new List<int>();

            bool bStop = false;

            // 1 min 18 sec to find 136
            while (!bStop)
            {
                nSteps = 0;
                LabirintPrefill(myInput);
                // a, f, b, j, g, n, h, d, l, o, e, p, c, i, k, m
                //16,9,3,22,4,17,17,23,1
                nKeysNumber = nKeysNumberEtalon;
                Node nodeStart = nodeStartOrigin;

                while (nKeysNumber > 0)
                {

                    if (lOptions[nKeysNumberEtalon - nKeysNumber].Count == 0)
                    {
                        foreach (Node N in Nodes)
                        {
                            if (N.isKey())
                            {
                                Res = GetRoute(nodeStart, N);
                                if (Res.Count > 1 && N.GetRouteCost() > 0)
                                    lOptions[nKeysNumberEtalon - nKeysNumber].Add(N.nID);
                            }
                        }
                    }


                    nodeEnd = Nodes.Find(n=>n.nID == lOptions[(nKeysNumberEtalon - nKeysNumber)].ElementAt(0));
                    Res = GetRoute(nodeStart, nodeEnd);
                    
                    if (Res.Count > 1)
                    {
                        nKeysNumber--;
                        nodeStart = PointToPoint(Res);
                    }
                    else
                        break;

                    
                    if (nSteps >= nAbsoluteMin) break;
                }

                if (nSteps < nAbsoluteMin)
                {
                    nAbsoluteMin = nSteps;
                    
                    int i = 0;
                    string sMessage = "";
                    while(lOptions[i].Count > 0)
                    {
                        Node N = Nodes.Find(n => n.nID == lOptions[i].ElementAt(0));
                        sMessage += Labirint[N.X, N.Y] + " , ";
                        i++;
                    }
                    sMessage += " End";
                    Console.WriteLine("Minimal Steps: {0}", nAbsoluteMin);
                    Console.WriteLine("Keys: {0}", sMessage);

                }

                bStop = !GetNewOrder(ref lOptions);
            }
        }

        private static bool GetNewOrder(ref List<int>[] lOptions)
        {
            bool res = false;
            for (int i = 30; i >= 0; i--)
            {
                if (lOptions[i].Count >= 1)
                { 
                    lOptions[i].RemoveAt(0);
                    if (lOptions[i].Count >= 1)
                    {
                        res = true;
                        break;
                    }
                }
            }

            return res;
            
        }

        private static Node PointToPoint(List<Node> Res)
        {
            Node nodeStart = Res[Res.Count-1];
            Node nodeEnd = Res[0];
            //if (Res.Count > 1 && nodeEnd.isKey())
            {
                nodeStart = nodeEnd;
                //here is the Key
                // Open the relevant door
                Node nodeDoor = Nodes.Find(n => n.cValue.ToString() == nodeStart.cValue.ToString().ToUpper());
                if (!(nodeDoor is null))
                    nodeDoor.OpenDoor();

                //Console.Write("{0}={1}  ", nodeEnd.cValue, nodeEnd.GetRouteCost());
                nSteps += nodeEnd.GetRouteCost();

                // Pick the key
                Nodes.Find(n => n.cValue == '@').cValue = '.';
                nodeEnd.pickKey();
                //nKeysNumber--;

                //nX = nodeEnd.X;
                //nY = nodeEnd.Y;
                //foreach (Node nodeN in Res)
                //{
                //    string sNodetoNode = nodeN.Connection.FirstOrDefault(n => n.Value == nodeN.GetClosestID()).Key;
                //    foreach (KeyValuePair<string, int> Connection in nodeN.Connection)
                //        if (Connection.Value == nodeN.GetClosestID() && sNodetoNode.Length > Connection.Key.Length)
                //            sNodetoNode = Connection.Key;
                //    //ShowLabirint();
                //    //GoGoGo(sNodetoNode);
                //    //System.Threading.Thread.Sleep(50);
                //}
            }

            return nodeStart;
        }

        private static void LabirintPrefill(List<string> myInput)
        {
            Counter.Reset();
            Nodes = new List<Node>();
            Nodes.Add(new Node()); // dummy Node into [0]

            // filling the array by data from the input
            for (int y = 0; y < nRoomDimensionY; y++)
                for (int x = 0; x < nRoomDimensionX; x++)
                {
                    Labirint[x, y] = char.Parse(myInput[y].Substring(x, 1));

                    if (Labirint[x, y] != '.' && Labirint[x, y] != '#')
                        Nodes.Add(new Node(x, y, Labirint[x, y]));
                }

            // adding intersections modes
            for (int y = 0; y < nRoomDimensionY; y++)
                for (int x = 0; x < nRoomDimensionX; x++)
                    if (IsIntersection(x, y))
                    {
                        Nodes.Add(new Node(x, y, '.'));
                    }

            // Explore each node
            for (int i = 1; i < Nodes.Count; i++)
            {
                Node Ctemp = Nodes[i];
                ExploreNode(ref Ctemp);
                Nodes[i] = Ctemp;
            }
        }

        private static void ShowLabirint()
        {
            Console.Clear();
            for (int y = 0; y < nRoomDimensionY; y++)
            {
                for (int x = 0; x < nRoomDimensionX; x++)
                {
                    if (IsIntersection(x, y))
                        Console.ForegroundColor = System.ConsoleColor.Yellow;
                    else if (Labirint[x, y] != '.' && Labirint[x, y] != '#')
                        Console.ForegroundColor = System.ConsoleColor.Cyan;
                    else
                        Console.ForegroundColor = System.ConsoleColor.Gray;

                    Console.Write(Labirint[x, y].ToString());
                }
                Console.WriteLine();
            }
        }

        private static int RunByThePath (string sPath)
        {
            //a, f, b, j, g, n, h, d, l, o, e, p, c, i, k, m
            //c, e, b, f, k, a, g, n, l, d, h, m, j, o, i, p
            //int nSteps = 0;
            //string[] sNodesNames = sPath.Split(", ");
            string[] sNodesNames = new string[sPath.Length];
            for (int ii = 0; ii < sPath.Length; ii++)
                sNodesNames[ii] = sPath[ii].ToString();

            Node nodeStart = Nodes.Find(n => n.cValue == '@');

            int i = 0;
            while(i < sNodesNames.Length)
            {
                Node nodeEnd = Nodes.Find(n => n.cValue.ToString() == sNodesNames[i]);
                string sStartValue = nodeStart.cValue.ToString();
                string sEndValue = nodeEnd.cValue.ToString();
                List<Node> Res = new List<Node>();
                Res = GetRoute(nodeStart, nodeEnd);
                if (Res.Count > 1)
                {
                    Node nodeStartNew = PointToPoint(Res);
                    //Console.WriteLine("From {0} to {1} = {2}", sStartValue, sEndValue, nSteps);
                    nodeStart = nodeStartNew;
                    i++;
                }
                else
                    break;
            }


            return nSteps;
        }


        private static void FromAtoB()
        {
            Console.SetCursorPosition(nRoomDimensionX + 10, 2);
            Console.Write("Enter the Start  point: ");
            string sFinish = Console.ReadLine();
            Console.SetCursorPosition(nRoomDimensionX + 10, 3);
            Console.Write("Enter the Finish point: ");
            string sStart = Console.ReadLine();
            Node nodeStart = Nodes.Find(n => n.cValue == sStart[0]);
            Node nodeFinish = Nodes.Find(n => n.cValue == sFinish[0]);


            List<Node> Res = new List<Node>();
            Res = GetRoute(nodeStart, nodeFinish);
            nX = nodeFinish.X;
            nY = nodeFinish.Y;
            foreach (Node nodeN in Res)
            {
                string sNodetoNode = nodeN.Connection.FirstOrDefault(n => n.Value == nodeN.GetClosestID()).Key;
                foreach (KeyValuePair<string, int> Connection in nodeN.Connection)
                {
                    if (Connection.Value == nodeN.GetClosestID() && sNodetoNode.Length > Connection.Key.Length)
                        sNodetoNode = Connection.Key;
                }
                GoGoGo(sNodetoNode);
            }

            Console.SetCursorPosition(0, nRoomDimensionY + 2);
            Console.WriteLine("                                           ");
            Console.WriteLine("                                           ");
            Console.SetCursorPosition(0, nRoomDimensionY + 2);
            Console.WriteLine("Steps: {0,5}", nodeFinish.GetRouteCost());
            Console.WriteLine("Steps: {0,5}", nSteps); // for test purpose

        }

        private static void GoGoGo(string sPath)
        {
            if (!(sPath is null))
            {
                Console.ForegroundColor = System.ConsoleColor.Red;

                for (int i = 0; i < sPath.Length; i++)
                {
                    char cTemp = Labirint[nX, nY];
                    Console.SetCursorPosition(nX, nY);
                    Console.Write(cTemp);
                    switch (sPath[i])
                    {
                        case 'N': nY--; break;
                        case 'S': nY++; break;
                        case 'W': nX--; break;
                        case 'E': nX++; break;
                        default: break;
                    }
                    System.Threading.Thread.Sleep(10);
                    nSteps++;
                }
                Console.ForegroundColor = System.ConsoleColor.Gray;
            }
            
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
            while (NextNodes.Count>0) //(NextNodes.Count > 0 && !bStop)
            {
                Start = NextNodes[0];
                if(Start.bOpened || !bCheckDoor) // only if it is opened
                foreach (KeyValuePair<string,int> Connection in Start.Connection)
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
                if (Temp.cValue == '@') break;
                Temp = Nodes.Find(n => n.nID == Temp.GetClosestID());
            }

            return Res;
        }

 


        private static Node GetNodebyID(int nID)
        {
            Node res = new Node();
            foreach (Node N in Nodes)
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

        private static void ExploreNode (ref Node C)
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
                    cDirection = NextStep(x,y, cDirection);

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
