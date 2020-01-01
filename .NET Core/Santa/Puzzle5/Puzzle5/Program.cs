using System;
using System.Collections.Generic;
using System.IO;
using MyClasses;

namespace Puzzle5
{

    class Program
    {
        static void Main(string[] args)
        {
            StreamReader file = new StreamReader(@".\data_5_2.txt");
            string line = file.ReadLine();
            string[] words = line.Split(',');

            List<Int64> commands_vanile = new List<Int64>();
            foreach (string word in words)
            {
                commands_vanile.Add(int.Parse(word));
            }
            // part #1
            List<Int64> commands = new List<Int64>(commands_vanile);
            TheCommand.RunMyProgramm(commands, 1);

            // part #2
            commands = new List<Int64>(commands_vanile);
            TheCommand.RunMyProgramm(commands, 5);
        }
    }
}
