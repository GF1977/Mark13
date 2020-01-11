using System;
using System.Collections.Generic;
using System.IO;
using MyClasses;

namespace Puzzle13
{
    class Program
    {
        public struct ArcadeCommands
        {
            public int x;
            public int y;
            public int type;

            public ArcadeCommands(int[] ArComm)
            {
                this.x = ArComm[0];
                this.y = ArComm[1];
                this.type = ArComm[2];
            }

        }


    static void Main(string[] args)
        {
            Int64 StartValue = 0;
            Int64 nStep = 0;
            Int64[] res;

            StreamReader file = new StreamReader(@".\data.txt");
            string line = file.ReadLine();
            string[] words = line.Split(',');

            List<Int64> commands_vanile = new List<Int64>();
            foreach (string word in words)
                commands_vanile.Add(Int64.Parse(word));

            // Extending the programm area by 10 bytes
            for (int ii = 0; ii < 10; ii++)
                commands_vanile.Add(0);

            List<Int64> commands = new List<Int64>(commands_vanile);

            int nArcadeOutputCount = 0; // max = 3
            int[] nArComRaw = new int[3];
            List<ArcadeCommands> ArCommands = new List<ArcadeCommands>();

            //Output[0] = Output , Output[1] = Step
            do
            {
                TheCommand myCommand = new TheCommand(nStep, ref commands);
                res = myCommand.ExecuteOneCommand(nStep, StartValue, commands);
                if (res[0] >= 0)
                {
                    nArComRaw[nArcadeOutputCount++] = (int)res[0];
                    if (nArcadeOutputCount == 3)
                    {
                        nArcadeOutputCount = 0;
                        ArCommands.Add(new ArcadeCommands(nArComRaw));
                    }
                }
                nStep = res[1];
            }
            while (nStep <= commands.Count && nStep > 0);

            //0 is an empty tile.No game object appears in this tile.
            //1 is a wall tile.Walls are indestructible barriers.
            //2 is a block tile.Blocks can be broken by the ball.
            //3 is a horizontal paddle tile. The paddle is indestructible.
            //4 is a ball tile.The ball moves diagonally and bounces off objects.

            //How many block (2) tiles are on the screen when the game exits?
            int nCount = 0;
            foreach (ArcadeCommands Arcommand in ArCommands)
            {
                if (Arcommand.type == 2 )
                    nCount++;
            }

            Console.WriteLine("Number of blocks: {0}", nCount);
            Console.WriteLine("End of execution.");

        }
    }
}
