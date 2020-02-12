using MyClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


// Solution -> Add existing project (MyClasses)
// PuzzleNN -> Add reference (shared project)

namespace Puzzle17
{

    class Program
    {
        static Int64 nProgrammStep = 0;
        static List<Int64> commands;
        const int ROOM_SIZE = 46;

        static void Main(string[] args)
        {

            StreamReader file = new StreamReader(@".\data.txt");
            string line = file.ReadLine();
            string[] words = line.Split(',');

            List<Int64> commands_vanile = new List<Int64>();
            foreach (string word in words)
                commands_vanile.Add(int.Parse(word));

            commands = new List<Int64>(commands_vanile);
            for(int i = 0;i<10000;i++)
                commands.Add(0);

            // Part One

            Int64[,] Room = new Int64[ROOM_SIZE, ROOM_SIZE];

            Int64 nStatus = -1;
            int posX = 0;
            int posY = 0;
            do
            {
                TheCommand myCommand = new TheCommand(nProgrammStep, ref commands);
                Int64[] res = myCommand.ExecuteOneCommand(nProgrammStep, 0, commands);
                nStatus = res[0];
                nProgrammStep = res[1];

                if (nStatus == 10)
                {
                    Console.WriteLine("");
                    posY++;
                    posX = 0;
                }
                else if (nStatus > 20 && nStatus < 128)
                {
                    char C = (char)nStatus;
                    Console.Write(C.ToString());
                    Room[posX, posY] = nStatus;
                    posX++;
                }

                
            }
            while (nProgrammStep != 0);

            Console.WriteLine("");

            int nCalibration = 0;

            for (int x = 0;x< ROOM_SIZE; x++)
                for (int y = 0; y < ROOM_SIZE; y++)
                {
                    bool bIntersection = isIntersection(Room,x,y);
                    if (bIntersection)
                    {
                        //Console.WriteLine("X: {0}     Y: {1}", x, y);
                        Console.SetCursorPosition(x, y);
                        Console.WriteLine("O");
                        nCalibration += x * y;
                    }
                }

            Console.SetCursorPosition(0, 50);
            Console.WriteLine("Part One - Answer: {0}",nCalibration);
        }

        private static bool isIntersection(long[,] room, int x, int y)
        {
            bool res = false;
            Int64 N = 0;
            try
            {
                if (room[x - 1, y] == 35 && room[x + 1, y] == 35 && room[x, y - 1] == 35 && room[x, y + 1] == 35 && room[x, y] == 35)
                    res = true;
            }
            catch
            {
                return false;
            }

            return res;
        }
    }
}
