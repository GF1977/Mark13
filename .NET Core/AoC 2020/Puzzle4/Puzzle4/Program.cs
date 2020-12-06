using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Puzzle4
{
    class Program
{

    public class Passport
    {
            string byr;    // byr(Birth Year)
            string iyr;    // iyr(Issue Year)
            string eyr;    // eyr(Expiration Year)
            string hgt; // hgt(Height)
            string hcl; // hcl(Hair Color)
            string ecl; // ecl(Eye Color)
            string pid; // pid(Passport ID)
            string cid; // cid(Country ID)

            Int64 nFields;

            bool bValid;

            public Passport(string sRawData)
            {
                string[] sArray = sRawData.Split(" ");
                nFields = 0;

                for (int i=1; i< sArray.Length; i++)
                {
                    switch (sArray[i].Substring(0,3))
                    {
                        case "byr":
                            byr = sArray[i].Substring(4);
                            nFields += 1;
                            break;

                        case "iyr":
                            iyr = sArray[i].Substring(4);
                            nFields += 10;
                            break;

                        case "eyr":
                            eyr = sArray[i].Substring(4);
                            nFields += 100;
                            break;

                        case "hgt":
                            hgt = sArray[i].Substring(4);
                            nFields += 1000;
                            break;

                        case "hcl":
                            hcl = sArray[i].Substring(4);
                            nFields += 10000;
                            break;

                        case "ecl":
                            ecl = sArray[i].Substring(4);
                            nFields += 100000;
                            break;

                        case "pid":
                            pid = sArray[i].Substring(4);
                            nFields += 1000000;
                            break;

                        case "cid":
                            cid = sArray[i].Substring(4);
                            nFields += 10000000;
                            break;

                        default:
                            Console.WriteLine("Error: sArray[i] = {0}", sArray[i]);
                            break;
                    }

                }

                if (nFields == 11111111 || nFields == 1111111)
                    bValid = true;
                else
                    bValid = false;
            }

            public bool IsPassportValid()
            {
                return bValid;
            }
    }


    static void Main(string[] args)
    {
        Console.Clear();
        Console.WriteLine(DateTime.Now);
        StreamReader file = new StreamReader(@".\data.txt");

        var nValidPassports = 0;
        var vPartTwoAnswer = "";

        List<Passport> PassList = new List<Passport>();

        string S = "";
        while (!file.EndOfStream)
        {
                string  sTemp = file.ReadLine();
                if (sTemp == "")
                {
                    PassList.Add(new Passport(S));
                    S = "";
                }
                else
                    S = S + " " + sTemp;
        }
        if (S != "")
                PassList.Add(new Passport(S));

            foreach (Passport P in PassList)
                if (P.IsPassportValid()) nValidPassports++;



        Console.WriteLine("--------------------------");
        Console.WriteLine("PartOne: {0}", nValidPassports);
        Console.WriteLine("PartTwo: {0}", vPartTwoAnswer);

    }
}
}
