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


        using (StreamReader file = new StreamReader(@".\data.txt"))
        while (!file.EndOfStream)
        {
            string S = file.ReadLine();
                    Adapters.Add(int.Parse(S));
        }

            Adapters.Add(0);
            Adapters.Sort();

            List<int> Chain = new List<int>();
            int i = 0;

            Int64 PartTwoAnswer = 1;

            // we need to find all consequences with Len >= 3, as them can be replaced by other consequences
            // 1,2,3 = 1,2,3 ; 1,3
            // 1,2,3,4 = 1,2,3,4 ; 1,2,4 ; 1,3,4; 1,4
            while (i<Adapters.Count-1)
            {
                while (i < Adapters.Count - 1 && Adapters[i] == Adapters[i + 1]-1)
                    Chain.Add(Adapters[i++]);

                Chain.Add(Adapters[i]);

                if (Chain.Count >= 3)
                {
                    // did some pen & paper research to get those magic numbers
                    // the piece of chain with Len = X can be replaced by Y combinations
                    // X = 4 -> Y = 4
                    // 1 2 3 4 ; 1 2 4 ; 1 3 4; 1 4
                    switch (Chain.Count)
                    {
                        case 3:
                            PartTwoAnswer *= 2; // Y = 2
                            break;
                        case 4:
                            PartTwoAnswer *= 4; // Y = 4
                            break;
                        case 5:
                            PartTwoAnswer *= 7; // Y = 7
                            break;
                        default:
                            break;
                    }


                }
                Chain.Clear();
                i++;
            }


        Console.WriteLine("--------------------------");
        Console.WriteLine("PartOne: {0}", JoltsDiff());
        Console.WriteLine("PartTwo: {0}", PartTwoAnswer);

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
