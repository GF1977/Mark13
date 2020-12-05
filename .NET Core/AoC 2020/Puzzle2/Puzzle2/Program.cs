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

            char cABC;
            string sPassword;


            public PasswordPolicy(string sRawData)
            {
                string[] sArray = sRawData.Split(' ','-' ,':');
                nMinCount = int.Parse(sArray[0]);
                nMaxCount = int.Parse(sArray[1]);
                cABC = sArray[2][0]; // [0] means first char of the string
                sPassword = sArray[4]; // [3] is an empty cell

            }

            public bool CheckPolicy()
            {
                bool bRes = false;
                int nABCCount = sPassword.Count(n => n == cABC);

                if (nABCCount >= nMinCount && nABCCount <= nMaxCount)
                    bRes = true;

                return bRes;
            }


            public bool CheckPolicyTCAS() //Toboggan Corporate Authentication System
            {
                bool bRes = false;

                if ((cABC == sPassword[nMinCount-1] && cABC != sPassword[nMaxCount - 1]) || (cABC != sPassword[nMinCount-1] && cABC == sPassword[nMaxCount - 1]))
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
            int nGoodPasswordsTCAS = 0;

            foreach (PasswordPolicy P in PassList)
            {
                if (P.CheckPolicy())
                    nGoodPasswords++;

                if (P.CheckPolicyTCAS())
                    nGoodPasswordsTCAS++;


            }

            Console.WriteLine("--------------------------");
            Console.WriteLine("Valid passwords       : {0}", nGoodPasswords);
            Console.WriteLine("Valid passwords (TCAS): {0}", nGoodPasswordsTCAS);

        }
    }
}
