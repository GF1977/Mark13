using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Puzzle6
{
    class Program
{

    static void Main(string[] args)
    {
        Console.Clear();
        Console.WriteLine(DateTime.Now);
        StreamReader file = new StreamReader(@".\data.txt");

        var vPartTwoAnswer = "";

        List<string> fileInput = new List<string>();

            string S = "";
            while (!file.EndOfStream)
            {
                string sTemp = file.ReadLine();
                if (sTemp == "")
                {
                    fileInput.Add(S);
                    S = "";
                }
                else
                    S = S + " " + sTemp;
            }
            if (S != "")
                fileInput.Add(S);

            int nYesAnsweredByAnyone = 0;
            int nYesAnsweredByEveryone = 0;
            foreach (string sAnswer in fileInput)
            {
                nYesAnsweredByAnyone    += AnyoneAnsweredYes(sAnswer);
                nYesAnsweredByEveryone  += EveryoneAnsweredYes(sAnswer);
            }

        Console.WriteLine("--------------------------");
        Console.WriteLine("PartOne: {0}", nYesAnsweredByAnyone);
        Console.WriteLine("PartTwo: {0}", nYesAnsweredByEveryone);

    }
        public static int AnyoneAnsweredYes(string S)
        {
            return S.ToLower().ToCharArray().Where(c => c <= 'z' && c >= 'a').Distinct().Count();
        }


        public static int EveryoneAnsweredYes(string S)
        {
            int nResult = 0;
            int nPassengers = S.Count(n => n == ' ');
            S = S.ToLower();

            for (char c = 'a'; c <= 'z'; c++)
                if (S.Count(n => n == c) == nPassengers)
                    nResult++;

            return nResult;
        }

    }
}
