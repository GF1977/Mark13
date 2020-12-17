using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace Puzzle15
{
    class Program
{

    static void Main()
    {
        Console.Clear();
        Console.WriteLine(DateTime.Now);

        List<string> fileInput = new List<string>();
        fileInput = GetData();

        string[] preParsingString = fileInput[0].Split(",");

            List<int> theQueue = new List<int>();
            
            // Item1 = position A
            // Item2 = position B
            // Item3 = counter
            Dictionary<long, Tuple<long, long, long>> Numbers = new Dictionary<long, Tuple<long, long, long>>();
            

            long x = 0;
            foreach (string S in preParsingString)
            {
                theQueue.Add(int.Parse(S));
                Tuple<long, long, long> PositionAndCount = new Tuple<long, long, long> ( 0 , x+1 , 1 );
                Numbers.Add(long.Parse(S), PositionAndCount);
                x++;
            }
        // the main cycle
        
        long nEnd = 30000000;

            long nTheLastNumberSpoken = theQueue.Last();
        while (x++ < nEnd)
            {
                long nNumberToSay = 0;
                //if (theQueue.Count(n=>n==nTheLastNumberSpoken)>1)
                Tuple<long, long, long> PositionAndCount;
                bool bNumberWasThere = Numbers.TryGetValue(nTheLastNumberSpoken,out PositionAndCount);
                if (bNumberWasThere && PositionAndCount.Item3  > 1)
                {

                    nNumberToSay = PositionAndCount.Item2 - PositionAndCount.Item1;
                    if (Numbers.TryGetValue(nNumberToSay, out PositionAndCount))
                    {
                        Numbers.Remove(nNumberToSay);
                        Numbers.Add(nNumberToSay, new Tuple<long, long, long>(PositionAndCount.Item2, x, PositionAndCount.Item3 + 1));
                    }
                    else
                        Numbers.Add(nNumberToSay, new Tuple<long, long, long>(0, x, 1));
                    nTheLastNumberSpoken = nNumberToSay;
                    continue;
                }
                else
                {
                    nNumberToSay = 0;
                    Numbers.TryGetValue(0, out PositionAndCount);
                    Numbers.Remove(nNumberToSay);
                    Numbers.Add(nNumberToSay, new Tuple<long, long, long>(PositionAndCount.Item2 , x , PositionAndCount.Item3 + 1));
                    nTheLastNumberSpoken = nNumberToSay;
                    continue;
                }

            }


        var vPartOneAnswer = nTheLastNumberSpoken;
        var vPartTwoAnswer = "";

        Console.WriteLine("--------------------------");
        Console.WriteLine("PartOne: {0}", vPartOneAnswer);
        Console.WriteLine("PartTwo: {0}", vPartTwoAnswer);

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
