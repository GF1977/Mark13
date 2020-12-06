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

            int nYesAnswers = 0;
            foreach(string sAnswer in fileInput)
            {
                nYesAnswers += QuestionsAnsweredYes(sAnswer);
            }

        Console.WriteLine("--------------------------");
        Console.WriteLine("PartOne: {0}", nYesAnswers);
        Console.WriteLine("PartTwo: {0}", vPartTwoAnswer);

    }
        public static int QuestionsAnsweredYes(string S)
        {
            return S.ToLower().ToCharArray().Where(c => c <= 'z' && c >= 'a').Distinct().Count();
        }
    
}
}
