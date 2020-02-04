using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MyClasses;


// Solution -> Add existing project (MyClasses)
// PuzzleNN -> Add reference (shared project)

namespace Puzzle15
{
    class Program
    {
        const int MAX_X = 42;
        const int MAX_Y = 42;
        static int[,,] Corridor = new int[MAX_X, MAX_Y, 2]; // last parameter keeps how much time Droid was there
        static List<Int64> commands;
        static List<string> route;
        // Only four movement commands are understood: north (1), south (2), west (3), and east (4).
        enum direction
        {
            north,
            south,
            west,
            east
        };

        static direction D;

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
                {
                    Corridor[x, y, 0] = 3;  // 3 means unexplored area
                    Corridor[x, y, 1] = 0;  // how much times droid was there
                }

            route = new List<string>();
            LaunchDroid(MAX_X / 2, MAX_Y / 2,false);
        }

        static void DrawCorridor()
        {
            for (int y = 0; y < MAX_Y; y++)
            {
                Console.SetCursorPosition(0, y);
                for (int x = 0; x < MAX_X; x++)
                {
                    if (Corridor[x, y, 0] == 0)
                    {
                        Console.Write("#");
                    }
                    if (Corridor[x, y, 0] == 1)
                    {
                        Console.Write(".");
                    }
                    if (Corridor[x, y, 0] == 2)
                    {
                        Console.Write("O");
                    }
                    if (Corridor[x, y, 0] == 3)
                    {
                        Console.Write("?");
                    }
                }
            }
        }
        static void MoveTheRobot(ref int X, ref int Y, direction D, bool reverse = false)
        {
            if (reverse)
            {
                switch (D)
                {
                    case direction.north: Y++; break;
                    case direction.south: Y--; break;
                    case direction.west: X++; break;
                    case direction.east: X--; break;
                    default: break;
                }
            }
            else
            {
                switch (D)
                {
                    case direction.north:   Y--; break;
                    case direction.south:   Y++; break;
                    case direction.west:    X--; break;
                    case direction.east:    X++; break;
                    default: break;
                }
            }
        }

        static int ReadMap(int X, int Y, direction D)
        {
            int res = -1;
            switch (D)
            {
                case direction.north:   res = Corridor[X, Y - 1, 1]; break;
                case direction.south:   res = Corridor[X, Y + 1, 1]; break;
                case direction.west:    res = Corridor[X - 1, Y, 1]; break;
                case direction.east:    res = Corridor[X + 1, Y, 1]; break;
                default: break;
            }
            return res;
        }

        static bool IsDeadEnd (int posX, int posY)
        {
            if ((ReadMap(posX, posY, direction.north)  + ReadMap(posX, posY, direction.south) + ReadMap(posX, posY, direction.west) + ReadMap(posX, posY, direction.east)) >= 300)
                return true;
            else
                return false;
        }

        static void LaunchDroid(int posX, int posY, bool bIgnoreDeadEnds)
        {
            Int64 nProgrammStep = 0;
            Int64 nMovemenet = 0;
            Int64[] res;
            Int64 nDirection = 1;
            Int64 nStatus = 0;
            //int posX = MAX_X / 2;
            //int posY = MAX_Y / 2;
            int nPrevposX = posX;
            int nPrevposY = posY;
            do
            {
                // Only four movement commands are understood: north (1), south (2), west (3), and east (4).
                if (nStatus == 0 || nStatus == 1 || nStatus == 2)
                {
                    bool bDebug = false;
                    if (bDebug)
                    {
                        DrawCorridor();
                        Console.SetCursorPosition(MAX_X / 2, MAX_Y / 2);
                        Console.Write("S");
                    }

                    Console.SetCursorPosition(posX, posY);
                    if (IsDeadEnd(posX, posY) && !bIgnoreDeadEnds)
                    {
                        Corridor[posX, posY, 1] = 100;
                        Corridor[posX, posY, 0] = 0;
                        Console.Write("#");
                        nMovemenet-=2;
                    }
                    else
                    {
                     
                        Console.Write("*");
                    }


                    List<int> Map = new List<int>();
                    Map.Add(ReadMap(posX, posY, direction.north));
                    Map.Add(ReadMap(posX, posY, direction.south));
                    Map.Add(ReadMap(posX, posY, direction.west));
                    Map.Add(ReadMap(posX, posY, direction.east));

                    int nValue = Map.Min(n => n);
                    nDirection = Map.FindIndex(n => n == nValue)+1;
                    switch (nDirection)
                    {
                        case 1: D = direction.north; break;
                        case 2: D = direction.south; break;
                        case 3: D = direction.west; break;
                        case 4: D = direction.east; break;
                        default: break;
                    }
                }

                TheCommand myCommand = new TheCommand(nProgrammStep, ref commands);
                res = myCommand.ExecuteOneCommand(nProgrammStep, nDirection, commands);
                nStatus = res[0];
                nProgrammStep = res[1];

                //0: The repair droid hit a wall. Its position has not changed.
                //1: The repair droid has moved one step in the requested direction.
                //2: The repair droid has moved one step in the requested direction; its new position is the location of the oxygen system.

                if (nStatus == 0)
                {
                    if (nMovemenet > 155)
                        nStatus = 0;


                    MoveTheRobot(ref posX, ref posY, D);
                    Corridor[posX, posY, 0] = 0;
                    Corridor[posX, posY, 1] = 100; 
                    MoveTheRobot(ref posX, ref posY, D, true);

                }
                if (nStatus == 1)
                {
                    MoveTheRobot(ref posX, ref posY, D);
                    Corridor[posX, posY, 0] = 1;
                    Corridor[posX, posY, 1] = 1;
                    nMovemenet++;
                }
                if (nStatus == 2)
                {
                    MoveTheRobot(ref posX, ref posY, D);
                    Corridor[posX, posY, 0] = 2;
                    Corridor[posX, posY, 1] = 0;
                    Console.SetCursorPosition(posX, posY);
                    Console.Write("O");
                    nMovemenet++;

                    Console.SetCursorPosition(0, 50);
                    Console.WriteLine("Oxygen: [{0}:{1}]   - Steps:{2} ", posX, posY, nMovemenet);

                    break;

                }

                if (posX == MAX_X / 2 && posY == MAX_Y / 2)
                {
                    Console.SetCursorPosition(posX, posY);
                    Console.Write("S");

                    Console.SetCursorPosition(0, 49);
                    Console.WriteLine("Start point: [{0}:{1}]   - Steps:{2} ", posX, posY, nMovemenet);
                    nMovemenet = 0;
                }

            }
            while (true);
        }
    }
}
