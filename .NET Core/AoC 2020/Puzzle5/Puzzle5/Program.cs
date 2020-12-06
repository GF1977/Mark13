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

        while (!file.EndOfStream)
        {
            string S = file.ReadLine();
                BoardPass.Add(S);
        }

        int nMaxTicketId = 0;
        foreach(string S in BoardPass)
            {
                int nTicketId = GetTicketId(S);
                if (nTicketId > nMaxTicketId)
                    nMaxTicketId = nTicketId;
            }


        Console.WriteLine("--------------------------");
        Console.WriteLine("PartOne: {0}", nMaxTicketId);
        Console.WriteLine("PartTwo: {0}", vPartTwoAnswer);

    }

        public static int GetTicketId(string sBoardPass)
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
