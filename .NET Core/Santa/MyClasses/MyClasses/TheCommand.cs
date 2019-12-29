using System;
using System.Collections.Generic;

namespace MyClasses
{
    class TheCommand
    {
        private static Int64 command;
        private static Int64 Step;
        private static Int64 relative_base_offset = 0;
        struct myArgument
        {
            public Int64 argValue;
            public Int64 argMode;
        }

        private static myArgument ArgOne;
        private static myArgument ArgTwo;
        private static myArgument ArgThree;
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
            {
                ArgOne.argValue = ReadMemory(nStep + 1, ArgOne.argMode, ref words);
            }

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
        public Int64 JumpIfTrue(Int64 globalStep)
        {
            if (ArgOne.argValue > 0)
                Step = ArgTwo.argValue;
            else
                Step = globalStep + 3;
            return Step;
        }
        //Opcode 6 is jump-if-false: if the first parameter is zero, it sets the instruction pointer to the value from the second parameter. Otherwise, it does nothing.
        public Int64 JumpIfFalse(Int64 globalStep)
        {
            if (ArgOne.argValue == 0)
                Step = ArgTwo.argValue;
            else
                Step = globalStep + 3;
            return Step;
        }
        //Opcode 7 is less than: if the first parameter is less than the second parameter, it stores 1 in the position given by the third parameter. Otherwise, it stores 0.
        public void LessThan(List<Int64> words)
        {
            if (ArgOne.argValue < ArgTwo.argValue)
                WriteMemory(ArgThree.argValue, ArgThree.argMode, 1, ref words);
            else
                WriteMemory(ArgThree.argValue, ArgThree.argMode, 0, ref words);
        }

        //Opcode 8 is equals: if the first parameter is equal to the second parameter, it stores 1 in the position given by the third parameter. Otherwise, it stores 0.
        public void Equals(List<Int64> words)
        {
            if (ArgOne.argValue == ArgTwo.argValue)
                WriteMemory(ArgThree.argValue, ArgThree.argMode, 1, ref words);
            else
                WriteMemory(ArgThree.argValue, ArgThree.argMode, 0, ref words);
        }

        //Opcode 9: adjusts the relative base
        public void AdjustRelativeBaseOffset()
        {
            relative_base_offset += ArgOne.argValue;
        }

        public Int64 GetCommand()   { return command; }
        public Int64 GetStep()      { return Step; }
        public Int64 ReadMemory(Int64 mAddress, Int64 mode, ref List<Int64> words)
        {
            Int64 res = -1;
            if (mode == 0) res = words[(int)(mAddress)];
            if (mode == 1) res = mAddress;
            if (mode == 2) res = words[(int)(mAddress + relative_base_offset)];
            return res;
        }
        public void WriteMemory(Int64 mAddress, Int64 mode, Int64 mvalue, ref List<Int64> words)
        {
            if (mode == 0 || mode == 1)
                words[(int)mAddress] = mvalue;
            if (mode == 2)
                words[(int)(mAddress + relative_base_offset)] = mvalue;
        }
        public void Add(List<Int64> words)
        {
            WriteMemory(ArgThree.argValue, ArgThree.argMode, ArgOne.argValue + ArgTwo.argValue, ref words);
        }
        public void Multi(List<Int64> words)
        {
            WriteMemory(ArgThree.argValue, ArgThree.argMode, ArgOne.argValue * ArgTwo.argValue, ref words);
        }
        public void Input(Int64 number, List<Int64> words)
        {
            WriteMemory(ArgOne.argValue, ArgOne.argMode, number, ref words);
        }
        public Int64 Output()
        {
            Console.WriteLine(ArgOne.argValue);
            Int64 res;
            if (ArgOne.argMode == 2)
                res = ArgOne.argValue;
            else
                res = ArgOne.argValue;
            return res;
        }

        public Int64 GetArgValue(Int64 ArgNum)
        {
            Int64 res = 0;
            if (ArgNum == 0) res = ArgOne.argValue;
            if (ArgNum == 1) res = ArgTwo.argValue;
            if (ArgNum == 2) res = ArgThree.argValue;
            return res;
        }

        public Int64 GetArgMode(Int64 ArgNum)
        {
            Int64 res = 0;
            if (ArgNum == 0) res = ArgOne.argMode;
            if (ArgNum == 1) res = ArgTwo.argMode;
            if (ArgNum == 2) res = ArgThree.argMode;
            return res;
        }

        public int GetArgNumber(Int64 arg)
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
        public void Debug()
        {

            Console.WriteLine("Command:     {0}", GetCommand());
            Console.WriteLine("Arg count:   {0}", GetArgNumber(GetCommand()));
            Console.WriteLine("Argument 1:  [{0}]{1}", GetArgMode(0), GetArgValue(0));
            if (GetArgNumber(GetCommand()) >= 2)
                Console.WriteLine("Argument 2:  [{0}]{1}", GetArgMode(1), GetArgValue(1));
            if (GetArgNumber(GetCommand()) >= 3)
                Console.WriteLine("Argument 3:  [{0}]{1}", GetArgMode(2), GetArgValue(2));
            Console.WriteLine("---------------------------");
        }

        public static Int64[] RunMyProgramm(List<Int64> commands2, Int64 InputValue)
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
