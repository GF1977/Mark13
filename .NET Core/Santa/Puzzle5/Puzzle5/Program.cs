using System;
using System.Collections.Generic;
using System.IO;

namespace Puzzle5
{

    class Program
    {
        static void Main(string[] args)
        {
            int nTheValue = 1;
            RunTheProgramm(nTheValue);
        }
        class TheCommand
        {
            private static int command;
            private static int Step;
            struct myArgument
            {
                public int argValue;
                public int argMode;
            }

            private static myArgument ArgOne;
            private static myArgument ArgTwo;
            private static myArgument ArgThree;
            public TheCommand(int nStep, ref List<int> words)
            {
                int word = words[nStep];
                command = word % 100;
                Step = GetArgNumber(command) + 1; // 1 for command
                word += 1000000;
                if (word.ToString()[4] == '1') ArgOne.argMode = 1; else ArgOne.argMode = 0;
                
                if(command != 99)
                    ArgOne.argValue = ReadMemory(words[nStep + 1], ArgOne.argMode, ref words);

                if (command == 3)
                {
                    ArgOne.argValue = ReadMemory(nStep + 1, ArgOne.argMode, ref words);
                }

                if (GetArgNumber(command) == 3)
                {
                    if (word.ToString()[3] == '1') ArgTwo.argMode = 1; else ArgTwo.argMode = 0;
                    ArgTwo.argValue = ReadMemory(words[nStep + 2], ArgTwo.argMode, ref words);

                    ArgThree.argMode = 1;
                    ArgThree.argValue = ReadMemory(words[nStep + 3], ArgThree.argMode, ref words);
                }
            }

            public int GetCommand() { return command; }
            public int GetStep()    { return Step; }
            public int ReadMemory(int mAddress, int mode, ref List<int> words)
            {
                int res;
                if (mode == 1)            res = mAddress;
                else                      res = words[mAddress];
                return res;
            }
            public void WriteMemory(int mAddress, int mvalue, ref List<int> words)
            {
                words[mAddress] = mvalue;
            }
            public void Add(List<int> words)
            {
                 WriteMemory(ArgThree.argValue, ArgOne.argValue + ArgTwo.argValue, ref words);
            }
            public void Multi(List<int> words)
            {
                WriteMemory(ArgThree.argValue, ArgOne.argValue * ArgTwo.argValue, ref words);
            }
            public void Input(int number, List<int> words)
            {
                WriteMemory(ArgOne.argValue, number, ref words);
            }
            public int Output()
            {
                Console.WriteLine(ArgOne.argValue);
                return ArgOne.argValue;
            }
            public int GetArgNumber(int arg)
            {
                int res = 0;
                switch (arg % 100)
                {
                    case 1: res = 3; break;
                    case 2: res = 3; break;
                    case 3: res = 1; break;
                    case 4: res = 1; break;
                    case 99: res = 0; break;
                }
                return res;
            }
        }
        static Int64 RunTheProgramm(int nTheValue)
        {
            StreamReader file = new StreamReader(@".\data.txt");
            string line = file.ReadLine();
            string[] words = line.Split(',');

            List<int> commands2 = new List<int>();
            foreach (string word in words)
            {
                commands2.Add(int.Parse(word));
            }

            int nStep = 0;
            bool bError = false;

            while (nStep <= commands2.Count && !bError)
            {
                TheCommand myCommand = new TheCommand(nStep, ref commands2);
                switch (myCommand.GetCommand())
                {
                    case 1: // Add
                        myCommand.Add(commands2);
                        break;

                    case 2: // Multi
                        myCommand.Multi(commands2);
                        break;

                    case 3: // Input
                        myCommand.Input(nTheValue, commands2);
                        break;

                    case 4: // Output
                        nTheValue = myCommand.Output();
                        break;

                    case 99: // Halt
                        Console.WriteLine("Case 3: Stop");
                        bError = true;
                        break;

                    default:
                        Console.WriteLine("Error");
                        bError = true;
                        break;
                }
                nStep += myCommand.GetStep();
            }
            return 0;
        }
    }
}
