using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;

namespace Puzzle14
{
    class Program
{

        static public Dictionary<long, long> Cells = new Dictionary<long, long>();

        static void Main()
    {
        Console.Clear();
        Console.WriteLine(DateTime.Now);




        List<string> fileInput = new List<string>();
        fileInput = GetData();


            string sMask = "";
            foreach (string S in fileInput)
            {
                string[] sPreParsing = S.Split(" = ");
                if(sPreParsing[0] == "mask")
                {
                    sMask = sPreParsing[1];
                    continue;
                }
                long CellAddress = long.Parse(sPreParsing[0].Split('[', ']')[1]);
                long lValue = long.Parse(sPreParsing[1]);

                long CellValue = ApplyMask(sMask, lValue);

                if (Cells.ContainsKey(CellAddress))
                    Cells[CellAddress] = CellValue;
                else
                    Cells.Add(CellAddress, CellValue);


            }

            long vPartOneAnswer = 0;
            foreach (var Cell in Cells)
            {
                vPartOneAnswer += Cell.Value;
            }


        
        var vPartTwoAnswer = "";

        Console.WriteLine("--------------------------");
        Console.WriteLine("PartOne: {0}", vPartOneAnswer);
        Console.WriteLine("PartTwo: {0}", vPartTwoAnswer);

    }

    static long ApplyMask(string sMask, long lValue)
        {
            string sV = Convert.ToString(lValue, 2);
            StringBuilder sValue = new StringBuilder("000000000000000000000000000000000000", 0, 36 - sV.Length, 36);
            sValue.Append(sV);
            sMask = sMask.Substring(sMask.Length - sValue.Length);


            for (int i = 0; i < sValue.Length; i++)
                if (sMask[i] != 'X')
                    sValue[i] = sMask[i];



            long result = Convert.ToInt64(sValue.ToString(), 2);
            return result;
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
