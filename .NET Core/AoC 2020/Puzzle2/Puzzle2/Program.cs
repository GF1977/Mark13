using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Puzzle1
{
    class Program
    {

        public class PasswordPolicy
        {
            int nMinCount;
            int nMaxCount;

            char sABC;
            string sPassword;


            public PasswordPolicy(string sRawData)
            {
                string[] sArray = sRawData.Split(' ','-' ,':');
                this.nMinCount = int.Parse(sArray[0]);
                this.nMaxCount = int.Parse(sArray[1]);
                this.sABC = sArray[2][0]; // [0] means first char of the string
                this.sPassword = sArray[4]; // [3] is an empty cell

            }

            public bool CheckPolicy()
            {
                bool bRes = false;
                // check the min
                int nABCCount = this.sPassword.Count(n => n == sABC);

                if (nABCCount >= nMinCount && nABCCount <= nMaxCount)
                    bRes = true;

                return bRes;

            }

            public string GetPass()
            {
                return this.sPassword;
            }

        }





        static void Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine(DateTime.Now);
            StreamReader file = new StreamReader(@".\data.txt");

            List<PasswordPolicy> PassList = new List<PasswordPolicy>();
            int A = 0;

            while (!file.EndOfStream)
            {
                string S = file.ReadLine();
                PasswordPolicy P = new PasswordPolicy(S);
                PassList.Add(P);
            }

            int nGoodPasswords = 0;
            foreach(PasswordPolicy P in PassList)
            {
                if (P.CheckPolicy())
                {
                    Console.WriteLine("Password: {0} - PASSED", P.GetPass());
                    nGoodPasswords++;
                }
                else
                    Console.WriteLine("Password: {0} - FAILED", P.GetPass());

            }

            Console.WriteLine("--------------------------");
            Console.WriteLine("Valid passwords: {0}", nGoodPasswords);

        }
    }
}
