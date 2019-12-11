using System;
using System.Collections.Generic;
using System.IO;

namespace Puzzle2
{
    class Program
    {
        static void Main(string[] args)
        {
            int noun = 0;
            int verb = 0;
            Int64 res;

            bool bStop = false;
            

            while (noun < 100 && !bStop)
            {
                while (verb < 100 && !bStop)
                {
                    res = RunTheProgramm(noun,verb);
                    if (res == 19690720)
                    {
                        Console.WriteLine("Verb = {0};   Noun = {1};  Result = {2}; Answer = {3}", verb, noun,res, 100 * noun + verb);
                        bStop = true;
                        break;
                    }
                    else
                    {
                        //Console.WriteLine("Verb = {0};   Noun = {1};   Result = {2}", verb, noun, res);
                        verb++;
                    }
                }
                verb = 0;
                noun++;
            }



            Console.WriteLine("1st part Puzzle: {0}", RunTheProgramm(12, 2));

        }


                     
        static Int64 RunTheProgramm(int noun, int verb)
        {
            StreamReader file = new StreamReader(@".\data.txt");
            string line = file.ReadLine();
            string[] words = line.Split(',');

            List<int> commands = new List<int>();
            int i = 0;

            while (i < words.Length)
            {
                commands.Add(int.Parse(words[i++]));
            }

            // run the programm..
            // replace position 1 with the value 12 and replace position 2 with the value 2
            commands[1] = noun;
            commands[2] = verb;

            i = 0;
            bool bError = false;
            while (i <= commands.Count && commands[i] != 99 && !bError)
            {
                int command = commands[i];
                int n1 = commands[i + 1];
                int n2 = commands[i + 2];
                int n3 = commands[i + 3];
                switch (command)
                {
                    case 1:
                        //Console.WriteLine("Case 1: ADD   {0} + {1} -> {2}", commands[n1], commands[n2], i + 3 );
                        commands[n3] = commands[n1] + commands[n2];
                        break;

                    case 2:
                        //Console.WriteLine("Case 2: Multiply {0} * {1} -> {2}", commands[n1], commands[n2], i + 3);
                        commands[n3] = commands[n1] * commands[n2];
                        break;

                    case 99:
                        // Console.WriteLine("Case 3: Stop");
                        break;

                    default:
                        Console.WriteLine("Error");
                        bError = true;
                        break;
                }

                i += 4;
            }
            
            return commands[0];
        }
    }
}
