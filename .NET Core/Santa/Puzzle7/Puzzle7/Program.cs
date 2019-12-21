using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Puzzle7
{

    class Program
    {
        static void Main(string[] args)
        {
            RunTheProgramm();
        }


        private static IEnumerable<string> Permutate(string source)
        {
            if (source.Length == 1) return new List<string> { source };

            var permutations = from c in source
                               from p in Permutate(new String(source.Where(x => x != c).ToArray()))
                               select c + p;
            return permutations;
        }

        static int RunMyProgramm(List<int> commands2, int[] InputValue)
        {
            int i = 0;
            int nStep = 0;
            bool bError = false;
            int Output=-1;
            while (nStep <= commands2.Count && !bError)
            {
                TheCommand myCommand = new TheCommand(nStep, ref commands2);
                //Console.WriteLine("Pointer: {0}     Command: {1}", nStep, myCommand.GetCommand());
                int stepIncrease = myCommand.GetStep();
                switch (myCommand.GetCommand())
                {
                    case 1: // Add
                        myCommand.Add(commands2);
                        break;

                    case 2: // Multi
                        myCommand.Multi(commands2);
                        break;

                    case 3: // Input
                        myCommand.Input(InputValue[i], commands2);
                        i++;
                        break;

                    case 4: // Output
                        Output = myCommand.Output();
                        break;

                    case 5: //Opcode 5: jump-if-true:
                        nStep = myCommand.JumpIfTrue(nStep);
                        stepIncrease = 0;
                        break;

                    case 6: //Opcode 6: jump-if-false
                        nStep = myCommand.JumpIfFalse(nStep);
                        stepIncrease = 0;
                        break;

                    case 7: //Opcode 7: less than
                        myCommand.LessThan(commands2);
                        break;

                    case 8: //Opcode 8: equal
                        myCommand.Equals(commands2);
                        break;

                    case 99: // Halt
                        //Console.WriteLine("Case 99: Stop");
                        bError = true;
                        break;

                    default:
                        Console.WriteLine("Error");
                        bError = true;
                        break;
                }

                nStep += stepIncrease;
            }
            return Output;
        }

        static Int64 RunTheProgramm()
        {
            StreamReader file = new StreamReader(@".\data.txt");
            string line = file.ReadLine();
            string[] words = line.Split(',');

            List<int> commands_vanile = new List<int>();

            foreach (string word in words)
            {
                commands_vanile.Add(int.Parse(word));
            }



            string sPhaseSettings;
            int counter = 1;
            int maxOutput = 0;
            string sBestPhaseSettings="";
            foreach (var p in Permutate("01234"))
            {
                sPhaseSettings = p;
                counter++;

                int[] nTheValue = { 0, 0 };
                int res = 0;
                for (int i = 0; i < 5; i++)
                {
                    nTheValue[0] = int.Parse(sPhaseSettings[i].ToString());
                    nTheValue[1] = res;
                    List<int> commands2 = new List<int>(commands_vanile);
                    res = RunMyProgramm(commands2, nTheValue);
                }
                if (res > maxOutput)
                {
                    maxOutput = res;
                    sBestPhaseSettings = sPhaseSettings;
                }
            }

            Console.WriteLine("PhaseSetting: {0}         Power: {1}", sBestPhaseSettings,maxOutput);
            return 0;
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

                if (command != 99)
                    ArgOne.argValue = ReadMemory(words[nStep + 1], ArgOne.argMode, ref words);

                if (command == 3)
                {
                    ArgOne.argValue = ReadMemory(nStep + 1, ArgOne.argMode, ref words);
                }

                if (GetArgNumber(command) >= 2)
                {
                    if (word.ToString()[3] == '1') ArgTwo.argMode = 1; else ArgTwo.argMode = 0;
                    ArgTwo.argValue = ReadMemory(words[nStep + 2], ArgTwo.argMode, ref words);

                    if (GetArgNumber(command) >= 3)
                    {
                        ArgThree.argMode = 1;
                        ArgThree.argValue = ReadMemory(words[nStep + 3], ArgThree.argMode, ref words);
                    }
                }
            }
            //Opcode 5 is jump-if-true: if the first parameter is non-zero, it sets the instruction pointer to the value from the second parameter.Otherwise, it does nothing.
            public int JumpIfTrue(int globalStep)
            {
                if (ArgOne.argValue > 0)
                    Step = ArgTwo.argValue;
                else
                    Step = globalStep + 3;
                return Step;
            }
            //Opcode 6 is jump-if-false: if the first parameter is zero, it sets the instruction pointer to the value from the second parameter. Otherwise, it does nothing.
            public int JumpIfFalse(int globalStep)
            {
                if (ArgOne.argValue == 0)
                    Step = ArgTwo.argValue;
                else
                    Step = globalStep + 3;
                return Step;
            }
            //Opcode 7 is less than: if the first parameter is less than the second parameter, it stores 1 in the position given by the third parameter. Otherwise, it stores 0.
            public void LessThan(List<int> words)
            {
                if (ArgOne.argValue < ArgTwo.argValue)
                    WriteMemory(ArgThree.argValue, 1, ref words);
                else
                    WriteMemory(ArgThree.argValue, 0, ref words);
            }

            //Opcode 8 is equals: if the first parameter is equal to the second parameter, it stores 1 in the position given by the third parameter. Otherwise, it stores 0.
            public void Equals(List<int> words)
            {
                if (ArgOne.argValue == ArgTwo.argValue)
                    WriteMemory(ArgThree.argValue, 1, ref words);
                else
                    WriteMemory(ArgThree.argValue, 0, ref words);
            }

            public int GetCommand() { return command; }
            public int GetStep() { return Step; }
            public int ReadMemory(int mAddress, int mode, ref List<int> words)
            {
                int res;
                if (mode == 1) res = mAddress;
                else res = words[mAddress];
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
               // Console.WriteLine(ArgOne.argValue);
                return ArgOne.argValue;
            }
            public int GetArgNumber(int arg)
            {
                int res = 0;
                switch (arg % 100)
                {
                    case 1: res = 3; break; //Opcode 1: add 
                    case 2: res = 3; break; //Opcode 2: multi
                    case 3: res = 1; break; //Opcode 3: input
                    case 4: res = 1; break; //Opcode 4: output
                    case 5: res = 2; break; //Opcode 5: jump-if-true:
                    case 6: res = 2; break; //Opcode 6: jump-if-false
                    case 7: res = 3; break; //Opcode 7: less than
                    case 8: res = 3; break; //Opcode 8: equal
                    case 99: res = 0; break;//Opcode 99: end
                }
                return res;
            }
        }
    }
}
