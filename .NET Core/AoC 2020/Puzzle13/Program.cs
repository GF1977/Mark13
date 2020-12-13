using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace Puzzle13
{
    class Program
    {

        static void Main()
        {
            Console.Clear();
            Console.WriteLine(DateTime.Now);

            List<string> fileInput = new List<string>();
            fileInput = GetData();

            int myDepartureTime = int.Parse(fileInput.ElementAt(0));

            string[] BusesSchedulesRaw = fileInput.ElementAt(1).Split(",");
            List<int> BusesSchedules = new List<int>();
            foreach (string S in BusesSchedulesRaw)
            {
                if (S != "x")
                    BusesSchedules.Add(int.Parse(S));
            }

            int nMinWaitTime = int.MaxValue;
            int BusId = 0;
            foreach(int Schedule in BusesSchedules)
            {
                int nWaitTime = Schedule - (myDepartureTime % Schedule);
                if (nWaitTime < nMinWaitTime)
                {
                    nMinWaitTime = nWaitTime;
                    BusId = Schedule;
                }
            }

            var vPartOneAnswer = nMinWaitTime * BusId;
            long vPartTwoAnswer = 0;

            long delta = 0;
            int minutes = 0;
            int ID = 0;
            long X = 0;
            foreach (string S in BusesSchedulesRaw)
            {
                if(delta == 0)
                {
                    delta = int.Parse(S);
                    X = delta;
                    continue;
                }

                if (S == "x")
                {
                    minutes++;
                    continue;
                }
                else
                    ID = int.Parse(S);
                minutes++;

                int i = 0;
                long new_X = 0;
                while (true)
                {
                    if ((X + minutes) % ID == 0)
                    {

                        if (i > 0)
                        {

                            delta = X - new_X;
                            X = new_X;
                            break;
                        }
                        if (i == 0)
                        {
                            new_X = X;
                            i++;
                        }
                        //Console.WriteLine(X);
                        //Console.ReadKey();
                        //break;
                    }
                    X += delta;
                }


            }





            

            Console.WriteLine("--------------------------");
            Console.WriteLine("PartOne: {0}", vPartOneAnswer);
            Console.WriteLine("PartTwo: {0}", X);

        }


        static List<string> GetData()
        {
            List<string> fileInput = new List<string>();
            string fileName = ".\\data.txt";
            if (File.Exists(@fileName))
            {
                using StreamReader file = new StreamReader(@fileName);
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
