using System;
using System.Collections.Generic;
using System.IO;
using MyClasses;

namespace Puzzle7
{

    class Program
    {
        static void Main(string[] args)
        {

            Puzzle9();

        }

        static void Puzzle9()
        {
            Int64 StartValue = 1;
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


            Int64[] res = RunMyProgramm(commands_vanile, StartValue);
            Console.WriteLine("Output: {0}", res[0]);

        }

        static Int64[] RunMyProgramm(List<Int64> commands2, Int64 InputValue)
        {
            Int64 nStep = 0;
            bool bError = false;
            Int64[] Output = { -1, nStep }; // Value , Pointer
            while (nStep <= commands2.Count && !bError)
            {
                TheCommand myCommand = new TheCommand(nStep, ref commands2);
                // myCommand.Debug();

                Int64 stepIncrease = myCommand.GetStep();
                switch (myCommand.GetCommand())
                {
                    case 1: // Add
                        myCommand.Add(commands2);
                        break;

                    case 2: // Multi
                        myCommand.Multi(commands2);
                        break;

                    case 3: // Input
                        myCommand.Input(InputValue, commands2);
                        break;

                    case 4: // Output
                        Output[0] = myCommand.Output();
                        //bError = true;
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

                    case 9: //Opcode 9: relative_base_offset adjustement
                        myCommand.AdjustRelativeBaseOffset();
                        break;


                    case 99: // Halt
                        //Console.WriteLine("Case 99: Stop");
                        nStep = 0;
                        bError = true;
                        break;

                    default:
                        Console.WriteLine("Error");
                        bError = true;
                        break;
                }
                nStep += stepIncrease;
            }
            Output[1] = nStep;
            return Output;
        }
    }
}