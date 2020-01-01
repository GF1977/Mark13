using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Puzzle3
{
    class Program
    {
        struct Coordinates
        {
            public int X;
            public int Y;

            public Coordinates(int X, int Y)
            {
                this.X = X;
                this.Y = Y;
            }
        }

        static void Main(string[] args)
        {
            StreamReader    file = new StreamReader(@".\data_1.txt");
            string[] wire_1 = file.ReadLine().Split(',');

                            file = new StreamReader(@".\data_2.txt");
            string[] wire_2 = file.ReadLine().Split(',');
            file.Close();

            List<Coordinates> W1_path = new List<Coordinates>();
            List<string> W1_full_path = new List<string>();

            Coordinates tempCoordinates = new Coordinates(0, 0);
            W1_path.Add(tempCoordinates);
            W1_full_path = PullWire(W1_path, wire_1);


            List<Coordinates> W2_path = new List<Coordinates>();
            List<string> W2_full_path = new List<string>();

            W2_path.Add(tempCoordinates);
            W2_full_path = PullWire(W2_path, wire_2);


            List<string> Intersections = W1_full_path.Intersect(W2_full_path).ToList();
            
            // Part #1 - What is the Manhattan distance from the central port to the closest intersection?
            int minDist = ManhattanDistance(Intersections[0]);
            for  (int i=0; i < Intersections.Count;i++)
            {
                if (minDist > ManhattanDistance(Intersections[i]))
                    minDist = ManhattanDistance(Intersections[i]);
                i++;
            }

            //part #2 - What is the fewest combined steps the wires must take to reach an intersection?

            int min_steps = W1_full_path.IndexOf(Intersections[0]) + W2_full_path.IndexOf(Intersections[0]) + 2;
            for (int i = 1; i < Intersections.Count; i++)
            {
                min_steps = Math.Min(min_steps, (W1_full_path.IndexOf(Intersections[i]) + W2_full_path.IndexOf(Intersections[i]) + 2));
            }

            Console.WriteLine("intersections = {0}; Min distance (Manhattan) = {1}; Min steps = {2}", Intersections.Count, minDist, min_steps);
        }

        static int ManhattanDistance(string coordinates)
        {
            string[] XY = coordinates.Split(';');
            return Math.Abs(int.Parse(XY[0])) + Math.Abs(int.Parse(XY[1]));
        }

        static List<string> PullWire(List<Coordinates> Path, string[] wire_1)
        {
            List<string> sFullPath = new List<string>();
            int last_index = 1;
            for (int i=0; i < wire_1.Length;i++)
            {
                string word = wire_1[i];
                string command = word.Substring(0, 1);
                int steps = int.Parse(word.Substring(1, word.Length - 1));
                Coordinates nDelta = GetDirection(command);
                while (steps > 0)
                {
                    Coordinates tempCoordinates = new Coordinates(Path[last_index - 1].X + nDelta.X, Path[last_index - 1].Y + nDelta.Y);
                    Path.Add(tempCoordinates);
                    sFullPath.Add(Path[last_index].X.ToString() + ";" + Path[last_index].Y.ToString());
                    last_index++;
                    steps--;
                }
            }
            return sFullPath;
        }

        private static Coordinates GetDirection(string command)
        {
            Coordinates nDelta = new Coordinates(0, 0);
            switch (command)
            {
                case "L": nDelta.X = -1; nDelta.Y =  0; break;
                case "R": nDelta.X =  1; nDelta.Y =  0; break;
                case "U": nDelta.X =  0; nDelta.Y =  1; break;
                case "D": nDelta.X =  0; nDelta.Y = -1; break;
                default:
                    Console.WriteLine("Something wrong");
                    break;
            }
            return nDelta;
        }
    }
}
