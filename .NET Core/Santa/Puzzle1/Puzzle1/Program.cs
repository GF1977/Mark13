using System;
// For a mass of 12, divide by 3 and round down to get 4, then subtract 2 to get 2.
// 3432671
// 5146132

namespace Puzzle1
{
    class Program
    {
        static void Main(string[] args)
        {
            Int64 fuel_mass = 0;
            Int64 fuel_for_fuel_total = 0;
            Int64 fuel_for_fuel_per_module;
            string line;

            System.IO.StreamReader file =  new System.IO.StreamReader(@".\data.txt");
            while ((line = file.ReadLine()) != null)
            {
                fuel_for_fuel_per_module = 0;
                Int64 fuel_for_module = GetFuelForMass(Int64.Parse(line));
                Int64 fuel_delta = fuel_for_module;
                while (fuel_delta > 0)
                {
                    fuel_delta = GetFuelForMass(fuel_delta);
                    if (fuel_delta > 0)
                        fuel_for_fuel_per_module += fuel_delta;
                }

                fuel_mass += fuel_for_module;
                fuel_for_fuel_total += fuel_for_fuel_per_module;

            }

            file.Close();
            System.Console.WriteLine("Fuel mass = {0}", fuel_mass);
            System.Console.WriteLine("Fuel for fuel  = {0}", fuel_for_fuel_total);
            System.Console.WriteLine("Fuel total  = {0}", fuel_mass+fuel_for_fuel_total);
        }

        static Int64 GetFuelForMass(Int64 mass)
        {
              return (Int64)(Math.Floor(mass / 3.0) - 2);
        }
    }
}


