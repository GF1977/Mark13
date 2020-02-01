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
        static int[,,] Corridor = new int[MAX_X, MAX_Y,2]; // last parameter keeps how much time Droid was there


        static void Main(string[] args)
        {
            StreamReader        file = new StreamReader(@".\data.txt");
            string line     =   file.ReadLine();
            string[] words  =   line.Split(',');

            List<Int64> commands_vanile = new List<Int64>();
            foreach (string word in words)
                commands_vanile.Add(int.Parse(word));

            //for (Int64 ii = 0; ii < 1000; ii++)
            //    commands_vanile.Add(0);

            List<Int64> commands = new List<Int64>(commands_vanile);

            int posX = MAX_X/2;
            int posY = MAX_Y/2;


            for (Int64 x = 0; x < MAX_X; x++)
                for (Int64 y = 0; y < MAX_Y; y++)
                {
                    Corridor[x, y, 0] = 3;  // 3 means unexplored area
                    Corridor[x, y, 1] = 0;  // how much times droid was there
                }
                    Corridor[posX, posY,0] = 1;
                    Corridor[posX, posY,1] = 0;

            Int64 nStep = 0;
            Int64[] res;
            Int64 nDirection = 1;
            Int64 nStatus = 0;
            do
            {
                // Only four movement commands are understood: north (1), south (2), west (3), and east (4).

                if (nStatus == 0 || nStatus == 1 || nStatus == 2)
                {
                    //Console.SetCursorPosition(0, 25);
                    //Console.WriteLine("Step: {0}     north (1), south (2), west (3), and east (4)", nStep);
                    //nDirection = int.Parse(Console.ReadLine());
                    Random chance = new Random();

                    List<int> Map = new List<int>();
                    Map.Add(ReadMap(posX, posY, 1));
                    Map.Add(ReadMap(posX, posY, 2));
                    Map.Add(ReadMap(posX, posY, 3));
                    Map.Add(ReadMap(posX, posY, 4));

                    int nValue = Map.Min(n => n);
                    nDirection = Map.FindIndex(n => n == nValue) + 1;


                    //nDirection = chance.Next() % 4 + 1;
                }

                TheCommand myCommand = new TheCommand(nStep, ref commands);
                res = myCommand.ExecuteOneCommand(nStep, nDirection, commands);
                nStatus = res[0];
                nStep = res[1];

                //0: The repair droid hit a wall. Its position has not changed.
                //1: The repair droid has moved one step in the requested direction.
                //2: The repair droid has moved one step in the requested direction; its new position is the location of the oxygen system.

                if (nStatus == 0)
                {
                    MoveTheRobot(ref posX, ref posY, nDirection);
                    Corridor[posX, posY, 0] = 0;
                    Corridor[posX, posY, 1] = 1000;
                    MoveTheRobot(ref posX, ref posY, nDirection, true );
                }
                if (nStatus == 1)
                {
                    MoveTheRobot(ref posX, ref posY, nDirection);
                    Corridor[posX, posY, 0] = 1;
                    Corridor[posX, posY, 1]++;
                }
                if (nStatus == 2)
                {
                    MoveTheRobot(ref posX, ref posY, nDirection);
                    Corridor[posX, posY, 0] = 2;
                    Corridor[posX, posY, 1]++;
                }
                bool bDebug = false;

                if (bDebug)
                {
                    DrawCorridor();
                    Console.SetCursorPosition(MAX_X/2, MAX_Y/2);
                    Console.Write("X");
                }


                Console.SetCursorPosition(posX, posY);
                Console.Write("D");

            }
            while (true);
        }


        static void DrawCorridor()
        {
                for (int y = 0; y < MAX_Y; y++)
                {
                    Console.SetCursorPosition(0, y);
                    for (int x = 0; x < MAX_X; x++)
                    {
                        if (Corridor[x, y,0] == 0)
                        {
                            Console.Write("#");
                        }
                        if (Corridor[x, y,0] == 1)
                        {
                            Console.Write(".");
                        }
                        if (Corridor[x, y,0] == 2)
                        {
                            Console.Write("O");
                        }
                        if (Corridor[x, y,0] == 3)
                        {
                            Console.Write("?");
                        }
                    }
                }
         }
        // Only four movement commands are understood: north (1), south (2), west (3), and east (4).
        static void MoveTheRobot(ref int X, ref int Y, Int64  direction, bool reverse = false)
        {
            if (reverse)
            {
                switch (direction)
                {
                    case 1: Y++; break;
                    case 2: Y--; break;
                    case 3: X++; break;
                    case 4: X--; break;
                    default: break;
                }
            }
            else
            {
                switch (direction)
                {
                    case 1: Y--; break;
                    case 2: Y++; break;
                    case 3: X--; break;
                    case 4: X++; break;
                    default: break;
                }
            }
        }

        static int ReadMap(int X, int Y, Int64 direction)
        {
            int res = -1;
                switch (direction)
                {
                    case 1: res = Corridor[X    ,Y - 1  , 1] ; break;
                    case 2: res = Corridor[X    ,Y + 1  , 1] ; break;
                    case 3: res = Corridor[X - 1,Y      , 1] ; break;
                    case 4: res = Corridor[X + 1,Y      , 1] ; break;
                    default: break;
                }
            return res;
        }
    }
}
