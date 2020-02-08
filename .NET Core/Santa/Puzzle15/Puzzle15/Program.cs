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
        static Int64 nProgrammStep = 0;
        const int MAX_X = 50;
        const int MAX_Y = 50;
        static int[,,] Corridor = new int[MAX_X, MAX_Y, 1]; 
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
                    Corridor[x, y, 0] = 0;  // 0 means unexplored area

            LaunchDroid(MAX_X / 2, MAX_Y / 2);
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
                Corridor[posX, posY, 0] = 1000;
                Console.Write("#");
                // As the droid hit the wall, we need to move the Pen back to it's previous position
                posX = nPrevPosX;
                posY = nPrevPosY;
                break;

                case 1:
                Corridor[posX, posY, 0] = 1;
                Console.Write(".");
                break;

                case 2:
                Corridor[posX, posY, 0] = 99;
                Console.Write("O");
                break;
            }
        }

        static List<int> ReadMap(int X, int Y)
        {
            List<int> Map = new List<int>();
            Map.Add(Corridor[X, Y - 1, 0]);
            Map.Add(Corridor[X, Y + 1, 0]);
            Map.Add(Corridor[X - 1, Y, 0]);
            Map.Add(Corridor[X + 1, Y, 0]);
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

        static void LaunchDroid(int posX, int posY)
        {
            int[] nOxygenX_Y = { 0, 0 }; 
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
                    Corridor[posX, posY, 0] = 900; 
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
                    Console.SetCursorPosition(0, 50);
                    Console.WriteLine("Oxygen: [{0}:{1}] Steps = {2}", posX, posY, nMovemenet);
                }

            }
        }
    }
}
