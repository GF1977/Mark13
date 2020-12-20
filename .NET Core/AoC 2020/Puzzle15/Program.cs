using System;
using System.Collections.Generic;
using System.Linq;

namespace MemoryGame
{
    public class Program
    {

        static void Main()
        {
            Console.Clear();
            Console.WriteLine(DateTime.Now);

            string myInput = "19,20,14,0,9,1";

            var vPartOneAnswer = GetNumber(2020, myInput);
            var vPartTwoAnswer = GetNumber(30000000, myInput);

            Console.WriteLine("--------------------------");
            Console.WriteLine("PartOne: {0}", vPartOneAnswer);
            Console.WriteLine("PartTwo: {0}", vPartTwoAnswer);
        }

        public static long GetNumber(long nEnd, string myInput)
        {
            long x = 0;
            string[] preParsingString = myInput.Split(",");

            // Item1 = position A
            // Item2 = position B
            // Item3 = counter
            Dictionary<long, Tuple<long, long, long>> Numbers = new Dictionary<long, Tuple<long, long, long>>();

            foreach (string S in preParsingString)
            {
                Numbers.Add(long.Parse(S), new Tuple<long, long, long>(0, x + 1, 1));
                x++;
            }

            long nTheLastNumberSpoken = Numbers.Last().Key;
            long nNumberToSay;
            while (x++ < nEnd)
            {
                bool bNumberWasThere = Numbers.TryGetValue(nTheLastNumberSpoken, out Tuple<long, long, long> PositionAndCount);
                if (bNumberWasThere && PositionAndCount.Item3 > 1)
                   nNumberToSay = x - 1 - PositionAndCount.Item1;
                else
                    nNumberToSay = 0;

                if (Numbers.TryGetValue(nNumberToSay, out PositionAndCount))
                {
                    Numbers.Remove(nNumberToSay);
                    Numbers.Add(nNumberToSay, new Tuple<long, long, long>(PositionAndCount.Item2, x, PositionAndCount.Item3 + 1));
                }
                else
                    Numbers.Add(nNumberToSay, new Tuple<long, long, long>(x, x, 1));

                nTheLastNumberSpoken = nNumberToSay;
            }

            return nTheLastNumberSpoken;
        }
    }
}
