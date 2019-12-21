//ABCDE
// 1002

//DE - two-digit opcode,      02 == opcode 2
// C - mode of 1st parameter,  0 == position mode
// B - mode of 2nd parameter,  1 == immediate mode
// A - mode of 3rd parameter,  0 == position mode,


//  Opcode 3 takes a single integer as input and saves it to the position given by its only parameter.For example, the instruction 3,50 would take an input value and store it at address 50.
//  Opcode 4 outputs the value of its only parameter.For example, the instruction 4,50 would output the value at address 50.



using System;
using System.Collections.Generic;
using System.IO;

namespace Puzzle2
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
            public int command;
            public int Step;
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
                    command = word%100;
                    Step = GetArgNumber(command)+1; // 1 for command
                    if (GetArgNumber(command) == 1)
                    {
                        if (word / 100 > 1) ArgOne.argMode = 1; else ArgOne.argMode = 0;
                        ArgOne.argValue = ReadMemory(nStep + 1, ArgOne.argMode, ref words);
                    }
                    if (GetArgNumber(command) == 3) 
                    {
                        word += 1000000;
                        if (word.ToString()[2] == '1') ArgOne.argMode = 1; else ArgOne.argMode = 0;
                        ArgOne.argValue = ReadMemory(words[nStep+1], ArgOne.argMode, ref words);

                        if (word.ToString()[3] == '1') ArgTwo.argMode = 1; else ArgTwo.argMode = 0;
                        ArgTwo.argValue = ReadMemory(words[nStep + 2], ArgTwo.argMode, ref words);

                        ArgThree.argMode = 1;
                        ArgThree.argValue = ReadMemory(words[nStep + 3], ArgThree.argMode, ref words);
                    }
            }
             
           public int ReadMemory(int mAddress, int mode, ref List<int> words)
            {
                int res = 0;
                if (mode == 1)
                    res = mAddress;
                else
                    res = words[mAddress];

                return res;
            }

            public void WriteMemory(int mAddress, int mvalue, ref List<int> words)
            {
                words[mAddress] = mvalue;
            }

            public void Add(List<int> words)
            {
                int res = ArgOne.argValue + ArgTwo.argValue;
                WriteMemory(ArgThree.argValue, res, ref words);
            }

            public void Multi(List<int> words)
            {
                int res = ArgOne.argValue * ArgTwo.argValue;
                WriteMemory(ArgThree.argValue, res, ref words);
            }

            public void Input(int number, List<int> words)
            {
                WriteMemory(ArgOne.argValue, number, ref words);
            }


            public int Output()
            {
                int nTheValue = ArgOne.argValue;
                Console.WriteLine(nTheValue);
                return nTheValue;
            }


            public int GetArgNumber(int arg)
            {
                int res = 0;
                switch (arg % 100)
                {
                    case  1: res = 3; break;
                    case  2: res = 3; break;
                    case  3: res = 1; break;
                    case  4: res = 1; break;
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

            List<int> commands  = new List<int>();
            List<int> commands2 = new List<int>();
            foreach (string word in words)
            {
                commands.Add(int.Parse(word));
                commands2.Add(int.Parse(word));
            }

            // run the programm..
            // replace position 1 with the value 12 and replace position 2 with the value 2

            int nStep = 0;
            int subFunctionStep = 0;
            bool bError = false;

            while (nStep <= commands2.Count && !bError)
            {
                // step 38
                TheCommand myCommand = new TheCommand(nStep, ref commands2);
                switch (myCommand.command)
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
                nStep += myCommand.Step;
            }

            nStep = 0;
            subFunctionStep = 0;
            bError = false;
            while (nStep <= commands.Count && !bError && false) // remove false
            {
                int command = commands[nStep];

                //ABCDE
                // 1002

                //DE - two-digit opcode,      02 == opcode 2
                // C - mode of 1st parameter,  0 == position mode
                // B - mode of 2nd parameter,  1 == immediate mode
                // A - mode of 3rd parameter,  0 == position mode,
                int[] mode = {0,0,0};
                int     DE = 0;
                int nArguments = 0;
                if (command > 100)
                {
                    DE = command % 100;
                    command = (command - DE) / 100;
                    string sFakeZero = "00";
                    sFakeZero += command.ToString();
                    mode[2] = int.Parse(sFakeZero.Substring(sFakeZero.Length-1, 1));
                    mode[1] = int.Parse(sFakeZero.Substring(sFakeZero.Length-2, 1));
                    mode[0] = int.Parse(sFakeZero.Substring(sFakeZero.Length-3, 1));
                    command = DE;
                }

                switch (command)
                {
                    case 1: // Add
                        if (DE > 0)
                        {
                            int nSource1, nSource2;
                            if (mode[2] == 1)
                                nSource1 = commands[nStep + 1];
                            else
                                nSource1 = commands[commands[nStep + 1]];

                            if (mode[1] == 1)
                                nSource2 = commands[nStep + 2];
                            else
                                nSource2 = commands[commands[nStep + 2]];

                            if (mode[0] == 1)
                            {
                                commands[nStep + 3] = nSource1 + nSource2;
                            }
                            else
                            {
                                 commands[commands[nStep + 3]] = nSource1 + nSource2;
                            }
                        }
                        else
                        {
                             commands[commands[nStep + 3]] = commands[commands[nStep + 1]] + commands[commands[nStep + 2]];
                        }
                        nStep += 4;
                        break;

                    case 2: // Multi
                        if (DE > 0)
                        {
                            int nSource1, nSource2;
                            if (mode[2] == 1)
                                nSource1 = commands[nStep + 1];
                            else
                                nSource1 = commands[commands[nStep + 1]];

                            if (mode[1] == 1)
                                nSource2 = commands[nStep + 2];
                            else
                                nSource2 = commands[commands[nStep + 2]];

                            if (mode[0] == 1)
                            {
                                commands[nStep + 3] = nSource1 * nSource2;
                            }
                            else
                            {
                                commands[commands[nStep + 3]] = nSource1 * nSource2;
                            }
                        }
                        else
                        {
                            commands[commands[nStep + 3]] = commands[commands[nStep + 1]] * commands[commands[nStep + 2]];
                        }
                        nStep += 4;
                        break;

                    case 3: // Input
                        if (mode[2] == 1)
                        {
                            commands[nStep + 1] = nTheValue;
                        }
                        else
                        {
                            commands[commands[nStep + 1]] = nTheValue;
                        }
                        nStep += 2;
                        break;

                    case 4: // Output
                        if (mode[2] == 1)
                        {
                            nTheValue = commands[nStep + 1] ;
                        }
                        else
                        {
                            nTheValue = commands[commands[nStep + 1]];
                        }
                        Console.WriteLine("Test code:  {0}", nTheValue);
                        if (nTheValue != 0)
                        {
                            Console.WriteLine("------------Error------------");
                            Console.WriteLine("nStep: {0}", nStep);
                            Console.WriteLine("subFunctionStep: {0}", subFunctionStep);
                            Console.WriteLine("minidump:");
                            for(int i = subFunctionStep;i<nStep;i++)
                            {
                                Console.WriteLine("{0} = [{1}]", i, commands[i]);
                            }

                        }

                        nStep += 2;
                        subFunctionStep = nStep;
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


            }

            return commands[0];
        }
    }
}
