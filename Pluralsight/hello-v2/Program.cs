using System;

namespace hello
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
			if (args.Length > 0)
			{
				int a = int.Parse(args[0]);
				int b = int.Parse(args[1]);
				try
				{
					int c = a*b;

					Console.WriteLine(c);
				}
				catch (Exception e) 
				{
					Console.WriteLine("{0} Exception caught.", e);
				}
				
			}
        }
    }
}
