using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Puzzle22
{
    class Program
    {
        static readonly Int64 nMaxCards = 10007;  //119315717514047 - for part 2  // standard is 10007
        static readonly Int64 nIterations  = 101741582076661; // Part 2 - how many iterations we need to do


        static void Main()
        {
            int nCardValue = 7777;
            Int64 nCardPosition = 2020;



            List<string> commands = new List<string>();
            List<int> cards = new List<int>();
            for (int i = 0; i < nMaxCards; i++)
                cards.Add(i);


            foreach (string line in File.ReadLines(@".\data.txt"))
                commands.Add(line);


            // PART #1
            Console.WriteLine("Part One");
            foreach (string line in commands)
            {
                string[] words = line.Split(' ');
                switch (words[0])
                {
                    case "deal":
                        if (words[1] == "with")
                            DealWithIncremental(ref cards, int.Parse(words[3]));
                        else
                            NewStack(ref cards);
                        break;
                    case "cut":
                        Cut(ref cards, int.Parse(words[1]));
                        break;
                }

                nCardPosition = cards.FindIndex(n => n == nCardValue);
              //  Console.WriteLine("Card {0} is on position {1}", nCardValue, nCardPosition);

            }
            nCardPosition = cards.FindIndex(n => n == nCardValue);
            //Console.WriteLine("-------------------------------------------");
            Console.WriteLine("Card {0} is on position {1}", nCardValue, nCardPosition);

            // PART #2
            // Reverse order
            Console.WriteLine();
            Console.WriteLine("Part Two");


            Int64 nTempPosition = nCardPosition;
            for (int i = commands.Count - 1; i>=0;i--)
            {
                //Console.WriteLine("Checking position {0}", nCardPosition);
                string line = commands[i];
                string[] words = line.Split(' ');
                switch (words[0])
                {
                    case "deal":
                        if (words[1] == "with")
                            nCardPosition = DealWithIncrementalReverse(nCardPosition, int.Parse(words[3]));
                        else
                            nCardPosition = NewStackReverse(nCardPosition);
                        break;
                    case "cut":
                        nCardPosition = CutReverse(nCardPosition, int.Parse(words[1]));
                        break;
                }
            }
            //Console.WriteLine("-------------------------------------------");
            Console.WriteLine("On position {0} is Card {1}", nTempPosition, nCardPosition);

        }

        static Int64 NewStackReverse(Int64 nPosition)
        {
            return nMaxCards - nPosition - 1;
            //return nPosition;
        }

        static Int64 CutReverse(Int64 nPosition, Int64 nCut)
        {
            return (nPosition + nCut + nMaxCards) % nMaxCards;
            //return nPosition ;
        }

        static Int64 DealWithIncrementalReverse(Int64 nPosition, int nIncrement)
        {
            for (Int64 i = 0; i < nMaxCards; i++)
            {
                if ((i * nIncrement) % nMaxCards == nPosition)
                {
                    nPosition = i;
                    break;
                }
            }
            return nPosition;
        }

        static void DealWithIncremental(ref List<int> cards, int nIncrement)
        {
            //Console.WriteLine("DealWithIncremental {0}", nIncrement);
            List<int> lTemp = new List<int>();
            for (int n = 0; n < cards.Count; n++)
                lTemp.Add(-1);

            int nCardPosition = 0;
            int i = 0;
            while (nCardPosition < cards.Count)
            {
                lTemp[i] = cards[nCardPosition];
                nCardPosition++;
                i = (i + nIncrement) % cards.Count;
            }
            cards = lTemp;
        }
        static void Cut(ref List<int> cards, int nCut)
        {
          //  Console.WriteLine("Cut {0}", nCut);
            if (nCut < 0)
                nCut += cards.Count;

            List<int> lTemp = cards.GetRange(0, nCut);
            cards.RemoveRange(0, nCut);
            cards.AddRange(lTemp);
        }
        static void NewStack(ref List<int> cards)
        {
            //Console.WriteLine("NewStack");
            cards.Reverse();
        }



    }
}