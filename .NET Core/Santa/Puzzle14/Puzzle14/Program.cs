using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Puzzle14
{
    class Program
    {
        public struct Ratio
        {
            public string code;
            public int count;

            public Ratio(string code, int count)
            {
                this.code = code;
                this.count = count;
            }
        }


        static void Main(string[] args)
        {
            StreamReader file = new StreamReader(@".\data_test.txt");
            string data_from_file = file.ReadToEnd();
            string[] lines = data_from_file.Split("\r\n");
            List<Ratio>[] Resources = new List<Ratio>[lines.Length];

            // Filling the array of lists 
            int i = 0;
            List<Ratio> FuelReaction = new List<Ratio>();

            foreach (string line in lines)
            {
                Resources[i] = new List<Ratio>();
                string[] separators = { "=> ", ", " };
                string[] words = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                foreach (string word in words)
                {
                    string[] tempWords = word.Split(" ");
                    Resources[i].Add(new Ratio(tempWords[1], int.Parse(tempWords[0])));
                    if ((Resources[i].FindIndex(n => n.code.Contains("FUEL")) > 0))
                        FuelReaction = Resources[i];
                }
                Resources[i].Reverse();
                i++;
            }

            // Preliminary step - generate the chains of elements based on the FUEL chain
            List<Ratio> FinalReaction = new List<Ratio>();
            foreach (Ratio Element in FuelReaction)
            {
                if (Element.code != "FUEL")
                    FinalReaction.Add(Element);
            }

            // Need to check that all chains are generated
            do
            {
                FinalReaction = GenerateTheChain(FinalReaction, Resources);
                FinalReaction = Summarize(FinalReaction);
            }
            while (!isReadyToConvert(FinalReaction, Resources));

            // And the last tier - conversion to ORE
            Console.WriteLine("---------- Conversion to ORE -------------");
            int nOre = ConvertToOre(FinalReaction, Resources);



            Console.WriteLine("----------  The Final Result -------------");
            Console.WriteLine("Ore: {0,-6}", nOre);
            

        }


        static bool isReadyToConvert(List<Ratio> FinalReaction, List<Ratio>[] Resources)
        {
            Ratio X = new Ratio();
            bool bReady = true;
            foreach (Ratio ElementA in FinalReaction)
            {
                foreach (List<Ratio> Reaction in Resources)
                {
                    if (Reaction[0].code == ElementA.code)
                        if (Reaction.Count() != 2)
                        {
                            Console.WriteLine("{0} can't be converted to FUEL", ElementA.code);
                            //Console.WriteLine("Conver manually? [Y/N]");
                            //string a = Console.ReadLine().ToUpper();

                            //if (a == "Y")
                            {
                                //X.code = ElementA.code;
                                //if (ElementA.count <= Reaction[0].count)
                                //    X.count = Reaction[0].count;
                                //else
                                //    X.count = ElementA.count + (Reaction[0].count - ElementA.count % Reaction[0].count);

                               // bReady = false;
                            }
                        }
                }
            }
            if (!bReady)
            {
                int nPosition = FinalReaction.FindIndex(n => n.code == X.code);
                Console.WriteLine("Replaced {0} {1} to {2} {3}", FinalReaction[nPosition].code, FinalReaction[nPosition].count,X.code,X.count);
                FinalReaction[nPosition] = X;
            }

            return bReady;
        }

        static List<Ratio> Summarize(List<Ratio> FinalReaction)
        {
            List<Ratio> temp = new List<Ratio>();
            foreach (Ratio ElementA in FinalReaction)
            {
                Ratio R = new Ratio(ElementA.code, 0);
                foreach (Ratio ElementB in FinalReaction)
                {
                    if (ElementA.code == ElementB.code)
                        R.count += ElementB.count;
                }
                int d = temp.FindIndex(n => n.code == R.code);
                if (d< 0)
                {
                    temp.Add(R);
                    Console.WriteLine("{0,-6} {1,6} ", R.code, R.count);
                }
            }
            return temp;
        }

        static List<Ratio> GenerateTheChain(List<Ratio> FinalReaction, List<Ratio>[] Resources)
        {
            int nHitCount = 0;
            while (nHitCount < FinalReaction.Count())
            {
                Console.WriteLine("-----------------------------------");
                nHitCount = 0;
                List<Ratio> tempElement = new List<Ratio>();
                foreach (Ratio Element in FinalReaction)
                {
                    List<Ratio> X = FindReaction(Element, Resources);
                    if (X != null)
                        foreach (Ratio El in X)
                        {
                            if (El.code != Element.code)
                                Console.WriteLine("Element: {0,-6} - {1,6}", El.code, El.count);
                            tempElement.Add(El);
                        }
                    else
                    {
                        Console.WriteLine("Element: {0,-6} - {1,6}", Element.code, Element.count);
                        tempElement.Add(Element);
                        nHitCount++;
                    }
                }
                FinalReaction = tempElement;
            }
            return FinalReaction;
        }

        static int ConvertToOre(List<Ratio> FinalReaction, List<Ratio>[] Resources)
        {
            int nOre = 0;
            foreach (Ratio ElementA in FinalReaction)
            {
                foreach (List<Ratio> Reaction in Resources)
                {
                    if(Reaction[0].code == ElementA.code)
                    {
                        int nRate = ElementA.count  / Reaction[0].count ;
                        if (ElementA.count % Reaction[0].count > 0)
                            nRate++;

                        Console.WriteLine("{0,-6} {1,6}  = ORE {2,6}",ElementA.code, ElementA.count, Reaction[1].count * nRate);
                        nOre += Reaction[1].count * nRate;
                    }
                }

                if (ElementA.code == "ORE")
                    nOre += ElementA.count;
            }
            
            return nOre;
        }
        static List<Ratio> FindReaction(Ratio Element, List<Ratio>[] Resources)
        {
            bool bA = true;
            if (Element.code == "ORE")
                return null;

            List<Ratio> Res = new List<Ratio>();
            foreach (List<Ratio> Reaction in Resources)
            {
                    if(Reaction[0].code == Element.code)
                    {

                    if (Reaction[0].count == Element.count)
                    {
                        foreach (Ratio R in Reaction)
                            Res.Add(R);
                    }

                    else
                    if (Element.count % Reaction[0].count == 0)
                    {
                        foreach (Ratio R in Reaction)
                        {
                            int nTemp = R.count * (Element.count / Reaction[0].count);
                            Ratio Temp = new Ratio(R.code, nTemp);
                            Res.Add(Temp);
                        }
                    }
                    else if (Element.count > Reaction[0].count)
                    {
                        foreach (Ratio R in Reaction)
                        {
                            int nTemp = R.count * (Element.count / Reaction[0].count);
                            Ratio Temp = new Ratio(R.code, nTemp);
                            Res.Add(Temp);
                        }
                        Ratio Temp2 = new Ratio(Element.code, Element.count % Reaction[0].count);
                        Res.Add(Temp2);
                        Res.RemoveAt(0);
                        bA = false;
                    }
                    else
                        Res = null;

                    break;
                    }

            }
            
            if(Res!=null && bA)
                Res.Remove(Element);

            return Res;
        }
    }
}
