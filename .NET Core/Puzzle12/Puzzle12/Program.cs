using System;
using System.Collections.Generic;

namespace Puzzle12
{
    class Program
    {
        public struct Moon
        {
            public string name;
            public int x;
            public int y;
            public int z;
            public int velocity_x;
            public int velocity_y;
            public int velocity_z;

            public Moon(string name,int x,int y,int z)
            {
                this.name = name;
                this.x = x;
                this.y = y;
                this.z = z;
                this.velocity_x = 0;
                this.velocity_y = 0;
                this.velocity_z = 0;
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

        }

        //Io          <x= -3,   y= 10,  z=-1>
        //Europa      <x=-12,   y=-10,  z=-5>
        //Ganymede    <x= -9,   y=  0,  z=10>
        //Callisto    <x=  7,   y= -5,  z=-3>

        static void Main(string[] args)
        {
            Moon Io = new Moon("Io", -3, 10, -1);
            Moon Europa = new Moon("Europa", -12, -10, -5);
            Moon Ganymede = new Moon("Ganymede", -9, 0, 10);
            Moon Callisto = new Moon("Callisto", -7, -5, -3);

            // Test Sample
            //Moon Io         = new Moon("Io",         -1,   0,  2);
            //Moon Europa     = new Moon("Europa",      2, -10, -7);
            //Moon Ganymede   = new Moon("Ganymede",    4,  -8,  8);
            //Moon Callisto   = new Moon("Callisto",    3,   5, -1);

            List <Moon> Planets = new List<Moon>();

            Planets.Add(Io);
            Planets.Add(Europa);
            Planets.Add(Ganymede);
            Planets.Add(Callisto);


            for (int i = 0; i < 1000; i++)
            {
                for(int x=0; x<4; x++)
                    for (int y = 0; y < 4; y++)
                    {
                        Planets[x] = Planets[x].UpdateVelocity(Planets[y]);
                    }

                for (int x = 0; x < 4; x++)
                {
                    Planets[x] = Planets[x].UpdatePosition();
                    //Console.WriteLine("{0} : Potential E = {1}  Kinetic E = {2}  Total energy = {3}", Planets[x].name, Planets[x].GetPotentialEnergy(), Planets[x].GetKineticakEnergy(), Planets[x].GetTotalEnergy());
                }

               // foreach (Moon moon in Planets)
               //     Console.WriteLine("<{0}> <{1}> <{2}> = P<{3}> - <{4}> <{5}> <{6}> = K<{7}>", moon.x, moon.y, moon.z, moon.GetPotentialEnergy(), moon.velocity_x, moon.velocity_y, moon.velocity_z, moon.GetKineticakEnergy());            
            }

            int nTotalEnergy=0;

            foreach (Moon moon in Planets)
                nTotalEnergy += moon.GetTotalEnergy();

            Console.WriteLine("Total energy = {0}", nTotalEnergy);

        }
    }
}
