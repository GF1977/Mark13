using System;
using System.Collections.Generic;

namespace MyClasses
{
    class TheCommand
    {
        private static Int64 command;
        private static Int64 Step;
        private static Int64 relative_base_offset = 0;
        private struct myArgument
        {
            public Int64 argValue;
            public Int64 argMode;
        }

        private static myArgument ArgOne;
        private static myArgument ArgTwo;
        private static myArgument ArgThree;

        public TheCommand()
        {
            relative_base_offset = 0;
            Step = 0;
        }

        public TheCommand(Int64 nStep, ref List<Int64> words)
        {
            Int64 word = words[(int)nStep];
            command = word % 100;
            Step = GetArgNumber(command) + 1; // 1 for command
            word += 1000000;
            if (word.ToString()[4] == '0') // position mode
                ArgOne.argMode = 0;
            if (word.ToString()[4] == '1') // absolute mode
                ArgOne.argMode = 1;
            if (word.ToString()[4] == '2') // relative mode
                ArgOne.argMode = 2;


            if (command != 99)
                ArgOne.argValue = ReadMemory(words[(int)(nStep + 1)], ArgOne.argMode, ref words);

            if (command == 3)
               ArgOne.argValue = ReadMemory(nStep + 1, 0 , ref words); // argValue is always the 2nd byte of Input command

            if (GetArgNumber(command) >= 2)
            {
                if (word.ToString()[3] == '0') // position mode
                    ArgTwo.argMode = 0;
                if (word.ToString()[3] == '1') // absolute mode
                    ArgTwo.argMode = 1;
                if (word.ToString()[3] == '2') // relative mode
                    ArgTwo.argMode = 2;

                ArgTwo.argValue = ReadMemory(words[(int)(nStep + 2)], ArgTwo.argMode, ref words);

                if (GetArgNumber(command) >= 3)
                {
                    if (word.ToString()[2] == '0') // position mode (3 rd argument has address to write, it is always absolute or relative address)
                        ArgThree.argMode = 1;
                    if (word.ToString()[2] == '1') // absolute mode
                        ArgThree.argMode = 1;
                    if (word.ToString()[2] == '2') // relative mode
                        ArgThree.argMode = 2;

                    ArgThree.argValue = ReadMemory(words[(int)(nStep + 3)], 1, ref words);
                }
            }
        }
        //Opcode 5 is jump-if-true: if the first parameter is non-zero, it sets the instruction pointer to the value from the second parameter.Otherwise, it does nothing.
        private Int64 JumpIfTrue(Int64 globalStep)
        {
            if (ArgOne.argValue > 0)
                Step = ArgTwo.argValue;
            else
                Step = globalStep + 3;
            return Step;
        }
        //Opcode 6 is jump-if-false: if the first parameter is zero, it sets the instruction pointer to the value from the second parameter. Otherwise, it does nothing.
        private Int64 JumpIfFalse(Int64 globalStep)
        {
            if (ArgOne.argValue == 0)
                Step = ArgTwo.argValue;
            else
                Step = globalStep + 3;
            return Step;
        }
        //Opcode 7 is less than: if the first parameter is less than the second parameter, it stores 1 in the position given by the third parameter. Otherwise, it stores 0.
        private void LessThan(List<Int64> words)
        {
            if (ArgOne.argValue < ArgTwo.argValue)
                WriteMemory(ArgThree.argValue, ArgThree.argMode, 1, ref words);
            else
                WriteMemory(ArgThree.argValue, ArgThree.argMode, 0, ref words);
        }

        //Opcode 8 is equals: if the first parameter is equal to the second parameter, it stores 1 in the position given by the third parameter. Otherwise, it stores 0.
        private void Equals(List<Int64> words)
        {
            if (ArgOne.argValue == ArgTwo.argValue)
                WriteMemory(ArgThree.argValue, ArgThree.argMode, 1, ref words);
            else
                WriteMemory(ArgThree.argValue, ArgThree.argMode, 0, ref words);
        }

        //Opcode 9: adjusts the relative base
        private void AdjustRelativeBaseOffset()
        {
            relative_base_offset += ArgOne.argValue;
        }

        public Int64 GetCommand() { return command; }
        private Int64 GetStep() { return Step; }
        private Int64 ReadMemory(Int64 mAddress, Int64 mode, ref List<Int64> words)
        {
            Int64 res = -1;
            if (mode == 0) res = words[(int)(mAddress)];
            if (mode == 1) res = mAddress;
            if (mode == 2)
            {
                try
                    {
                       res = words[(int)(mAddress + relative_base_offset)];
                    }
                catch
                {
                    Console.WriteLine("Operation [Read] from address = {0}  the arrays size = {1}", mAddress + relative_base_offset, words.Count);
                    throw;
                }
                    
            }
            return res;
        }
        private void WriteMemory(Int64 mAddress, Int64 mode, Int64 mvalue, ref List<Int64> words)
        {
            if (mode == 0 || mode == 1)
                words[(int)mAddress] = mvalue;
            if (mode == 2)
            {
                try
                {
                    words[(int)(mAddress + relative_base_offset)] = mvalue;
                }
                catch
                {
                    Console.WriteLine("Operation [Write] to address = {0}  the arrays size = {1}", mAddress + relative_base_offset, words.Count);
                    throw;
                }
            }
                
        }
        private void Add(List<Int64> words)
        {
            WriteMemory(ArgThree.argValue, ArgThree.argMode, ArgOne.argValue + ArgTwo.argValue, ref words);
        }
        private void Multi(List<Int64> words)
        {
            WriteMemory(ArgThree.argValue, ArgThree.argMode, ArgOne.argValue * ArgTwo.argValue, ref words);
        }
        private void Input(Int64 number, List<Int64> words)
        {
            WriteMemory(ArgOne.argValue, ArgOne.argMode, number, ref words);
        }
        private Int64 Output()
        {
            //Console.WriteLine("Output: {0}" ,ArgOne.argValue);
            //Int64 res;
            //if (ArgOne.argMode == 2)
            //    res = ArgOne.argValue;
            //else
            //    res = ArgOne.argValue;
            return ArgOne.argValue; ;
        }

