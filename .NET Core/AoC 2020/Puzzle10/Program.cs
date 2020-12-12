using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Puzzle10
{
    class Program
{
        static List<int> Adapters = new List<int>();
        static void Main()
    {
        Console.Clear();
        Console.WriteLine(DateTime.Now);

        var vPartTwoAnswer = "";

        using (StreamReader file = new StreamReader(@".\data.txt"))
        while (!file.EndOfStream)
        {
            string S = file.ReadLine();
                    Adapters.Add(int.Parse(S));
        }

            Adapters.Sort();


        Console.WriteLine("--------------------------");
        Console.WriteLine("PartOne: {0}", JoltsDiff());
        Console.WriteLine("PartTwo: {0}", vPartTwoAnswer);

    }


    static int JoltsDiff()
        {
            int Diff1Jolts = 1;
            int Diff3Jolts = 1;

            int n = 0;
            while(n<Adapters.Count-1)
            {
                if (Math.Abs(Adapters[n] - Adapters[n + 1]) == 1)
                    Diff1Jolts++;
                else if (Math.Abs(Adapters[n] - Adapters[n + 1]) == 3)
                    Diff3Jolts++;
                else
                    break;
                n++;
            }

            return Diff1Jolts * Diff3Jolts;
        }

}
}
