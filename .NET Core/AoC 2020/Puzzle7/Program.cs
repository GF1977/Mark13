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
        public static List<string> Bags = new List<string>();
        public static int nBagsCount = 0;
        public class BagsPolicy
        {
            public string MainBagsColor;
            public int quantity;
            public bool bHasChilds;

            public List<BagsPolicy> ChildBags = new List<BagsPolicy>();

            public bool ContainBag(string sColor)
            {
                foreach (BagsPolicy Child in ChildBags)
                    if (Child.MainBagsColor == sColor)
                        return true;

                return false;
            }

            public BagsPolicy()
            {
                MainBagsColor = "";
                quantity = 0;
                bHasChilds = false;
            }
            public BagsPolicy(BagsPolicy Source)
            {
                MainBagsColor = Source.MainBagsColor;
                quantity = Source.quantity;
                bHasChilds = Source.bHasChilds;
                
                    foreach (BagsPolicy Child in Source.ChildBags)
                    ChildBags.Add(Child);
            }
        }

    static void Main(string[] args)
    {
        Console.Clear();
        Console.WriteLine(DateTime.Now);
        StreamReader file = new StreamReader(@".\data.txt");

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
                BP.quantity = 1;
                Policies.Add(BP);

            }

            
            string sBag = "shiny gold";

            // Part One
            WhereTheBag(sBag);

            Console.WriteLine("--------------------------");
            Console.WriteLine("PartOne: {0}", PoliciesResults.Count);

            // Part two
            PoliciesResults = new List<BagsPolicy>();
            var vPartTwoAnswer = ContainBag(sBag);

            Console.WriteLine("PartTwo: {0}", vPartTwoAnswer);

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
            int nRes = 0;

            BagsPolicy MainBag = Policies.Find(n => n.MainBagsColor == sColor);

            while (nRes< MainBag.ChildBags.Count)
            {

                if (MainBag.ChildBags.Count > 0)
                {
                    BagsPolicy FirstChild = Policies.Find(n => n.MainBagsColor == MainBag.ChildBags[nRes].MainBagsColor);
                    int nFirstChildQuantity = MainBag.ChildBags[nRes].quantity;
                    foreach (BagsPolicy InnerBag in FirstChild.ChildBags)
                    {
                        BagsPolicy BP = new BagsPolicy(InnerBag);
                        BP.quantity *= nFirstChildQuantity;
                        MainBag.ChildBags.Add(BP);
                    }
                    //MainBag.ChildBags.RemoveAt(0);
                    nRes++;
                }
                else
                    break;

            }


            nRes = 0;

            foreach (BagsPolicy BP in MainBag.ChildBags)
                nRes += BP.quantity;

            return nRes;
        }

        public static int ContainBag(string sColor)
        {
            int nRes = 0;
            int i = 0;
            BagsPolicy MainBag = Policies.Find(n => n.MainBagsColor == sColor);

            do
            {
                if (MainBag.ChildBags.Count == 0)
                    i++;

                else
                {
                    foreach (BagsPolicy Child in MainBag.ChildBags)
                    {
                        int nCount = Child.quantity * MainBag.quantity;

                        BagsPolicy TempBag = new BagsPolicy(Policies.Find(n => n.MainBagsColor == Child.MainBagsColor));
                        TempBag.quantity = nCount;
                        PoliciesResults.Add(TempBag);
                    }

                    sColor = PoliciesResults[i].MainBagsColor;
                    MainBag = PoliciesResults[i];
                    PoliciesResults.RemoveAt(i);

                    if (MainBag.ChildBags.Count != 0)
                        nRes += MainBag.quantity;
                }
            }
            while (i <= PoliciesResults.Count);
            
            PoliciesResults.Add(MainBag);

            foreach (BagsPolicy BP in PoliciesResults)
                nRes += BP.quantity;

            return nRes;
        }


    }
}
