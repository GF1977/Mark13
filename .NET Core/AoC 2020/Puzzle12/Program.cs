using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace Puzzle11
{
    class Program
    {
        public class Action
        {
            public string command;
            public int value; 

            public Action(string command, int value)
            {
                this.command = command;
                this.value = value;
            }
        }



        static void Main()
        {
            Console.Clear();
            Console.WriteLine(DateTime.Now);

            var vPartTwoAnswer = "";

            List<string> fileInput = new List<string>();
            fileInput = GetData();

            List<Action> ActionsQueue = new List<Action>();
            foreach(string S in fileInput)
            {
                string command  = S.Substring(0, 1);
                int value       = int.Parse(S.Substring(1, S.Length-1));
                ActionsQueue.Add(new Action(command, value));
            }

            var vPartOneAnswer = ShipRun(ActionsQueue);

            Console.WriteLine("--------------------------");
            Console.WriteLine("PartOne: {0}", vPartOneAnswer);
            Console.WriteLine("PartTwo: {0}", vPartTwoAnswer);



        }

        private static object ShipRun(List<Action> actionQueue)
        {
            int Angle = 90; // 0 = N; 90 = E;  180 = S; 270 = W;
            int NS = 0; // positive = N , negative = S
            int WE = 0; // Positive = E , negative = W

            foreach(Action A in actionQueue)
            {
                switch (A.command)
                {
                    case "N":                        NS += A.value;                        break;
                    case "S":                        NS -= A.value;                        break;
                    case "E":                        WE += A.value;                        break;
                    case "W":                        WE -= A.value;                        break;
                    case "L": Angle = (Angle + 360 - A.value) % 360; break;
                    case "R": Angle = (Angle + A.value) % 360; break;
                    case "F":
                        {
                            switch (Angle)
                            {
                                case   0: NS += A.value; break;
                                case  90: WE += A.value; break;
                                case 180: NS -= A.value; break;
                                case 270: WE -= A.value; break;
                                default:                 break;
                            }
                        }
                        break;
                    default:
                        break;
                }
            }

            return Math.Abs(NS) + Math.Abs(WE);
         
        }

        static List<string> GetData()
        {
            List<string> fileInput = new List<string>();
            string fileName = ".\\data.txt";
            if (File.Exists(@fileName))
            {
                using (StreamReader file = new StreamReader(@fileName))
                    while (!file.EndOfStream)
                    {
                        string S = file.ReadLine();
                        fileInput.Add(S);
                    }
            }
            else
            {
                var myFile = File.CreateText(@fileName);
                myFile.Close();
                Process.Start(@"C:\Program Files\Notepad++\notepad++.exe", fileName);
            }

            return fileInput;
        }
    }
}
