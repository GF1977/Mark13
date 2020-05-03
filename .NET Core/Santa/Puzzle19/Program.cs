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
            int nCount = 0;
            for (int X = 0; X < 50; X++)
                for (int Y = 0; Y < 50; Y++)
                {
                    List<Int64> commands = new List<Int64>(commands_vanile);
                    //Console.SetCursorPosition(Y, X);
                    Int64 nRes = RunTheProgramm(X, Y, commands);
                    if (nRes == 0) //the drone is stationary (0)
                    {
                        //Console.Write(".");
                    }
                    else if (nRes == 1) //the drone is being pulled by something (1)
                    {
                        //Console.Write("#");
                        nCount++;
                    }
                }
            Console.WriteLine("Result: {0}", nCount);

            // Puzle #2
            bool bStop = false;
            for (int X = 1060; X < 10000 && !bStop; X+=1)
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
                        Console.WriteLine("X = {0}      Y = {1} ", X, Y);
                        bStop = true;
                        break;
                    }
                }

            }



            //    for (int X = 9000; X < 10000; X += 1)
            //{
            //    List<Int64> commands1 = new List<Int64>(commands_vanile);
            //    Int64 nRes1 = RunTheProgramm(X, X, commands1);

            //    List<Int64> commands2 = new List<Int64>(commands_vanile);
            //    Int64 nRes2 = RunTheProgramm(X+99, X, commands2);

            //    List<Int64> commands3 = new List<Int64>(commands_vanile);
            //    Int64 nRes3 = RunTheProgramm(X, X+99, commands3);

            //    List<Int64> commands4 = new List<Int64>(commands_vanile);
            //    Int64 nRes4 = RunTheProgramm(X+99, X+99, commands4);


            //    if (nRes1 == 1 && nRes2 == 1 && nRes3 == 1 && nRes4 == 1)
            //    {
            //        Console.WriteLine("X = {0}      Y = {1} ", X, X);
            //    }
            //}
        }
    }
}
