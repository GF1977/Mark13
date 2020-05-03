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
            do
            {
                TheCommand myCommand = new TheCommand(nProgrammStep, ref commands);
                if(myCommand.GetCommand()==3) //it is Input
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

            for (int ii = 0; ii < 5000; ii++)
                commands_vanile.Add(0);

            

            int nCount = 0;
            for (int X = 0; X < 10; X++)
                for (int Y = 0; Y < 10; Y++)
                {
                    List<Int64> commands = new List<Int64>(commands_vanile);
                    Console.SetCursorPosition(Y, X);
                    Int64 nRes = RunTheProgramm(X, Y, commands);
                    if ( nRes == 0) //the drone is stationary (0)
                    {
                        Console.Write(".");
                    }
                    else if (nRes == 1) //the drone is being pulled by something (1)
                    {
                        Console.Write("#");
                        nCount++;
                    }
                    else
                        Console.Write(".");



                }

            Console.WriteLine("Result: {0}", nCount);
        }





    }
}
