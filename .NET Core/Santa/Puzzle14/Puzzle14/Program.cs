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

        static List<Ratio>[] Resources;

        static void Main(string[] args)
        {
            StreamReader file = new StreamReader(@".\data.txt");
            string data_from_file = file.ReadToEnd();
            string[] lines = data_from_file.Split("\r\n");
            Resources = new List<Ratio>[lines.Length];

            // Filling the array of lists 
            int i = 0;
            List<Ratio> FinalReaction = new List<Ratio>();

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
                        FinalReaction = Resources[i];
                }
                Resources[i].Reverse();
                i++;
            }
            FinalReaction = GenerateTheChain(FinalReaction);

            Console.WriteLine("Ore: {0,-6}", FinalReaction[0].count);
            

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
                int d = temp.FindIndex(n => n.code == R.code);
                if (d< 0)
                    temp.Add(R);
            }
            return temp;
        }

        static List<Ratio> GenerateTheChain(List<Ratio> FinalReaction)
        {
            int n = 0;
            Ratio ElementForForceConversion = new Ratio();
            while (FinalReaction.Count()!=1)
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
                
                n++;
                if (n>10)
                {
                    n = 0;
                    List<Ratio> nonConvertableElements = new List<Ratio>();
                    foreach (Ratio El in FinalReaction)
                        if (El.code != "ORE")
                            nonConvertableElements.Add(El);

                    ElementForForceConversion = GetparentElement(nonConvertableElements);

                    List<Ratio> MainReaction = new List<Ratio>();
                    foreach (List<Ratio> Reaction in Resources)
                    {
                        if (ElementForForceConversion.code == Reaction[0].code)
                        {
                            MainReaction = Reaction;
                            foreach (Ratio Element in MainReaction)
                                if(MainReaction[0].code != Element.code)
                                    FinalReaction.Add(Element);

                            FinalReaction.Remove(ElementForForceConversion);
                            break;
                        }
                    }

                    FinalReaction = Summarize(FinalReaction);

                }
            }
            return FinalReaction;
        }

        static Ratio GetparentElement(List<Ratio> nonConvertableElements)
        {
            List<Ratio> ChildList = new List<Ratio>();
            foreach (Ratio ElementA in nonConvertableElements)
                foreach (Ratio ElementB in nonConvertableElements)
                {
                    if (ElementA.code != ElementB.code)
                    if (IsChild(ElementB, ElementA))
                    {
                         ChildList.Add(ElementB);
                    }
                }

            foreach (Ratio Child in ChildList)
                nonConvertableElements.Remove(Child);

            return nonConvertableElements[0];
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
                                if (IsChild(Child, R))
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
                    if(Element.code == Reaction[0].code && Reaction[0].code != "FUEL")
                    {
                    if (Element.count == Reaction[0].count)
                        foreach (Ratio R in Reaction)
                            Res.Add(R);
                    
                    else if (Element.count % Reaction[0].count == 0)
                        foreach (Ratio R in Reaction)
                        {
                            int nTemp = R.count * (Element.count / Reaction[0].count);
                            Ratio Temp = new Ratio(R.code, nTemp);
                            Res.Add(Temp);
                        }

                    else if (Element.count % Reaction[0].count > 0 && Element.count > Reaction[0].count)
                    {
                        Ratio Temp;
                        foreach (Ratio R in Reaction)
                        {
                            int nTemp = R.count * (Element.count / Reaction[0].count);
                            Temp = new Ratio(R.code, nTemp);
                            Res.Add(Temp);
                        }
                        Temp.code = Element.code;
                        Temp.count = Element.count % Reaction[0].count;
                        Res.Add(Temp);
                        Res.RemoveAt(0);
                    }
                    else    
                        return null;
                    }
            }
           
            Res.Remove(Element);
            return Res;
        }
    }
}
