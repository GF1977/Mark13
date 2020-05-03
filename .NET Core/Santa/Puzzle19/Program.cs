using System;
using System.Collections.Generic;
using System.IO;

namespace Puzzle19
{
    class Program
    {
        static void Main(string[] args)
        {
            Int64 StartValue = 1;    // Part #1 

            StreamReader file = new StreamReader(@".\data.txt");
            string line = file.ReadLine();
            string[] words = line.Split(',');

            List<Int64> commands_vanile = new List<Int64>();
            foreach (string word in words)
                commands_vanile.Add(Int64.Parse(word));

            for (int ii = 0; ii < 1000; ii++)
                commands_vanile.Add(0);

            List<Int64> commands = new List<Int64>(commands_vanile);
        }
    }
}
