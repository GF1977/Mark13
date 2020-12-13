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


            // PART TWO
            long delta = int.Parse(BusesSchedulesRaw[0]);
            
            long X = delta; // X = start timestamp, and finaly the number we are looking for
            int busID;
            int minutes = 0;

            foreach(string S in BusesSchedulesRaw.Skip(1))
            {
                minutes++;
                if (S == "x") continue;
                busID = int.Parse(S);

                while (true)
                {
                    if ((X + minutes) % busID == 0)
                    {
                        // here is the trick. as the X is huge, we have to increas delta to speed up our search
                        delta *= busID;
                        break;
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
