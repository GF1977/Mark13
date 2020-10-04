using System;
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


    public struct TheMap
    {
        public int nId; // node id
        public char cValue; // node value
        public List<Route> Routes;

        public TheMap(int nId, char cValue)
        {
            this.nId = nId;
            this.cValue = cValue;
            Routes = new List<Route>();
    }
    }

    public struct Route
    {
        public int nId;
        public int nSteps;
        public List<char> Doors;// = new List<char>();

       
        public Route(int nId)
        {
            this.nId = nId;
            nSteps = 0;
            Doors = new List<char>();
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
        private int  nRouteCost;
        private int  nClosestID;
        public bool bVisited;


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
        }


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

    public struct OptimalPath
    {
        public List<Node> Path;
        public int nSteps;
        public string sDoors;
        public string sKeys;
        public int nDoors;
        public int nKeys;
        public char cStart;
        public char cEnd;

        public OptimalPath(List<Node> Res)
            {
                Path = new List<Node>();
                sDoors = "";
                sKeys = "";
                nDoors = 0;
                nKeys = 0;
                nSteps = Res[0].GetRouteCost();
                cStart = Res.First().cValue;
                cEnd = Res.Last().cValue;

                foreach (Node N in Res)
                    {
                        Path.Add(N);
                        if(N.isDoor())
                        {
                            sDoors += N.cValue;
                            nDoors++;
                        }
                        if (N.isKey())
                        {
                            sKeys += N.cValue;
                            nKeys++;
                        }
                    }

                }
    }

    class Program
    {
        //test
        static int nRoomDimensionX;
        static int nRoomDimensionY;
        static char[,] Labirint;
        static List<Node> Nodes = new List<Node>();
        static List<Node> NodesVanila = new List<Node>();
        static int nX = 0;
        static int nY = 0;
        //static int nSteps = 0;
        static int nMinSteps = 0;
        static int nKeysNumber = 0;
        static List<string> myInput = new List<string>();
        static List<TheMap> MapVanile = new List<TheMap>();
        static List<TheMap> Map = new List<TheMap>();
        static OptimalPath[,] CalculatedPaths;
        static String FoundKeys = "";

        //static string sBestPath = "";

        static char[] Directions = { 'W', 'E', 'N', 'S' };
        static void Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine(DateTime.Now);

            StreamReader file = new StreamReader(@".\data2.txt");

            while (!file.EndOfStream)
                myInput.Add(file.ReadLine());

            nRoomDimensionX = myInput[0].Length;
            nRoomDimensionY = myInput.Count;

            Labirint = new char[nRoomDimensionX, nRoomDimensionY];
            LabirintPrefill(myInput);
            ResetLabirint();


            CalculatedPaths = new OptimalPath[Nodes.Count, Nodes.Count];
            CalculateRouteCost();


                    DrawTheMap(ref MapVanile);

            nMinSteps = int.MaxValue;
            List<string> lBestOptions = new List<string>();

            // Part One 
            lBestOptions = RunForestRun("", 12);
            foreach (string S in lBestOptions)
            {
                RunForestRun(S);
            }

            // Part Two
            //lBestOptions = RunForestRun("", 12);
            //               RunForestRun(lBestOptions[0] );

            Console.WriteLine(DateTime.Now);




        }

        private static void ResetLabirint()
        {
            Counter.Reset();
            FoundKeys = "";
            Nodes = new List<Node>();

            foreach (Node N in NodesVanila)
            {
                Node newN = new Node(N);
                Nodes.Add(newN);
            }
        }

        private static void DrawTheMap(ref List<TheMap> Map)
        {
            foreach (Node NodeStart in Nodes)
            if(NodeStart.isKey() || NodeStart.cValue == '@')
            {
                TheMap map = new TheMap(NodeStart.nID,NodeStart.cValue);
                foreach (Node NodeFinish in Nodes)
                if(NodeFinish.isKey())
                {
                    List<Node> Path = new List<Node>();
                    Route route = new Route(0);
                    if (NodeStart.nID > 0 && NodeFinish.nID > 0 &&  NodeFinish.nID != NodeStart.nID)
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
                    if(route.nId>0)
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

        private static List<string>  RunForestRun(string sBegin, int nStepLimit = 0)
        {
            ResetLabirint();
            int nNotificationStep = 10000;
            List<string> lResultat = new List<string>();
            
            string sBestPath = "";
            int nKeysNumberEtalon = 0;
            nMinSteps = int.MaxValue;

            foreach (Node N in Nodes)
                if (N.isKey())
                    nKeysNumberEtalon++;

            if (nStepLimit > 0)
                nKeysNumberEtalon = nStepLimit;



            List<String>[] lOptions = new List<String>[nKeysNumberEtalon+1];
            for (int i = 0; i < nKeysNumberEtalon+1; i++)
                lOptions[i] = new List<String>();

            Node nodeStart;
            Node nodeEnd;

            bool bStart = true;
            while (bStart || lOptions[0].Count > 0)
            {
                bStart = false;
                nKeysNumber = 0;
                string sPath = "";
                ResetLabirint();


                int nSteps = 0;

                //nodeStart = Nodes.Find(n => n.cValue == '@');
                List<Node> nodeStartAll = Nodes.FindAll(n => n.cValue == '@');
                nodeStart = nodeStartAll[0];
                
                while (nKeysNumber < nKeysNumberEtalon)
                    {
                        List<Node> Res = new List<Node>();
                        int nMinSteps = int.MaxValue;

                        int n = 0;
                    if (lOptions[nKeysNumber].Count == 0)
                    {
                        foreach (Node N in nodeStartAll)
                        {
                        nodeStart = N;


                            if (nKeysNumber < sBegin.Length)
                            {
                                lOptions[nKeysNumber].Clear();
                                Node NS = Nodes.Find(n => n.cValue == sBegin[nKeysNumber].ToString()[0]);
                                foreach (Node N2 in nodeStartAll)
                                    if (CalculatedPaths[NS.nID, N2.nID].Path != null)
                                        lOptions[nKeysNumber].Add(sBegin[nKeysNumber].ToString() + ";" + N2.nID.ToString());
                                

                            }

                            else
                                foreach (Node NEnd in Nodes)
                                    if (NEnd.isKey())
                                    {
                                        OptimalPath OP = CalculatedPaths[nodeStart.nID, NEnd.nID];
                                        if (OP.Path != null)
                                        {
                                            int nDoorCount = OP.sDoors.Length;
                                            Res = OP.Path;
                                            if (Res.Count > 1)
                                            {
                                                foreach (char C in OP.sDoors)
                                                    if (FoundKeys.Contains(C.ToString().ToLower()))
                                                        nDoorCount--;


                                                if (nDoorCount == 0 && !lOptions[nKeysNumber].Contains(NEnd.cValue.ToString()) && OP.nSteps < nMinSteps)
                                                {
                                                    nMinSteps = OP.nSteps+60;
                                                    lOptions[nKeysNumber].Add(NEnd.cValue.ToString() + ";" + nodeStart.nID.ToString());
                                                    n++;
                                                }
                                            }
                                        }
                                    }
                        }
                    }

                        string[] split = lOptions[nKeysNumber].First().Split(';');
                        char cValue = split[0].ToCharArray()[0];
                        int nNodeStartNumber = Int32.Parse(split[1]);
                        nodeStart = Nodes.Find(n => n.nID == nNodeStartNumber);
    
                        sPath += cValue;
                        nodeEnd = Nodes.Find(n => n.cValue == cValue);


                        Res = CalculatedPaths[nodeStart.nID, nodeEnd.nID].Path;


                        if (Res.Count > 1)
                        {
                            nKeysNumber++;
                        //Nodes.Find(n => n.cValue == '@').cValue = '.';
                            
                            nodeStart.cValue = '.';
                            FoundKeys += nodeEnd.pickKey();

                            nSteps += CalculatedPaths[nodeStart.nID, nodeEnd.nID].nSteps;
                            nodeStartAll.Find(n => n.nID == nodeStart.nID).nID = nodeEnd.nID;
                            nodeStart = nodeEnd;
                        }
                        else
                            break;


                        if (nKeysNumber == nKeysNumberEtalon)//|| nKeysNumber == nStepLimit)
                        {
                            int j = 1;
                            lOptions[nKeysNumber - j].RemoveAt(0);
                            while (j < lOptions.Count() - 1)
                            {
                                if (lOptions[nKeysNumber - j].Count() == 0)
                                    lOptions[nKeysNumber - j - 1].RemoveAt(0);
                                else
                                    break;
                                j++;
                            }
                        }


                    }
                

                if (nSteps < nMinSteps)
                {
                    sBestPath = sPath;
                    nMinSteps = nSteps;
                    //Console.SetCursorPosition(0, 2);
                    //Console.Write("                                                    ");
                    //Console.SetCursorPosition(0, 2);
                    //Console.WriteLine("Best one: {0} Steps: {1}", sPath, nSteps);


                    if (lResultat.Count > 10)
                        lResultat.RemoveAt(0);

                    lResultat.Add(sPath);

                }

                //if (nNotificationStep == 0)
                //{
                //    Console.SetCursorPosition(0, 3);
                //    Console.Write("                                                   ");
                //    Console.SetCursorPosition(0, 3);
                //    Console.WriteLine("Current:  {0} Steps: {1}", sPath, nSteps);
                //    nNotificationStep = 10000;
                //}
                //nNotificationStep--;



            }
            //Console.SetCursorPosition(0, 5);
            Console.WriteLine("--------------------");
            Console.WriteLine("{0} Steps: {1}", sBestPath, nMinSteps);

            lResultat.Reverse();
            return lResultat;
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
                
                //nSteps += nodeEnd.GetRouteCost();

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
            NodesVanila = new List<Node>();
            NodesVanila.Add(new Node()); // dummy Node into [0]

            // filling the array by data from the input
            for (int y = 0; y < nRoomDimensionY; y++)
                for (int x = 0; x < nRoomDimensionX; x++)
                {
                    Labirint[x, y] = char.Parse(myInput[y].Substring(x, 1));

                    if (Labirint[x, y] != '.' && Labirint[x, y] != '#')
                        NodesVanila.Add(new Node(x, y, Labirint[x, y]));
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

        private static void RunByThePath (string sPath)
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
                GoGoGo(sNodetoNode,0);
            }

            Console.SetCursorPosition(0, nRoomDimensionY + 2);
            Console.WriteLine("                                           ");
            Console.WriteLine("                                           ");
            Console.SetCursorPosition(0, nRoomDimensionY + 2);
            Console.WriteLine("Steps: {0,5}", nodeFinish.GetRouteCost());

        }

        private static int GoGoGo(string sPath, int nSteps)
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
            return nSteps;
        }

        private static void CalculateRouteCost()
        {
            foreach (Node Start in Nodes)
                foreach (Node End in Nodes)
                {
                    if (Start != End && !Start.isTunnel() && !End.isTunnel())
                    {
                        List<Node> Res = GetRoute(Start, End,false);
                        if(Res.Count>1)
                            CalculatedPaths[Start.nID, End.nID] = new OptimalPath(Res);                                           
                    }
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
               // if (Temp.cValue == '@') break;
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
