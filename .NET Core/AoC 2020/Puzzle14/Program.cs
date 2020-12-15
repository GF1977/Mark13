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
        private const string MASK_36BIT = "000000000000000000000000000000000000";
        private static readonly Dictionary<long, long> Cells = new Dictionary<long, long>();
        private static readonly Dictionary<long, long> CellsV2 = new Dictionary<long, long>();

        static void Main()
    {
        Console.Clear();
        Console.WriteLine(DateTime.Now);

        List<string> fileInput = GetData();
            
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

                // Part ONE
                long CellValue = ApplyMaskV1(sMask, lValue);
                if (Cells.ContainsKey(CellAddress))
                    Cells[CellAddress] = CellValue;
                else
                    Cells.Add(CellAddress, CellValue);
                
                // Part TWO
                List<long> FluctuatedCellAddress = ApplyMaskV2(sMask, CellAddress);

                foreach (long Address in FluctuatedCellAddress)
                    if (CellsV2.ContainsKey(Address))
                        CellsV2[Address] = lValue;
                    else
                        CellsV2.Add(Address, lValue);
             }

            long vPartOneAnswer = 0;
            foreach (var Cell in Cells)
                vPartOneAnswer += Cell.Value;

            long vPartTwoAnswer = 0;
            foreach (var Cell in CellsV2)
                vPartTwoAnswer += Cell.Value;

        Console.WriteLine("--------------------------");
        Console.WriteLine("PartOne: {0}", vPartOneAnswer);
        Console.WriteLine("PartTwo: {0}", vPartTwoAnswer);

    }

        static List<long> ApplyMaskV2(string sMask, long lValue)
        {
            StringBuilder sbValue = ApplyMaskGeneral(sMask, lValue, '0');

            int x = sbValue.ToString().Count(n => n == 'X');

            List<long> result = new List<long>();
            int nMaxFluctuation = (int)Math.Pow(2, x);

            for (int i=0; i< nMaxFluctuation; i++)
            {
                long lAddress = Convert.ToInt64(ReplaceXby0or1(sbValue, i, x), 2);
                result.Add(lAddress);
            }
            return result;
        }

        static string ReplaceXby0or1(StringBuilder sbMaskedAddress, int Xreplacement, int x)
        {
            StringBuilder newAddress = new StringBuilder(sbMaskedAddress.ToString());
            string sFluctuation = MASK_36BIT + Convert.ToString(Xreplacement, 2);
            sFluctuation = sFluctuation[^x..];

            int y = 0;
            for(int i = 0; i < newAddress.Length;i++)
                if(newAddress[i] == 'X')
                    newAddress[i] = sFluctuation[y++];

            return newAddress.ToString();
        }

        static long ApplyMaskV1(string sMask, long lValue)
        {
            StringBuilder sbValue = ApplyMaskGeneral(sMask, lValue, 'X');

            long result = Convert.ToInt64(sbValue.ToString(), 2);
            return result;
        }

        private static StringBuilder ApplyMaskGeneral(string sMask,  long lValue, char C)
        {
            string sV = Convert.ToString(lValue, 2);
            StringBuilder sbValue = new StringBuilder(MASK_36BIT, 0, 36 - sV.Length, 36);
            sbValue.Append(sV);
            sMask = sMask[^sbValue.Length..];


            for (int i = 0; i < sbValue.Length; i++)
                if (sMask[i] != C)
                    sbValue[i] = sMask[i];

            return sbValue;
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
