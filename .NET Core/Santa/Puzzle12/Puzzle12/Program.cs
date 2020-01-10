using System;
using System.Collections.Generic;

namespace Puzzle12
{
    public class Moon
    {
         string name;
         int x;
         int y;
         int z;
         int velocity_x;
         int velocity_y;
         int velocity_z;

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

        public static Int64 GetFullCycleForAxis (char cAxis, List<Moon> PlanetsVanile) // 'X' 'Y' 'Z'
        {
            int nTheVelocity    = 0;
            bool bCondition = false;

            List<Moon> Planets2;
            Planets2 = new List<Moon>();
            foreach (Moon planet in PlanetsVanile)
                Planets2.Add(new Moon(planet));

            Int64 nMovementCount = 0;
            int nCount = 0;
            while (nCount != 4)
            {
                nMovementCount++;
                nCount = 0;
                Moon.MoveThePlanets(Planets2);
                foreach (Moon planet in Planets2)
                {
                    switch (cAxis) // this looks ugly, ned to think about
                    {
                        case 'X':
                            //nTheVelocity = planet.velocity_x;
                            bCondition = (planet.velocity_x == 0 && planet.x == PlanetsVanile[nCount].x);
                            break;
                        case 'Y':
                            //nTheVelocity = planet.velocity_y;
                            bCondition = (planet.velocity_y == 0 && planet.y == PlanetsVanile[nCount].y);
                            break;
                        case 'Z':
                            //nTheVelocity = planet.velocity_z;
                            bCondition = (planet.velocity_z == 0 && planet.z == PlanetsVanile[nCount].z);
                            break;
                        default:
                            Console.WriteLine("Argument Axis should be X|Y|Z");
                            break;
                    }

                    //if (nTheVelocity == 0)
                    if (bCondition)
                        nCount++;
                    else
                        break;
                }
            }

            return nMovementCount;
        }

    }
    class Program
    {
        static void Main(string[] args)
        {
            DateTime dateTime = DateTime.Now;

            int steps;
            int nTotalEnergy = 0;

            // My personal input:
            // Answers: Total energy =  10944  / Total movement = 484244804958744
            steps = 1000;
            Moon Io = new Moon("Io", -3, 10, -1);
            Moon Europa = new Moon("Europa", -12, -10, -5);
            Moon Ganymede = new Moon("Ganymede", -9, 0, 10);
            Moon Callisto = new Moon("Callisto", 7, -5, -3);

            //<x=  -3, y=  10, z= -1>
            //<x= -12, y= -10, z= -5>
            //<x=  -9, y=   0, z= 10>
            //<x=   7, y=  -5, z= -3>

            //Test Sample -10 runs. Answers: Total energy =  179  / Total movement = 2772
            //steps = 10;
            //Moon Io = new Moon("Io", -1, 0, 2);
            //Moon Europa = new Moon("Europa", 2, -10, -7);
            //Moon Ganymede = new Moon("Ganymede", 4, -8, 8);
            //Moon Callisto = new Moon("Callisto", 3, 5, -1);


            //test sample - 100 runs. Answers: Total energy =  1940  / Total movement = 4686774924
            //steps = 100;
            //Moon Io = new Moon("Io", -8, -10, 0);
            //Moon Europa = new Moon("Europa", 5, 5, 10);
            //Moon Ganymede = new Moon("Ganymede", 2, -7, 3);
            //Moon Callisto = new Moon("Callisto", 9, -8, -3);

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

            while (steps-- > 0)
                Moon.MoveThePlanets(Planets);

            foreach (Moon planet in Planets)
                nTotalEnergy += planet.GetTotalEnergy();

            Console.WriteLine("Total energy = {0}", nTotalEnergy);

            // Second part of the puzzle;

            Int64 nX = Moon.GetFullCycleForAxis('X', PlanetsVanile);
            Console.WriteLine("Number of cycles: {0}", nX);

            Int64 nY = Moon.GetFullCycleForAxis('Y', PlanetsVanile);
            Console.WriteLine("Number of cycles: {0}", nY);

            Int64 nZ = Moon.GetFullCycleForAxis('Z', PlanetsVanile);
            Console.WriteLine("Number of cycles: {0}", nZ);

            Console.WriteLine("Final Answer: {0}", GetLCM(nX, nY, nZ)); 
            Console.WriteLine("----------------------------------------------");
            Console.WriteLine("Execution time: {0}", DateTime.Now - dateTime);
        }

        static Int64 GetLCM(Int64 nA, Int64 nB, Int64 nC) //Least common multiple
        {
            return GetLCM(GetLCM(nA, nB), nC);
        }

        static Int64 GetLCM(Int64 nA, Int64 nB) //Least common multiple
        {
            if (nA == nB) return nA;

            Int64 nMax = Math.Max(nA, nB);
            Int64 nMin = Math.Min(nA, nB);
            Int64 nGCD = 1; //Greatest common divisor

            for (Int64 i = 2; i <= Math.Sqrt(nMax); i++)
            {
                while (nMax % i == 0 && nMin % i == 0)
                {
                    nGCD *= i;
                    nMax /= i;
                    nMin /= i;
                }
            }
            return nA*nB/nGCD;
        }


    }
}
