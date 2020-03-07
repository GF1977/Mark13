using System;
using System.Collections.Generic;
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
            Nodes.Add(new Node()); // dummy Node into [0]

            Console.WindowHeight = 70;
            Console.WindowWidth = nRoomDimensionX + 50;

            for (int y = 0; y < nRoomDimensionY; y++)
                for (int x = 0; x < nRoomDimensionX; x++)
                {
                    Labirint[x, y] = char.Parse(myInput[y].Substring(x, 1));

                    if (Labirint[x, y] != '.' && Labirint[x, y] != '#')
                        Nodes.Add(new Node(x, y, Labirint[x, y]));
                }

            for (int y = 0; y < nRoomDimensionY; y++)
            {
                for (int x = 0; x < nRoomDimensionX; x++)
                {
                    if (IsIntersection(x, y))
                    {
                        Console.ForegroundColor = System.ConsoleColor.Yellow;
                        Console.Write(Labirint[x, y].ToString());
                        Node C = new Node(x, y, '.');
                        Nodes.Add(C);
                    }
                    else if (Labirint[x, y] != '.' && Labirint[x, y] != '#')
                    {
                        Console.ForegroundColor = System.ConsoleColor.Cyan;
                        Console.Write(Labirint[x, y].ToString());
                    }
                    else
                    {
                        Console.ForegroundColor = System.ConsoleColor.Gray;
                        Console.Write(Labirint[x, y].ToString());
                    }
                    

                }
                Console.WriteLine();
            }
    
            for(int i = 1; i < Nodes.Count;i++)
            {
                Node Ctemp = Nodes[i];
                ExploreNode(ref Ctemp);
                Nodes[i] = Ctemp;
            }

            Console.SetCursorPosition(nRoomDimensionX + 10 , 2);
            Console.Write("Enter the Start  point: ");
            string sFinish = Console.ReadLine();
            Console.SetCursorPosition(nRoomDimensionX + 10, 3);
            Console.Write("Enter the Finish point: ");
            string sStart =  Console.ReadLine();
            Node nodeStart  = Nodes.Find(n => n.cValue == sStart[0]);
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
            Console.WriteLine();
            Console.WriteLine("Steps: {0}", nodeFinish.GetRouteCost());
            Console.WriteLine("Steps: {0}", nSteps);

        }

        //private static string GetShortestWay(Node N, int )

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

        private static List<Node> GetRoute(Node Start, Node End)
        {
            Start.SetRouteCost(0);
            List<Node> NextNodes = new List<Node>();
            NextNodes.Add(Start);
            while (NextNodes.Count>0) //(NextNodes.Count > 0 && !bStop)
            {
                Start = NextNodes[0];
                foreach (KeyValuePair<string,int> Connection in Start.Connection)
                {
                        int nIndex = Nodes.FindIndex(n => n.nID == Connection.Value);
                        int nTotalCost = Connection.Key.Length + Start.GetRouteCost();
                        if (Nodes[nIndex].GetRouteCost() > nTotalCost)
                        {
                            Nodes[nIndex].SetRouteCost(nTotalCost);
                            Nodes[nIndex].SetClosestID(Start.nID);
                        }
                        if(!Nodes[nIndex].isVisited())
                            NextNodes.Add(Nodes[nIndex]);
                }
                Start.Visited(true);
                NextNodes.RemoveAt(0);
            }

            List<Node> Res = new List<Node>();
            Node Temp = End;
            while (!(Temp is null))
            {
                Res.Add(Temp);
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
