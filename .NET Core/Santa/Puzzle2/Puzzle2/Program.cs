using MyClasses;
using System;
using System.Collections.Generic;
using System.IO;


// https://adventofcode.com/2019/day/2

namespace Puzzle2
{
    class Program
    {
        static void Main(string[] args)
        {
            Int64 StartValue = 1;
            StreamReader file = new StreamReader(@".\data.txt");
            string line = file.ReadLine();
            string[] words = line.Split(',');

            List<Int64> commands_vanile = new List<Int64>();
            foreach (string word in words)
                commands_vanile.Add(Int64.Parse(word));

            List<Int64> commands = new List<Int64>(commands_vanile);


            // Part One
            commands[1] = 12;
            commands[2] = 2;

            TheCommand.RunMyProgramm(commands, StartValue);
            Console.WriteLine("1st part Puzzle: {0}", commands[0]);

            // Part two
            for (int n1 = 0; n1 < 100; n1++)
                for (int n2 = 0; n2 < 100; n2++)
                {
                    commands = new List<Int64>(commands_vanile);
                    commands[1] = n1;
                    commands[2] = n2;
                    TheCommand.RunMyProgramm(commands, StartValue);
                    if (commands[0] == 19690720)
                    {
                        Console.WriteLine("2nd part Puzzle: {0}", 100 * n1 + n2);
                        break;
                    }

                }
        }
    }
}
