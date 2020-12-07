using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Puzzle7
{
    class Program
{
        public static List<BagsPolicy> PoliciesResults = new List<BagsPolicy>();
        public static List<BagsPolicy> Policies = new List<BagsPolicy>();
        public class BagsPolicy
        {
            public string MainBagsColor;
            public int quantity;
            public List<BagsPolicy> ChildBags = new List<BagsPolicy>();
            public bool bHasChilds;


            public bool ContainBag(string sColor)
            {
                foreach (BagsPolicy Child in ChildBags)
                    if (Child.MainBagsColor == sColor)
                        return true;

                return false;
            }

        }

    static void Main(string[] args)
    {
        Console.Clear();
        Console.WriteLine(DateTime.Now);
        StreamReader file = new StreamReader(@".\data.txt");



        var vPartOneAnswer = "";
        var vPartTwoAnswer = "";



        while (!file.EndOfStream)
        {
                BagsPolicy BP = new BagsPolicy();
                

                string S = file.ReadLine();
                string[] sParsedPolicy;
                if (S.Contains("contain no other"))
                {
                    sParsedPolicy = S.Split(' ');
                    BP.MainBagsColor = sParsedPolicy[0] + " " + sParsedPolicy[1];
                    BP.bHasChilds = false;
                }
                else
                {
                    sParsedPolicy = S.Split(" bags contain ");
                    BP.MainBagsColor = sParsedPolicy[0];

                    sParsedPolicy = sParsedPolicy[1].Split(", ");

                    BP.bHasChilds = true;
                    foreach (string sChild in sParsedPolicy)
                    {
                        BagsPolicy BPchild = new BagsPolicy();
                        string[] sChildDetails = sChild.Split(' ');
                        BPchild.MainBagsColor = sChildDetails[1] + " " + sChildDetails[2];
                        BPchild.quantity = int.Parse(sChildDetails[0]);

                        BP.ChildBags.Add(BPchild);
                    }
                }
                Policies.Add(BP);

            }

            string sBag = "shiny gold";
            ContainBag(sBag);



        Console.WriteLine("--------------------------");
        Console.WriteLine("PartOne: {0}", PoliciesResults.Count);
        Console.WriteLine("PartTwo: {0}", vPartTwoAnswer);

    }

        public static void ContainBag(string sColor)
        {
            foreach (BagsPolicy MainBag in Policies)
                foreach (BagsPolicy Child in MainBag.ChildBags)
                    if (Child.MainBagsColor == sColor && PoliciesResults.Find(n=>n== MainBag) ==null)
                        {
                            PoliciesResults.Add(MainBag);
                            ContainBag(MainBag.MainBagsColor);
                        }

        }


    }
}
