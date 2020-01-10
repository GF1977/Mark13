using System;
using System.Collections.Generic;

namespace Puzzle12
{
    public class Moon
    {
        public string name;
        public int x;
        public int y;
        public int z;
        public int velocity_x;
        public int velocity_y;
        public int velocity_z;

        public Moon(string name, int x, int y, int z)
        {
            this.name = name;
            this.x = x;
            this.y = y;
            this.z = z;
            this.velocity_x = 0;
            this.velocity_y = 0;
            this.velocity_z = 0;
        }

        public Moon(Moon m)
        {
            this.name = m.name;
            this.x = m.x;
            this.y = m.y;
            this.z = m.z;
            this.velocity_x = m.velocity_x;
            this.velocity_y = m.velocity_y;
            this.velocity_z = m.velocity_z;
        }

        public Moon UpdateVelocity(Moon targetMoon)
        {
            if (this.x > targetMoon.x)
                this.velocity_x--;
            if (this.x < targetMoon.x)
                this.velocity_x++;

            if (this.y > targetMoon.y)
                this.velocity_y--;
            if (this.y < targetMoon.y)
                this.velocity_y++;

            if (this.z > targetMoon.z)
                this.velocity_z--;
            if (this.z < targetMoon.z)
                this.velocity_z++;

            return this;
        }

        public Moon UpdatePosition()
        {
            this.x += this.velocity_x;
            this.y += this.velocity_y;
            this.z += this.velocity_z;

            return this;
        }


        //A moon's potential energy is the sum of the absolute values of its x, y, and z position coordinates
        public int GetPotentialEnergy()
        {
            return Math.Abs(this.x) + Math.Abs(this.y) + Math.Abs(this.z);
        }

        //A moon's kinetic energy is the sum of the absolute values of its velocity coordinates
        public int GetKineticakEnergy()
        {
            return Math.Abs(this.velocity_x) + Math.Abs(this.velocity_y) + Math.Abs(this.velocity_z);
        }

        //The total energy for a single moon is its potential energy multiplied by its kinetic energy.
        public int GetTotalEnergy()
        {
            return this.GetKineticakEnergy() * this.GetPotentialEnergy();
        }

        public bool Compare(Moon planet)
        {
            // We don't compare names because they are not the same.
            if (this.x == planet.x &&
                    this.y == planet.y &&
                    this.z == planet.z &&
                    this.velocity_x == planet.velocity_x &&
                    this.velocity_y == planet.velocity_y &&
                    this.velocity_z == planet.velocity_z)

                return true;
            else
                return false;
        }

        public static void MoveThePlanets(List<Moon> Planets)
        {
            foreach (Moon planetA in Planets)
                foreach (Moon planetB in Planets)
                    planetA.UpdateVelocity(planetB);

            foreach (Moon planet in Planets)
            {
                planet.UpdatePosition();
            }
        }

    }
    class Program
    {
        static void Main(string[] args)
        {
            DateTime dateTime = DateTime.Now;

            //int  steps = 1000;
            //Moon Io = new Moon("Io", -3, 10, -1);
            //Moon Europa = new Moon("Europa", -12, -10, -5);
            //Moon Ganymede = new Moon("Ganymede", -9, 0, 10);
            //Moon Callisto = new Moon("Callisto", 7, -5, -3);

            //<x=  -3, y=  10, z= -1>
            //<x= -12, y= -10, z= -5>
            //<x=  -9, y=   0, z= 10>
            //<x=   7, y=  -5, z= -3>

            //Test Sample -10 runs. Answers: Total energy =  179  / Total movement = 2772
            //int steps = 10;
            //Moon Io = new Moon("Io", -1, 0, 2);
            //Moon Europa = new Moon("Europa", 2, -10, -7);
            //Moon Ganymede = new Moon("Ganymede", 4, -8, 8);
            //Moon Callisto = new Moon("Callisto", 3, 5, -1);


            //test sample - 100 runs. Answers: Total energy =  1940  / Total movement = 4686774924
            int steps = 100;
            Moon Io = new Moon("Io", -8, -10, 0);
            Moon Europa = new Moon("Europa", 5, 5, 10);
            Moon Ganymede = new Moon("Ganymede", 2, -7, 3);
            Moon Callisto = new Moon("Callisto", 9, -8, -3);

            //< x = -8, y = -10, z =  0 >
            //< x =  5, y =   5, z = 10 >
            //< x =  2, y =  -7, z =  3 >
            //< x =  9, y =  -8, z = -3 >

            List<Moon> PlanetsVanile = new List<Moon>();

            PlanetsVanile.Add(Io);
            PlanetsVanile.Add(Europa);
            PlanetsVanile.Add(Ganymede);
            PlanetsVanile.Add(Callisto);


            // First part of the puzzle;
            List<Moon> Planets = new List<Moon>();
            foreach (Moon planet in PlanetsVanile)
                Planets.Add(new Moon(planet));


            while (steps-->0)
            {
                Moon.MoveThePlanets(Planets);
            }

            int nTotalEnergy=0;
            foreach (Moon planet in Planets)
                nTotalEnergy += planet.GetTotalEnergy();

            Console.WriteLine("Total energy = {0}", nTotalEnergy);

            // Second part of the puzzle;
            //List<Moon> Planets2 = new List<Moon>();
            //foreach (Moon planet in PlanetsVanile)
            //    Planets2.Add(new Moon(planet));

            Int64 nMovementCount;
            //while(i != Planets2.Count) // i represents the number of moons with the exactly the same parameters
            //{
            //    i = 0;
            //    Moon.MoveThePlanets(Planets2);
            //    foreach (Moon planet in Planets2)
            //        if(planet.Compare(PlanetsVanile[i]))
            //            i++;
            //        else
            //            break;
            //    nMovementCount++;
            //}

            Console.WriteLine("Execution time: {0}", DateTime.Now - dateTime);
        }

        static Int64 IsGood(Int64 a, Int64 b, Int64 c, Int64 d)
        {
            Int64 res = 0;
            if (a == 0 || b == 0 || c == 0 || d == 0)
                return 0;

            Int64 nMin = Math.Min(Math.Min(a, b), Math.Min(c, d));
            if (a % nMin == 0 && b % nMin == 0 && c % nMin == 0 && d % nMin == 0)
                res = nMin;

            return res;
        }

        static Int64 GetNOK(Int64 a, Int64 b, Int64 c)
        {
            Int64 nMax = Math.Max(Math.Max(a, b), c);

            for (Int64 i = 2;i < nMax; i++)
            {
                while (a % i == 0 && b % i ==0 && c % i ==0)
                {
                    a /= i;
                    b /= i;
                    c /= i;
                }
            }

            return a*b*c;
        }

    }
}
