using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MyClasses;


// Solution -> Add existing project (MyClasses)
// PuzzleNN -> Add reference (shared project)

namespace Puzzle15
{
    public struct Position
    {
        public int X;
        public int Y;
        public Position(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
        }
        public Position(Position P)
        {
            this.X = P.X;
            this.Y = P.Y;
        }

        public Position GetPosition(int nDirection = 0) //0 return the same position "this"
        {
            Position res = this;
            switch(nDirection)
            {
                case 1: res.Y--; break;
                case 2: res.Y++; break;
                case 3: res.X--; break;
                case 4: res.X++; break;
            }
            return res;
        }
    }

    class Program
    {
        static Int64 nProgrammStep = 0;
        const int MAX_X = 50;
        const int MAX_Y = 50;
        static int[,] Corridor = new int[MAX_X, MAX_Y]; 
        static List<Int64> commands;
        // Only four movement commands are understood: north (1), south (2), west (3), and east (4).

        static void Main(string[] args)
        {

            StreamReader file = new StreamReader(@".\data.txt");
            string line = file.ReadLine();
            string[] words = line.Split(',');

            List<Int64> commands_vanile = new List<Int64>();
            foreach (string word in words)
                commands_vanile.Add(int.Parse(word));

            commands = new List<Int64>(commands_vanile);

            for (Int64 x = 0; x < MAX_X; x++)
                for (Int64 y = 0; y < MAX_Y; y++)
                    Corridor[x, y] = 0;  // 0 means unexplored area

            Console.SetWindowSize(MAX_X+10, MAX_Y+10);
            // Part One
            Position posOxygen= LaunchDroid(MAX_X / 2, MAX_Y / 2);

            // Part two
            Corridor[posOxygen.X, posOxygen.Y] = 99;

            List<Position> RoomsWithOxygen = new List<Position>();
            RoomsWithOxygen.Add(posOxygen);

            int nMinutes = 0;
            while (RoomsWithOxygen.Count>0)
            {
                nMinutes++;
                List<Position> nextRoundOfRooms = new List<Position>();
                foreach (Position room in RoomsWithOxygen)
                {
                    nextRoundOfRooms.AddRange(FillTheNearestSpace(room));
                }
                RoomsWithOxygen = nextRoundOfRooms;
                Console.SetCursorPosition(0, 52);
                Console.WriteLine("Minutes: {0}", nMinutes - 1);
                System.Threading.Thread.Sleep(10);

            }
        }

        static bool CheckTheRoom(Position P)
        {
            if (Corridor[P.X, P.Y] == 900)
            {
                Console.SetCursorPosition(P.X, P.Y);
                Console.Write("o");
                Corridor[P.X, P.Y] = 88; // 88 = oxygen in the room
                return true;
            }

            return false;
        }

        static List<Position> FillTheNearestSpace (Position P)
        {
            List<Position> RoomsWithOxygen = new List<Position>();
            if (Corridor[P.X, P.Y] == 99 || Corridor[P.X, P.Y] == 88)
            {
                for (int i=1;i<=4;i++)
                if (CheckTheRoom(P.GetPosition(i)))
                    RoomsWithOxygen.Add(P.GetPosition(i));
            }
            return RoomsWithOxygen;
        }


        static void WriteMap(ref int posX, ref int posY, int nDirection, Int64 nStatus)
        {
            int nPrevPosX = posX;
            int nPrevPosY = posY;
            switch (nDirection)
                {
                    case 1:  posY --; break;
                    case 2:  posY ++; break;
                    case 3:  posX --; break;
                    case 4:  posX ++; break;
                    default: break;
                }

            Console.SetCursorPosition(posX, posY);
            switch (nStatus)
            {
                case 0:
                Corridor[posX, posY] = 1000;
                Console.Write("#");
                // As the droid hit the wall, we need to move the Pen back to it's previous position
                posX = nPrevPosX;
                posY = nPrevPosY;
                break;

                case 1:
                Corridor[posX, posY] = 1;
                Console.Write(".");
                break;

                case 2:
                Corridor[posX, posY] = 99;
                Console.Write("O");
                break;
            }
        }

        static List<int> ReadMap(int X, int Y)
        {
            List<int> Map = new List<int>();
            Map.Add(Corridor[X, Y - 1]);
            Map.Add(Corridor[X, Y + 1]);
            Map.Add(Corridor[X - 1, Y]);
            Map.Add(Corridor[X + 1, Y]);
            return Map;
        }

        static Int64 MoveDroid(int nDirection)
        {
            // Only four movement commands are understood: north (1), south (2), west (3), and east (4).
            Int64 nStatus = -1;
            while (nStatus != 0 && nStatus != 1 && nStatus != 2)
            {
                TheCommand myCommand = new TheCommand(nProgrammStep, ref commands);
                Int64[] res = myCommand.ExecuteOneCommand(nProgrammStep, nDirection, commands);
                nStatus         = res[0];
                nProgrammStep   = res[1];

                //0: The repair droid hit a wall. Its position has not changed.
                //1: The repair droid has moved one step in the requested direction.
                //2: The repair droid has moved one step in the requested direction; its new position is the location of the oxygen system.
            }


            return nStatus;
        }

        static Position LaunchDroid(int posX, int posY)
        {
            Position posOxygen = new Position(-1,-1); 
            Int64 nStatus = -1;
            int nDirection = 0;
            int nMovemenet = 0;
            while (true) 
            {
                List<int> Map = new List<int>();
                Map = ReadMap(posX,posY);
                int nDeadEnd = 0; 
                foreach (int n in Map)
                    nDeadEnd += n;

                // Wall     = 1000
                // Deadend  = 900
                // if the sum of "n" >= 2700 it means there is only one exit (deadend)
                // wall + wall + wall = 3000
                // deadend * 3 = 2700
                if (nDeadEnd >= 2700) 
                {
                    Corridor[posX, posY] = 900; 
                    nMovemenet -= 2;
                }

                if (nDeadEnd >= 3600) // surrounded by deadends or||and walls
                {
                    Console.SetCursorPosition(0, 53);
                    break;
                }


                nDirection = Map.FindIndex(n => n == Map.Min(n => n)) + 1;

                nStatus = MoveDroid(nDirection);
                WriteMap(ref posX, ref posY, nDirection, nStatus);

                if (nStatus == 1)
                    nMovemenet++;

                if (nStatus == 2)
                {
                    nMovemenet++;
                    posOxygen = new Position(posX, posY);
                    Console.SetCursorPosition(0, 50);
                    Console.WriteLine("Oxygen: [{0}:{1}] Steps = {2}", posX, posY, nMovemenet);
                }

            }
            return posOxygen;

        }
    }
}
