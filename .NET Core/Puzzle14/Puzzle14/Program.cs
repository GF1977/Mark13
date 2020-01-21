using System;
using System.IO;
using System.Collections.Generic;
using System.Collections;
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
                this.code  = code;
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
            foreach (string line in lines)
            {
                Resources[i] = new List<Ratio>();
                string[] separators = { "=> ", ", " };
                string[] words = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                foreach (string word in words)
                {
                    string[] tempWords = word.Split(" ");
                    Resources[i].Add(new Ratio(tempWords[1], int.Parse(tempWords[0])));
                }
                Resources[i].Reverse();
                i++;
            }

            List<Ratio> FuelReaction  = new List<Ratio>();
            List<Ratio> FinalReaction = new List<Ratio>();

            foreach (List<Ratio> Reaction in Resources)
            {
                int a = Reaction.FindIndex(n => n.code.Contains("FUEL"));
                if (a>=0)
                {
                    FuelReaction = Reaction;
                    break;
                }
            }

            foreach (Ratio Element in FuelReaction)
            {
                if(Element.code != "FUEL")
                    FinalReaction.Add(Element);
            }


            
            int nHitCount = 0;
            int nHitCountMax = -1;
            bool bStop = false;
            while (!bStop)
            {
                nHitCountMax = nHitCount;
                nHitCount = 0;
                List<Ratio> tempElement = new List<Ratio>();
                foreach (Ratio Element in FinalReaction)
                {
                    List<Ratio> X = FindReaction(Element, Resources);
                    if (X != null)
                        foreach (Ratio El in X)
                        {
                            if (El.code != Element.code)
                                Console.WriteLine("Element: {0} - {1}", El.code, El.count);
                            tempElement.Add(El);
                        }
                    else
                    {
                        Console.WriteLine("Element: {0} - {1}", Element.code, Element.count);
                        tempElement.Add(Element);
                        nHitCount++;
                    }
                }
                Console.WriteLine("---------------- nHitCount = {0} ---------------", nHitCount);
                if (nHitCount == nHitCountMax) bStop = true;
                FinalReaction = tempElement;
            }

            Console.WriteLine("-----------------------------------");
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
                if (d < 0)
                {
                    temp.Add(R);
                    Console.WriteLine("{0} {1}", R.code, R.count);
                }
            }

            Console.WriteLine("-----------------------------------");
            int nOre = ConvertToOre(temp, Resources);
            Console.WriteLine("Ore: {0}",nOre);

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
                    else
                        Res = null;

                    break;
                    }

            }
            
            if(Res!=null)
                Res.Remove(Element);

            return Res;
        }
    }
}
