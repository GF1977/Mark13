using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Puzzle6
{
    class Program
    {
        static void Main(string[] args)
        {
            StreamReader file = new StreamReader(@".\data.txt");
            string line = file.ReadToEnd();
            string[] words = line.Split("\r\n");

            List<StarMap> SpaceObjects = new List<StarMap>();
            foreach (string word in words)
            {
                string[] ParentAndChild = word.Split(')');
                SpaceObjects.Add(new StarMap(ParentAndChild[0], ParentAndChild[1]));
            }

            int totalOrbits = 0;
            foreach (StarMap planet in SpaceObjects)
            {
                string firstPlanet = planet.GetParent();
                int pos = 1;
                StarMap tempP = planet;
                while (true)//&& tempP.GetParent()!="COM")
                {
                    pos = tempP.FindParent(ref SpaceObjects);
                    planet.SetOrbits(planet.GetOrbits() + 1);
                    if (pos == -1) break;
                    tempP = SpaceObjects[pos];
                }
                totalOrbits += planet.GetOrbits();
            }

            StarMap You = new StarMap("", "YOU");
            StarMap San = new StarMap("", "SAN");
            foreach (StarMap planet in SpaceObjects)
            {
                if (planet.GetChild() == "YOU")
                    You = planet;
                if (planet.GetChild() == "SAN")
                    San = planet;
            }

            List<StarMap> myWayToCOM = new List<StarMap>();
            List<StarMap> SanWayToCOM = new List<StarMap>();
            myWayToCOM = You.GetTheWay(ref SpaceObjects);
            SanWayToCOM = San.GetTheWay(ref SpaceObjects);
            var intersection = myWayToCOM.Intersect(SanWayToCOM);

            Console.WriteLine("Total number of Orbits: {0}", totalOrbits);
            int jumps = SanWayToCOM.Count() + myWayToCOM.Count - 2 * intersection.Count()-2;
            Console.WriteLine("Intersection: {0}, number of orbits: {1}", intersection.First().GetParent(),jumps);

        }
    }
     class StarMap
        {
            private string Parent;
            private string Name;
            private int nOrbits;

            public StarMap(string argParent, string argName)
            {
                Parent = argParent;
                Name = argName;
                SetOrbits(0);
            }

            public int GetOrbits()
            {
                return nOrbits;
            }

            public void SetOrbits(int argOrbits)
            {
                nOrbits = argOrbits;
            }

            public string GetParent()
            {
                return Parent;
            }

            public string GetChild()
            {
                return Name;
            }


        public int FindParent(ref List<StarMap> SO)
            {
            if (Parent == "COM") return -1;
        
                int res = 0;
                foreach (StarMap planet in SO)
                {
                    if (planet.GetChild() == Parent)
                        break;
                    res++;
                }
            return res;
            }

        public List<StarMap> GetTheWay(ref List<StarMap> SO)
        {
            List<StarMap> myWay = new List<StarMap>();
            int pos;
            StarMap tempP = this;
                while (true)
                {
                    pos = tempP.FindParent(ref SO);
                    myWay.Add(tempP);
                    if (pos == -1) break;
                    tempP = SO[pos];
                }
            return myWay;
        }

    }

}
