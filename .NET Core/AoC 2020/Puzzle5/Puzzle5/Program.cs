using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Puzzle5
{
    public static class Program
    {

    static void Main(string[] args)
    {
        Console.Clear();
        Console.WriteLine(DateTime.Now);
        StreamReader file = new StreamReader(@".\data.txt");

        var vPartTwoAnswer = "";

        List<string> BoardPass = new List<string>();
        List<int> ListSitId = new List<int>();

            while (!file.EndOfStream)
        {
            string S = file.ReadLine();
                BoardPass.Add(S);
        }

        int nMaxSitId = 0;
        foreach(string S in BoardPass)
            {
                int nSitId = GetSitId(S);
                if (nSitId > nMaxSitId)
                    nMaxSitId = nSitId;
                
                ListSitId.Add(nSitId);
            }


        ListSitId.Sort();

            int nMySitId = 0;
            for(int i = 1; i < ListSitId.Count; i++)
            {
                if (ListSitId[i - 1] + 2 == ListSitId[i])
                    nMySitId = ListSitId[i] - 1;
            }




        Console.WriteLine("--------------------------");
        Console.WriteLine("PartOne: {0}", nMaxSitId);
        Console.WriteLine("PartTwo: {0}", nMySitId);

    }

        public static int GetSitId(string sBoardPass)
        {

            int nRow = 0;
            int nColumn = 0;

            int nRank = 0;
            foreach(char C in sBoardPass)
            {
                if (C == 'B')
                    nRow    += (int)Math.Pow(2, 6 - nRank);

                if (C == 'R')
                    nColumn += (int)Math.Pow(2, 9 - nRank);

                nRank++;
            }

            return nRow *  8 + nColumn;
        }
}
}
