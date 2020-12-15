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
        static public Dictionary<long, long> CellsV2 = new Dictionary<long, long>();

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

            // PART TWO
            foreach (string S in fileInput)
            {
                string[] sPreParsing = S.Split(" = ");
                if (sPreParsing[0] == "mask")
                {
                    sMask = sPreParsing[1];
                    continue;
                }
                long CellAddress = long.Parse(sPreParsing[0].Split('[', ']')[1]);
                long lValue = long.Parse(sPreParsing[1]);


                List<long> FluctuatedCellAddress = new List<long>();
                FluctuatedCellAddress = ApplyMaskV2(sMask, CellAddress);

                
                foreach(long Address in FluctuatedCellAddress)
                if (CellsV2.ContainsKey(Address))
                        CellsV2[Address] = lValue;
                else
                        CellsV2.Add(Address, lValue);
            }

            long vPartTwoAnswer = 0;
            foreach (var Cell in CellsV2)
                vPartTwoAnswer += Cell.Value;



        Console.WriteLine("--------------------------");
        Console.WriteLine("PartOne: {0}", vPartOneAnswer);
        Console.WriteLine("PartTwo: {0}", vPartTwoAnswer);

    }

        static List<long> ApplyMaskV2(string sMask, long lValue)
        {
            string sV = Convert.ToString(lValue, 2);
            StringBuilder sbValue = new StringBuilder("000000000000000000000000000000000000", 0, 36 - sV.Length, 36);
            sbValue.Append(sV);
            sMask = sMask.Substring(sMask.Length - sbValue.Length);


            for (int i = 0; i < sbValue.Length; i++)
                if (sMask[i] != '0')
                    sbValue[i] = sMask[i];


            string sValue = sbValue.ToString();
            int x = sValue.Count(n => n == 'X');

            

            List<long> result = new List<long>();
            int nMaxFluctuation = (int)Math.Pow(2, x);

            for (int i=0; i< nMaxFluctuation; i++)
            {

                string newAddress = ReplaceXby0or1(sbValue, i, x);

                long lAddress = Convert.ToInt64(newAddress, 2); ;
                result.Add(lAddress);
            }

            return result;
        }

        static string ReplaceXby0or1(StringBuilder sbValueoriginal, int N, int x)
        {
            StringBuilder sbValue = new StringBuilder(sbValueoriginal.ToString());
            string sFluctuation = "0000000000000000000000000000" + Convert.ToString(N, 2);
            sFluctuation = sFluctuation.Substring(sFluctuation.Length - x);

            int y = 0;
            for(int i = 0; i < sbValue.Length;i++)
                if(sbValue[i] == 'X')
                    sbValue[i] = sFluctuation[y++];



            return sbValue.ToString();
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
