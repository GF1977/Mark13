using System;
using System.Collections.Generic;
using System.IO;
using MyClasses;

    
namespace Puzzle11
{
    class Program
    {
        static void Main(string[] args)
        {
            StreamReader file = new StreamReader(@".\data.txt");
            string line = file.ReadLine();
            string[] words = line.Split(',');

            List<Int64> commands_vanile = new List<Int64>();

            foreach (string word in words)
            {
                commands_vanile.Add(Int64.Parse(word));
            }

            for (int i = 0; i < 1000; i++)
                commands_vanile.Add(0);


            List<Int64> commands = new List<Int64>(commands_vanile);


            Int64 StartValue = 0;
            TheCommand.RunMyProgramm(commands, StartValue, false);

        }
    }
}
