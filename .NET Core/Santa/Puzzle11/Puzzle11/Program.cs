using System;

namespace Puzzle11
{
    class Program
    {
        static void Main(string[] args)
        {
            var x = Math.PI;
            Console.WriteLine($"Pi {x:N10}");

            string s = String.Format("It is now {0:D} at {0:t}", DateTime.Now);
            Console.WriteLine(s);
        }
    }
}
