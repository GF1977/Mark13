using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Puzzle4
{
    internal class Program
{

    public class Passport
    {
            private string byr;    // byr(Birth Year)
            private string iyr;    // iyr(Issue Year)
            private string eyr;    // eyr(Expiration Year)
            private string hgt; // hgt(Height)
            private string hcl; // hcl(Hair Color)
            private string ecl; // ecl(Eye Color)
            private string pid; // pid(Passport ID)
            private string cid; // cid(Country ID)

            private Int64 nFields;
            private bool  bValid;

            public Passport(string sRawData)
            {
                string[] sArray = sRawData.Split(" ");
                nFields = 0;

                for (int i=1; i< sArray.Length; i++)
                {
                    string[] sKeyValue = sArray[i].Split(":");
                    switch (sKeyValue[0])
                    {
                        case "byr":
                            byr = sKeyValue[1];
                            nFields += 1;
                            break;

                        case "iyr":
                            iyr = sKeyValue[1];
                            nFields += 10;
                            break;

                        case "eyr":
                            eyr = sKeyValue[1];
                            nFields += 100;
                            break;

                        case "hgt":
                            hgt = sKeyValue[1];
                            nFields += 1000;
                            break;

                        case "hcl":
                            hcl = sKeyValue[1];
                            nFields += 10000;
                            break;

                        case "ecl":
                            ecl = sKeyValue[1];
                            nFields += 100000;
                            break;

                        case "pid":
                            pid = sKeyValue[1];
                            nFields += 1000000;
                            break;

                        case "cid":
                            cid = sKeyValue[1];
                            nFields += 10000000;
                            break;

                        default:
                            Console.WriteLine("Error: sArray[i] = {0}", sArray[i]);
                            break;
                    }

                }

                bValid = PassValidation();
            }


            public bool BasicValidation()
            {
                // main check
                if (nFields == 11111111 || nFields == 1111111)
                    return true;
                else
                    return false;
            }

            private bool PassValidation()
            {
                bool bResult = false;

                if (BasicValidation()) bResult = true;
                else return false;

                // byr (Birth Year) - four digits; at least 1920 and at most 2002.
                if (IsYearValid(byr, 1920, 2002)) bResult = true;
                else return false;

                // iyr (Issue Year) - four digits; at least 2010 and at most 2020.
                if (IsYearValid(iyr, 2010, 2020)) bResult = true;
                else return false;

                // eyr (Expiration Year) - four digits; at least 2020 and at most 2030.
                if (IsYearValid(eyr, 2020, 2030)) bResult = true;
                else return false;

                // hgt (Height) - a number followed by either cm or in:
                // If cm, the number must be at least 150 and at most 193.
                // If in, the number must be at least 59 and at most 76.
                if (IsHeightValid(hgt)) bResult = true;
                else return false;

                // hcl (Hair Color) - a # followed by exactly six characters 0-9 or a-f.
                if (IsColorValid(hcl)) bResult = true;
                else return false;

                // ecl (Eye Color) - exactly one of: amb blu brn gry grn hzl oth.
                if (IsEyeColorValid(ecl)) bResult = true;
                else return false;

                // pid (Passport ID) - a nine-digit number, including leading zeroes.
                if (IsPIDvalid(pid)) bResult = true;
                else return false;

                return bResult;

            }

            private bool IsPIDvalid(string sPID)
            {
                if (sPID.Length != 9)
                    return false;

                try
                {
                    int intValue = Convert.ToInt32(sPID);
                    return true;
                }
                catch
                {
                    return false;
                }

            }

            private bool IsEyeColorValid(string ecl)
            {
                List<string> ValidColors = new List<string>();
                ValidColors.Add("amb");
                ValidColors.Add("blu");
                ValidColors.Add("brn");
                ValidColors.Add("gry");
                ValidColors.Add("grn");
                ValidColors.Add("hzl");
                ValidColors.Add("oth");


                if (ValidColors.Find(n => n == ecl) != null)
                    return true;
                else
                    return false;
            }

            private bool IsColorValid(string sColor)
            {
                if (sColor.Length != 7)
                    return false;

                try
                {
                    sColor = sColor.Replace("#", "0x");
                    int intValue = Convert.ToInt32(sColor, 16);

                    return true;
                }
                catch
                {
                    return false;
                }



            }

            private bool IsHeightValid(string sHgt)
            {
                int nHgt;
                int nPosition;
                int nHgtMin;
                int nHgtMax;

                if (sHgt.ToLower().IndexOf("in") >= 0)
                {
                    nPosition = sHgt.ToLower().IndexOf("in");
                    nHgtMin = 59;
                    nHgtMax = 76;                }
                else
                {
                    nPosition = sHgt.ToLower().IndexOf("cm");
                    nHgtMin = 150;
                    nHgtMax = 193;
                }

                try
                {
                    nHgt = int.Parse(sHgt.Substring(0,nPosition));

                    if (nHgt >= nHgtMin && nHgt <= nHgtMax)
                            return true;
                        else
                            return false;
                }
                catch
                {
                    return false;
                }
            }

            private bool IsYearValid(string sYear, int nYearMin, int nYearMax)
            {
                int nByr;
                try
                {
                    nByr = int.Parse(sYear);
                    if (nByr >= nYearMin && nByr <= nYearMax)
                        return  true;
                    else
                        return false;
                }
                catch
                {
                    return false;
                }
            }


            public bool IsPassportValid()
            {
                return bValid;
            }


    }

        private static void Main(string[] args)
    {
        Console.Clear();
        Console.WriteLine(DateTime.Now);
        StreamReader file = new StreamReader(@".\data.txt");

        var nValidPassportsBasic = 0;
        var nValidPassportsStrong = 0;

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
            {
                // Part ONE BasicValidation
                if (P.BasicValidation()) nValidPassportsBasic++;

                // Part TWO
                if (P.IsPassportValid()) nValidPassportsStrong++;
            }



        Console.WriteLine("--------------------------");
        Console.WriteLine("PartOne: {0}", nValidPassportsBasic);
        Console.WriteLine("PartTwo: {0}", nValidPassportsStrong);

    }
}
}
