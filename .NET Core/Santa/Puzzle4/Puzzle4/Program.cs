//You arrive at the Venus fuel depot only to discover it's protected by a password. The Elves had written the password on a sticky note, but someone threw it out.
//However, they do remember a few key facts about the password:
//    It is a six-digit number.
//    The value is within the range given in your puzzle input.
//    Two adjacent digits are the same (like 22 in 122345).
//    Going from left to right, the digits never decrease; they only ever increase or stay the same(like 111123 or 135679).
//Other than the range rule, the following are true:
//    111111 meets these criteria(double 11, never decreases).
//    223450 does not meet these criteria(decreasing pair of digits 50).
//    123789 does not meet these criteria(no double).
//How many different passwords within the range given in your puzzle input meet these criteria?
//Your puzzle input is 387638-919123.


using System;

namespace Puzzle4
{
    class Program
    {
        static void Main(string[] args)
        {
            int passCount = 0;
            for (int i = 387638; i <= 919123; i++) //919123
            {
                if (CheckPassword(i))
                {
                    passCount++;
                    Console.WriteLine("Password is: {0}", i);
                }
            }
            Console.WriteLine("Password's count is: {0}", passCount);

        }

        static bool CheckPassword(int password)
        {
            bool res;
            bool condition_1 = false;
            bool condition_2 = true;

            //condition #1 two same digits but not in the large group (3 and more)
            int i = 0;
            string sPass = password.ToString();
            while (i < sPass.Length - 1 && !condition_1)
            {
                if (sPass[i] == sPass[i + 1])
                {
                    string a = sPass[i].ToString() + sPass[i].ToString() + sPass[i].ToString();
                    if (!sPass.Contains(a))
                        condition_1 = true;
                }
                i++;
            }

            //condition #2 Going from left to right, the digits never decrease; they only ever increase or stay the same(like 111123 or 135679).
            i = 0;

            while (i < sPass.Length - 1 && condition_2)
            {
                if (sPass[i] > sPass[i + 1])
                    condition_2 = false;
                i++;
            }




            res = condition_1 && condition_2;
            return res;        
        }
    }
         
}
