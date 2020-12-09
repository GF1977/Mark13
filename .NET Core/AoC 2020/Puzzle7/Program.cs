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
            public int quantity;
            public string MainBagsColor;

            public List<BagsPolicy> ChildBags = new List<BagsPolicy>();

            public BagsPolicy()
            {
                MainBagsColor = "";
                quantity = 0;
            }
            public BagsPolicy(BagsPolicy Source)
            {
                MainBagsColor = Source.MainBagsColor;
                quantity = Source.quantity;

                
               foreach (BagsPolicy Child in Source.ChildBags)
                    ChildBags.Add(Child);
            }


            public BagsPolicy(string S)
            {
                string[] sParsedPolicy;
                if (S.Contains("contain no other"))
                {
                    sParsedPolicy = S.Split(' ');
                    MainBagsColor = sParsedPolicy[0] + " " + sParsedPolicy[1];

                }
                else
                {
                    sParsedPolicy = S.Split(" bags contain ");
                    MainBagsColor = sParsedPolicy[0];

                    sParsedPolicy = sParsedPolicy[1].Split(", ");
                    foreach (string sChild in sParsedPolicy)
                    {
                        BagsPolicy BPchild = new BagsPolicy();
                        string[] sChildDetails = sChild.Split(' ');
                        BPchild.MainBagsColor = sChildDetails[1] + " " + sChildDetails[2];
                        BPchild.quantity = int.Parse(sChildDetails[0]);

                        ChildBags.Add(BPchild);
                    }
                }
                quantity = 1;
                
            }
        }

    static void Main()
    {
        Console.Clear();
        Console.WriteLine(DateTime.Now);
        StreamReader file = new StreamReader(@".\data.txt");

        while (!file.EndOfStream)
        {

                string S = file.ReadLine();
                BagsPolicy BP = new BagsPolicy(S);
                Policies.Add(BP);

            }

            
            string sBag = "shiny gold";

            // Part One
            WhereTheBag(sBag);

            Console.WriteLine("--------------------------");
            Console.WriteLine("PartOne: {0}", PoliciesResults.Count);

            // Part two (recursive)
            var vPartTwoAnswer2 = ContainBagRecursive(sBag);

            Console.WriteLine("PartTwo (recursive): {0}", vPartTwoAnswer2);

        }

        public static void WhereTheBag(string sColor)
        {
            foreach (BagsPolicy MainBag in Policies)
                foreach (BagsPolicy Child in MainBag.ChildBags)
                    if (Child.MainBagsColor == sColor && PoliciesResults.Find(n=>n== MainBag) ==null)
                        {
                            PoliciesResults.Add(MainBag);
                        WhereTheBag(MainBag.MainBagsColor);
                        }

        }

        
        public static int ContainBagRecursive(string sColor)
        {
            BagsPolicy MainBag = Policies.Find(n => n.MainBagsColor == sColor);

            int i = 0;
            while (i< MainBag.ChildBags.Count)
            {
                    BagsPolicy FirstChildpolicy = Policies.Find(n => n.MainBagsColor == MainBag.ChildBags[i].MainBagsColor);
                    int nFirstChildQuantity = MainBag.ChildBags[i].quantity;
                    foreach (BagsPolicy InnerBag in FirstChildpolicy.ChildBags)
                    {
                        BagsPolicy BP = new BagsPolicy(InnerBag);
                        BP.quantity *= nFirstChildQuantity;
                        MainBag.ChildBags.Add(BP);
                    }
                    i++;
            }

            int nRes = 0;
            foreach (BagsPolicy BP in MainBag.ChildBags)
                nRes += BP.quantity;

            return nRes;
        }
    }
}
