using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.ExceptionServices;
using MyClasses;

namespace MyClassTemplate
{
    class Program
    {

        // Part one - 278664
        // Fuel cell
        // mutex
        // cake
        // coin


        static Int64 nProgrammStep = 0;
        static List<Int64> commands;

        static void RunTheProgramm(Int64 nStartValue)
        {
            Int64 nStatus;
            Int64 nDamage = 0;
            do
            {
                TheCommand myCommand = new TheCommand(nProgrammStep, ref commands);

                if (myCommand.GetCommand() == 3)
                {
                    nStartValue = Console.Read();
                }
                if (nStartValue != 13)
                {

                    Int64[] res = myCommand.ExecuteOneCommand(nProgrammStep, nStartValue, commands);
                    nStatus = res[0];
                    nProgrammStep = res[1];


                    if (myCommand.GetCommand() == 4)
                    {
                        nDamage = nStatus;
                    }

                    if (nStatus == 10)
                    {
                        Console.WriteLine("");
                    }
                    else if (nStatus > 20 && nStatus < 128)
                    {
                        Console.Write((char)nStatus);
                    }


                }
            }
            while (nProgrammStep != 0);
            Console.WriteLine(nDamage.ToString());
        }

        static void Main(string[] args)
        {
            Int64 StartValue = 0;    // Part #1 

            StreamReader file = new StreamReader(@".\data.txt");
            string line = file.ReadLine();
            string[] words = line.Split(',');

            List<Int64> commands_vanile = new List<Int64>();
            foreach (string word in words)
                commands_vanile.Add(Int64.Parse(word));

            for (int ii = 0; ii < 1000; ii++)
                commands_vanile.Add(0);

            commands = new List<Int64>(commands_vanile);

            RunTheProgramm(StartValue);
        }





    }
}
