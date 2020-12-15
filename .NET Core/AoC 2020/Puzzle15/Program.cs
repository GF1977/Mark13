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
        foreach (string S in preParsingString)
        theQueue.Add(int.Parse(S));

        // the main cycle
        int nEnd = 30000000 - theQueue.Count;
            int nTheLastNumberSpoken = theQueue.Last();
        while (nEnd-- > 0)
            {
                int nNumberToSay = 0;
                if (theQueue.Count(n=>n==nTheLastNumberSpoken)>1)
                {
                    List<int> nPosition = new List<int>();
                    int n = theQueue.Count(n => n == nTheLastNumberSpoken);
                    int nIndex = 0;
                    int i = 0;
                    while(i++ < n)
                    {
                        nIndex = theQueue.FindIndex(nIndex, n => n == nTheLastNumberSpoken);
                        nPosition.Add(nIndex);
                        nIndex++;
                    }
                    nNumberToSay = nPosition[n-1] - nPosition[n-2];
                    theQueue.Add(nNumberToSay);
                    nTheLastNumberSpoken = nNumberToSay;
                    continue;
                }
                else
                {
                    nNumberToSay = 0;
                    theQueue.Add(nNumberToSay);
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
