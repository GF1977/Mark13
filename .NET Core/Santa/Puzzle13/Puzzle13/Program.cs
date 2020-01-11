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
                this.x      = ArComm[0];
                this.y      = ArComm[1];
                this.type   = ArComm[2];
            }
        }

    static void Main(string[] args)
        {
            // Standard part - read the input data
            StreamReader file = new StreamReader(@".\data.txt");
            string line = file.ReadLine();
            string[] words = line.Split(',');

            // Just for a case - this set of commands is not touchable
            List<Int64> commands_vanile = new List<Int64>();
            foreach (string word in words)  commands_vanile.Add(Int64.Parse(word));

            // Extending the programm area by 10 bytes
            for(int i = 0; i < 100; i++)    commands_vanile.Add(0);

            List<Int64> commands = new List<Int64>(commands_vanile); // Work set of commands

            List<ArcadeCommands> ArCommands = new List<ArcadeCommands>();
            commands[0] = 2; // Part two: Memory address 0 represents the number of quarters that have been inserted; set it to 2 to play for free

            Int64 nStep = 0;
            Int64[] res;
            Int64 nJoystickPosition = 0;
            int[] nArComRaw = new int[3]; // array for the struct constructor 
            int nArcadeOutputCount = 0; // max = 3
            do
            {
                    TheCommand myCommand = new TheCommand(nStep, ref commands);
                    res = myCommand.ExecuteOneCommand(nStep, nJoystickPosition, commands);
                    nStep = res[1];
                    if (res[0] > Int64.MinValue)
                    {
                        nArComRaw[nArcadeOutputCount++] = (int)res[0];
                        if (nArcadeOutputCount == 3)
                        {
                            nArcadeOutputCount = 0;
                            ArcadeCommands LastArcadeCommand = new ArcadeCommands(nArComRaw);
                            ArCommands.Add(LastArcadeCommand);
                            if (LastArcadeCommand.type == 4) // 4 means BALL
                            {
                                //If ball is to the right of the paddle, move right. If ball is left, move left.

                                // If the joystick is in the neutral position, provide 0.
                                // If the joystick is tilted to the left, provide -1.
                                // If the joystick is tilted to the right, provide 1.

                                ArcadeCommands Paddle   = ArCommands.FindLast(n => n.type == 3);
                                ArcadeCommands Ball     = ArCommands.FindLast(n => n.type == 4);
                                if (Paddle.type != 0 && Ball.type != 0 )
                                {
                                    if (Ball.x >  Paddle.x)   nJoystickPosition = 1;
                                    if (Ball.x <  Paddle.x)   nJoystickPosition = -1;
                                    if (Ball.x == Paddle.x)   nJoystickPosition = 0;
                                }
                            }
                        }
                    }
            }
            while (nStep <= commands.Count && nStep > 0);

            //How many block (2) tiles are on the screen when the game exits?
            int nCountblock = 0;
            foreach (ArcadeCommands Arcommand in ArCommands)
            {
                    if (Arcommand.type == 2)
                        nCountblock++;
                    // When three output instructions specify X=-1, Y=0,
                    // The third output instruction is not a tile; the value instead specifies the new score to show in the segment display.
                    if (Arcommand.x == -1 && Arcommand.y == 0)
                    {
                        Console.SetCursorPosition(40, 0);
                        Console.WriteLine("Score: {0}", Arcommand.type);
                    }

                    if (Arcommand.x >= 0)
                    {
                        //0 is an empty tile.No game object appears in this tile.
                        //1 is a wall tile.Walls are indestructible barriers.
                        //2 is a block tile.Blocks can be broken by the ball.
                        //3 is a horizontal paddle tile. The paddle is indestructible.
                        //4 is a ball tile.The ball moves diagonally and bounces off objects.
                        char X = ' ';
                        switch (Arcommand.type)
                        {
                            case 0: X = ' '; break;
                            case 1: X = '#'; break;
                            case 2: X = '~'; break;
                            case 3: X = '_'; break; 
                            case 4: X = 'o'; System.Threading.Thread.Sleep(1); break; 

                            default: break;
                        }
                        Console.SetCursorPosition(Arcommand.x, Arcommand.y);
                        Console.Write(X);
                    }
            }
            Console.SetCursorPosition(40, 1);
            Console.WriteLine("Number of blocks: {0}", nCountblock); // The Answer for Part one
            Console.SetCursorPosition(40, 2);
            Console.WriteLine("End of execution.");
            Console.SetCursorPosition(0, 23);
        }
    }
}
