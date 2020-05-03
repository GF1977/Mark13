using System;
using System.Collections.Generic;
using System.IO;
using MyClasses;

namespace MyClassTemplate
{
    class Program
    {

        static Int64 RunTheProgramm(int X, int Y, List<Int64> commands)
        {
            Int64 nRes = -1;
            Int64 nProgrammStep = 0;

            Int64 nStatus;
            Int64 nStartValue = X;
            int nInputvalueCount = 0;
            TheCommand myCommand = new TheCommand();
            do
            {
                myCommand = new TheCommand(nProgrammStep, ref commands);
                if (myCommand.GetCommand() == 3) //it is Input
                {
                    if (nInputvalueCount == 0)
                    {
                        nStartValue = X;
                        nInputvalueCount++;
                    }
                    else
                    {
                        nStartValue = Y;
                        nInputvalueCount--;
                    }
                }

                Int64[] res = myCommand.ExecuteOneCommand(nProgrammStep, nStartValue, commands);
                nStatus = res[0];
                nProgrammStep = res[1];

                if (myCommand.GetCommand() == 4) //it is Output
                {
                    // Console.WriteLine(nStatus.ToString());
                    nRes = nStatus;
                }

            }
            while (nProgrammStep != 0);

            return nRes;
        }

        static void Main(string[] args)
        {
            StreamReader file = new StreamReader(@".\data.txt");
            string line = file.ReadLine();
            string[] words = line.Split(',');

            List<Int64> commands_vanile = new List<Int64>();
            foreach (string word in words)
                commands_vanile.Add(Int64.Parse(word));

            for (int ii = 0; ii < 100; ii++)
                commands_vanile.Add(0);


            // Puzzle #1
            Int64 nRes;
            int nCount = 0;
            int X;
            for (X = 0; X < 50; X++)
                for (int Y = 0; Y < 50; Y++)
                {
                    List<Int64> commands = new List<Int64>(commands_vanile);
                    nRes = RunTheProgramm(X, Y, commands);
                    if (nRes == 1) //the drone is being pulled by something (1)
                        nCount++;
                }
            Console.WriteLine("Part One: {0}", nCount);


            nCount = 0;
            int nTestX = 100;

            X = nTestX;
            int nFirstY = 0;
            nRes=0;
            Int64 nResPrev;
                for (int Y = 0; Y < 2000; Y++)
                {
                    List<Int64> commands = new List<Int64>(commands_vanile);

                    nResPrev = nRes;
                    nRes = RunTheProgramm(X, Y, commands);
                    if (nRes == 1 & nResPrev == 0)
                        nFirstY = Y;


                    if (nRes == 1) //the drone is being pulled by something (1)
                        nCount++;
                }


            // Some magic math to get preliminary start X based on the beam triangle
            X = (nFirstY * 99  + nTestX* 100) /nCount;

            // Puzle #2
            bool bStop = false;
            for (; X < 10000 && !bStop; X+=1)
            {
                Int64 nRes1 = 0;
                int Y = -1;
                while(nRes1 == 0)
                {
                    Y++;
                    List<Int64> commands1 = new List<Int64>(commands_vanile);
                    nRes1 = RunTheProgramm(X, Y, commands1);
                }

                // Good, we have found the leftmost beam edge

                while (nRes1 == 1)
                {
                    Y++;
                    List<Int64> commands1 = new List<Int64>(commands_vanile);
                    nRes1 = RunTheProgramm(X, Y, commands1);

                    List<Int64> commands2 = new List<Int64>(commands_vanile);
                    Int64 nRes2 = RunTheProgramm(X + 99, Y, commands2);

                    List<Int64> commands3 = new List<Int64>(commands_vanile);
                    Int64 nRes3 = RunTheProgramm(X, Y + 99, commands3);

                    List<Int64> commands4 = new List<Int64>(commands_vanile);
                    Int64 nRes4 = RunTheProgramm(X + 99, Y + 99, commands4);


                    if (nRes1 == 1 && nRes2 == 1 && nRes3 == 1 && nRes4 == 1)
                    {
                        Console.WriteLine("Part Two: {0}", X*10000 + Y);
                        bStop = true;
                        break;
                    }
                }

            }
        }
    }
}
