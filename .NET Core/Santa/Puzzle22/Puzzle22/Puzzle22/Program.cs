using System;
using System.Collections.Generic;
using System.IO;

namespace Puzzle22
{
    class Program
    {

        static void Main(string[] args)
        {

            List<int> cards = new List<int>();
            for (int i = 0; i < 10007; i++)
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





        }



        static void DealWithIncremental(ref List<int> cards, int nIncrement)
        {
            Console.WriteLine("DealWithIncremental {0}", nIncrement);

        }

        static void Cut(ref List<int> cards, int nCut)
        {
            Console.WriteLine("Cut {0}", nCut);

        }


        static void NewStack(ref List<int> cards)
        {
            Console.WriteLine("NewStack");

        }

    }
}