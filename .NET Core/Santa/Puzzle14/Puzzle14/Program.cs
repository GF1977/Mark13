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
            public long count;
            public Ratio(string code, long count, bool child = false)
            {
                this.code = code;
                this.count = count;
            }
        }

        static List<Ratio>[] Resources;

        static void Main(string[] args)
        {
            StreamReader file = new StreamReader(@".\data_test.txt");
            string data_from_file = file.ReadToEnd();
            string[] lines = data_from_file.Split("\r\n");
            Resources = new List<Ratio>[lines.Length];

            // Filling the array of lists 
            long i = 0;
            List<Ratio> VanilaReaction = new List<Ratio>();

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
                        VanilaReaction = Resources[i];
                }
                Resources[i].Reverse();
                i++;
            }



            // Part One
            Console.WriteLine("-- Part One --");
            List<Ratio> FinalReaction = VanilaReaction;
            long theAnswer1 = GenerateTheChain(FinalReaction);
            Console.WriteLine("Ore: {0,-6}", theAnswer1);
            Console.WriteLine("");

            //Part TWO

            Console.WriteLine("-- Part Two --");
            const long ORE = 1000000000000;
            long nFuel = ORE / theAnswer1; // Guess amount of Fuel for beginning
            long nFuelIncremental = 1024; // step for the fast search
            long producedOre = 0;

            while (nFuelIncremental > 0)
            {
                nFuel += nFuelIncremental;
                VanilaReaction[0] = new Ratio("FUEL", nFuel);
                producedOre = GenerateTheChain(VanilaReaction);
                Console.WriteLine("Fuel: {0,-10} Ore: {1,-10} Step: {2,-5}", nFuel, producedOre, nFuelIncremental);
                if (producedOre > ORE)
                {
                    nFuel -= nFuelIncremental;  // too much, step back and decrease the incremental value
                    nFuelIncremental /= 2;
                }
                else
                    nFuelIncremental *= 2; //too smal, increase the incremental value
            }

            Console.WriteLine("Final answer: Fuel: {0,-6}", nFuel);
        }


        static List<Ratio> Summarize(List<Ratio> FinalReaction)
        {
            List<Ratio> temp = new List<Ratio>();
            foreach (Ratio ElementA in FinalReaction)
            {
                Ratio R = new Ratio(ElementA.code, 0);
                foreach (Ratio ElementB in FinalReaction)
                    if (ElementA.code == ElementB.code)
                        R.count += ElementB.count;
                if (temp.FindIndex(n => n.code == R.code) < 0)
                    temp.Add(R);
            }
            return temp;
        }

        static long GenerateTheChain(List<Ratio> VanilaReaction)
        {
            // if Fuel > 1
            List<Ratio> FinalReaction = new List<Ratio>();
            foreach (Ratio X in VanilaReaction)
                FinalReaction.Add(new Ratio(X.code, X.count * VanilaReaction[0].count));

            while (FinalReaction.Count() != 1)
            {
                long nCount = -1;
                while (nCount != FinalReaction.Count())
                {
                    List<Ratio> tempElement = new List<Ratio>();
                    foreach (Ratio Element in FinalReaction)
                    {
                        List<Ratio> X = FindReaction(Element);
                        if (X != null)
                            foreach (Ratio El in X)
                                tempElement.Add(El);
                        else
                            tempElement.Add(Element);
                    }
                    FinalReaction = Summarize(tempElement);
                    nCount = tempElement.Count();
                }
                FinalReaction = GetReactionWithForceConversion(FinalReaction);
            }
            return FinalReaction[0].count;
        }

        static List<Ratio> GetReactionWithForceConversion(List<Ratio> FinalReaction)
        {
            List<Ratio> nonConvertableElements = new List<Ratio>();
            foreach (Ratio El in FinalReaction)
                if (El.code != "ORE")
                    nonConvertableElements.Add(El);

            Ratio ElementForForceConversion = GetparentElement(nonConvertableElements);

            List<Ratio> MainReaction;
            if (ElementForForceConversion.code != "")
                foreach (List<Ratio> Reaction in Resources)
                {
                    if (ElementForForceConversion.code == Reaction[0].code)
                    {
                        MainReaction = Reaction;
                        foreach (Ratio Element in MainReaction)
                            if (MainReaction[0].code != Element.code)
                                FinalReaction.Add(Element);

                        FinalReaction.Remove(ElementForForceConversion);
                        break;
                    }
                }

            FinalReaction = Summarize(FinalReaction);
            return FinalReaction;
        }
        static Ratio GetparentElement(List<Ratio> nonConvertableElements)
        {
            foreach (Ratio X in nonConvertableElements)
            {
                long nCount = nonConvertableElements.Count();
                foreach (Ratio Y in nonConvertableElements)
                    if (!IsChild(X, Y))
                        nCount--;

                if (nCount == 1) // 1 because IsChild(A,A) returns true 
                    return X;
            }
            return new Ratio("", 0);
        }

        static bool IsChild(Ratio Child, Ratio Parent)
        {
            if (Child.code == Parent.code)
                return true;

            else
                foreach (List<Ratio> Reaction in Resources)
                    if (Reaction[0].code == Parent.code)
                        foreach (Ratio R in Reaction)
                            if (R.code != Parent.code)
                                if (IsChild(Child, R))  // reccurent scanning of the chain tree
                                    return true;
            return false;
        }
        static List<Ratio> FindReaction(Ratio Element)
        {
            if (Element.code == "ORE")
                return null;

            List<Ratio> Res = new List<Ratio>();
            foreach (List<Ratio> Reaction in Resources)
            {
                Ratio TopElement = Reaction[0];
                if (Element.code == TopElement.code && TopElement.code != "FUEL")
                {
                    if (Element.count % TopElement.count == 0)
                    {
                        foreach (Ratio R in Reaction)
                            Res.Add(new Ratio(R.code, R.count * (Element.count / TopElement.count)));

                        Res.Remove(Element); //as we replaced the element by its children
                    }
                    else if (Element.count % TopElement.count > 0 && Element.count > TopElement.count)
                    {
                        foreach (Ratio R in Reaction)
                            Res.Add(new Ratio(R.code, R.count * (Element.count / TopElement.count)));

                        Res.Add(new Ratio(Element.code, Element.count % TopElement.count)); // adding the rest of the parent element
                        Res.RemoveAt(0); // removing the original parent
                    }
                    else
                        return null;
                }
            }


            return Res;
        }
    }
}
