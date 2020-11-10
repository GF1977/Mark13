using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Puzzle22
{
    class Program
    {

        static void Main(string[] args)
        {
            int nMaxCards = 10007; // standard is 10007

            int nCardValue = 2019;
            List<int> cards = new List<int>();
            for (int i = 0; i < nMaxCards; i++)
                cards.Add(i);


            foreach (string line in File.ReadLines(@".\data.txt")) 
            {
                string[] words = line.Split(' ');
                switch (words[0])
                {
                    case "deal":
                        if(words[1] == "with")
                            DealWithIncremental(ref cards, int.Parse(words[3]));
                        else
                            NewStack(ref cards);
                        break;
                    case "cut":
                        Cut(ref cards, int.Parse(words[1]));
                        break;
                }
            }

            if (nCardValue == 0) // test scenarious
                foreach (int n in cards)
                {
                    Console.Write("{0} ", n);
                }

            else
            {
                Console.WriteLine("Card {0} is on position {1}", nCardValue, cards.FindIndex(n => n == nCardValue));
            }

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
            //Console.WriteLine("Cut {0}", nCut);
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