        private Int64 GetArgValue(Int64 ArgNum)
        {
            Int64 res = 0;
            if (ArgNum == 0) res = ArgOne.argValue;
            if (ArgNum == 1) res = ArgTwo.argValue;
            if (ArgNum == 2) res = ArgThree.argValue;
            return res;
        }

        private Int64 GetArgMode(Int64 ArgNum)
        {
            Int64 res = 0;
            if (ArgNum == 0) res = ArgOne.argMode;
            if (ArgNum == 1) res = ArgTwo.argMode;
            if (ArgNum == 2) res = ArgThree.argMode;
            return res;
        }

        private int GetArgNumber(Int64 arg)
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
                case 9: res = 1; break; //Opcode 9: adjusts the relative base
                case 99: res = 0; break;//Opcode 99: end
            }
            return res;
        }
        private void Debug(Int64 nStep)
        {
            Console.WriteLine("Step:        {0}", nStep);
            Console.WriteLine("Command:     {0},   arguments ({1})", GetCommandName(), GetArgNumber(GetCommand()));
            Console.WriteLine("Argument 1:  Mode [{0}]   Value:  {1}", GetArgMode(0), GetArgValue(0));
            if (GetArgNumber(GetCommand()) >= 2)
                Console.WriteLine("Argument 2:   Mode [{0}]   Value:  {1}", GetArgMode(1), GetArgValue(1));
            if (GetArgNumber(GetCommand()) >= 3)
                Console.WriteLine("Argument 3:   Mode [{0}]   Value:  {1}", GetArgMode(2), GetArgValue(2));
            Console.WriteLine("---------------------------");
        }

        string GetCommandName()
        {
            string theRes = "";
            switch (GetCommand())
            {
                case 1: theRes = "Add X,Y -> Z"; break;
                case 2: theRes = "Mult X,Y -> Z"; break;
                case 3: theRes = "Input X"; break;
                case 4: theRes = "Output X"; break;
                case 5: theRes = "Jump if true"; break;
                case 6: theRes = "Jump if false"; break;
                case 7: theRes = "Less than"; break;
                case 8: theRes = "Equal"; break;
                case 9: theRes = "Realteive base +X"; break;
                case 99: theRes = "STOP"; break;

                default:
                    theRes = "----WRONG COMMAND WORD-----";
                    break;
            }

            return theRes;
        }

        public static Int64[] RunMyProgramm(List<Int64> commands, Int64 InputValue, bool bDebug = false)
        {
            Int64 nStep = 0;
            Int64[] Output = { -1, nStep }; // Value , Pointer
            do
            {
                TheCommand myCommand = new TheCommand(nStep, ref commands);
                if (bDebug)
                    myCommand.Debug(nStep);

                Output = myCommand.ExecuteOneCommand(nStep, InputValue, commands);
                nStep = Output[1];
            }
            while (nStep <= commands.Count && nStep > 0);
            return Output;
        }

        public Int64[] ExecuteOneCommand(Int64 nStep, Int64 InputValue, List<Int64> commands)
        {
                Int64[] Output = {Int64.MinValue, 0 };
                Int64 stepIncrease = this.GetStep();
                switch (this.GetCommand())
                {
                    case 1: // Add
                        this.Add(commands);
                        break;

                    case 2: // Multi
                        this.Multi(commands);
                        break;

                    case 3: // Input
                        this.Input(InputValue, commands);
                        break;

                    case 4: // Output
                        Output[0] = this.Output();
                        if (Output[0] == Int64.MinValue)
                        {
                        Console.WriteLine("WRONG OUTPUT: -1");
                        }
                        //bError = true;
                        break;

                    case 5: //Opcode 5: jump-if-true:
                        nStep = this.JumpIfTrue(nStep);
                        stepIncrease = 0;
                        break;

                    case 6: //Opcode 6: jump-if-false
                        nStep = this.JumpIfFalse(nStep);
                        stepIncrease = 0;
                        break;

                    case 7: //Opcode 7: less than
                        this.LessThan(commands);
                        break;

                    case 8: //Opcode 8: equal
                        this.Equals(commands);
                        break;

                    case 9: //Opcode 9: relative_base_offset adjustement
                        this.AdjustRelativeBaseOffset();
                        break;


                    case 99: // Halt
                        //Console.WriteLine("Case 99: Stop");
                        nStep = 0;
                        stepIncrease = 0;
                        break;

                    default:
                        Console.WriteLine("Error");
                        break;
                }
            Output[1] = nStep + stepIncrease;
            return Output;
        }

    }
}
