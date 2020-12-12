using System;
using System.IO;
using System.Collections.Generic;


namespace Puzzle9
{
    class Program
{

    const int PREAMBULA = 25;
    static readonly List<Int64> EncryptedQueue = new List<Int64>();
    static void Main()
    {
        Console.Clear();
        Console.WriteLine(DateTime.Now);

            using (StreamReader file = new StreamReader(@".\data.txt"))
            while (!file.EndOfStream)
                {
                    string S = file.ReadLine();
                    EncryptedQueue.Add(Int64.Parse(S));
                }


            for (int i = PREAMBULA; i < EncryptedQueue.Count; i++)
            {
                if (!IsValidNumber(i))
                {
                    Console.WriteLine("PartOne: {0}", EncryptedQueue[i]);
                    Console.WriteLine("--------------------------");
                    Console.WriteLine("PartTwo: {0}", GetWeakSet(EncryptedQueue[i]));

                }
            }

    }

    static bool IsValidNumber(int nPosition)
        {
            for (int x = nPosition - PREAMBULA; x < nPosition; x++)
                for (int y = x + 1; y < nPosition; y++)
                    if (EncryptedQueue[nPosition] == EncryptedQueue[x] + EncryptedQueue[y]) return true;
            
            return false;
        }


        static Int64 GetWeakSet(Int64 NotValidNumber)
        {
            for (int x = 0; x < EncryptedQueue.Count; x++)
            {
                Int64 nMin = Int64.MaxValue; ;
                Int64 nMax = 0;

                Int64 nSum = 0;
                for(int y = x; y < EncryptedQueue.Count;y++)
                {
                    if (EncryptedQueue[y] > nMax) nMax = EncryptedQueue[y];
                    if (EncryptedQueue[y] < nMin) nMin = EncryptedQueue[y];

                    nSum += EncryptedQueue[y];

                    if (nSum == NotValidNumber)  return nMin + nMax;
                }
            }
            return -1;
        }
    }
}
