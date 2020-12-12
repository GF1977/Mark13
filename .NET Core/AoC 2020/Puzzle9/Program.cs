using System;
using System.IO;
using System.Collections.Generic;


namespace Puzzle9
{
    class Program
{

    const int PREAMBULA = 25;
    static List<Int64> fileInput = new List<Int64>();
    static void Main(string[] args)
    {
        Console.Clear();
        Console.WriteLine(DateTime.Now);
        StreamReader file = new StreamReader(@".\data.txt");

        var vPartOneAnswer = "";
        var vPartTwoAnswer = "";



        while (!file.EndOfStream)
        {
            string S = file.ReadLine();
            fileInput.Add(Int64.Parse(S));
        }


            for (int i = PREAMBULA; i < fileInput.Count; i++)
            {
                if(!IsValidNumber(i))
                    Console.WriteLine("PartOne: {0}", fileInput[i]);
            }

        Console.WriteLine("--------------------------");
//        Console.WriteLine("PartOne: {0}", vPartOneAnswer);
//        Console.WriteLine("PartTwo: {0}", vPartTwoAnswer);

    }

    static bool IsValidNumber(int N)
        {
            
            for (int x = N - PREAMBULA; x < N; x++)
                for (int y = x + 1; y < N; y++)
                    if (fileInput[N] == fileInput[x] + fileInput[y]) return true;
            
            return false;
        }
}
}
