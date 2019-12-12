using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Puzzle3
{
    class Program
    {
        static void Main(string[] args)
        {
            StreamReader file = new StreamReader(@".\data_1.txt");
            string[] wire_1 = file.ReadLine().Split(',');

                         file = new StreamReader(@".\data_2.txt");
            string[] wire_2 = file.ReadLine().Split(',');
            file.Close();


            List<int> W1_path_X = new List<int>();
            List<int> W1_path_Y = new List<int>();
            List<string> W1_full_path = new List<string>();

            W1_path_X.Add(0);
            W1_path_Y.Add(0);

            W1_full_path = PullWire(ref W1_path_X, ref W1_path_Y, wire_1);


            List<int> W2_path_X = new List<int>();
            List<int> W2_path_Y = new List<int>();
            List<string> W2_full_path = new List<string>();

            W2_path_X.Add(0);
            W2_path_Y.Add(0);

            W2_full_path = PullWire(ref W2_path_X, ref W2_path_Y, wire_2);



            List<string> Intersections = W1_full_path.Intersect(W2_full_path).ToList();

            int i = 0;
            int min_steps = W1_full_path.IndexOf(Intersections[i]) + W2_full_path.IndexOf(Intersections[i])+2;
            while (i < Intersections.Count)
            {
                int steps = W1_full_path.IndexOf(Intersections[i]) + W2_full_path.IndexOf(Intersections[i])+2;
                if (steps < min_steps)
                    min_steps = steps;
                Console.WriteLine("intersections = {0}; Distance (Manhattan) = {1};  Steps = {2}", Intersections[i], ManhattanDistance(Intersections[i]),steps);
                i++;
            }

            



            i  = 1;
            int minDist = ManhattanDistance(Intersections[0]);
            while (i<Intersections.Count)
            {
                if (minDist > ManhattanDistance(Intersections[i]))
                    minDist = ManhattanDistance(Intersections[i]);
                i++;
            }

            Console.WriteLine("intersections = {0}; Min distance (Manhattan) = {1}; Min steps = {2}", Intersections.Count, minDist, min_steps);



        }

        static int ManhattanDistance (string coordinates)
        {
            int distance = 0;
            string[] XY = coordinates.Split(';');

            distance = Math.Abs(int.Parse(XY[0])) + Math.Abs(int.Parse(XY[1]));
            return distance;
        }

        static List<string> PullWire (ref List<int> refW1_path_X , ref List<int> refW1_path_Y, string[] wire_1)
        {
            List<string> sFullPath = new List<string>();
            int last_index = 1;
            int i = 0;
        while (i < wire_1.Length)
        {
            string word = wire_1[i];
            string command = word.Substring(0, 1);
            int steps = int.Parse(word.Substring(1, word.Length - 1));
            switch (command)
            {
                case "L":
                   // Console.WriteLine("Left, {0} steps; ", steps);
                    while (steps > 0)
                    {

                        refW1_path_X.Add(refW1_path_X[last_index - 1] - 1);
                        refW1_path_Y.Add(refW1_path_Y[last_index - 1]);
                            sFullPath.Add(refW1_path_X[last_index].ToString() + ";" + refW1_path_Y[last_index].ToString());
                            last_index++;
                        steps--;
                    }
                    break;

                case "R":
                   // Console.WriteLine("Right, {0} steps; ", steps);
                    while (steps > 0)
                    {
                        refW1_path_X.Add(refW1_path_X[last_index - 1] + 1);
                        refW1_path_Y.Add(refW1_path_Y[last_index - 1]);
                            sFullPath.Add(refW1_path_X[last_index].ToString() + ";" + refW1_path_Y[last_index].ToString());
                            last_index++;
                        steps--;
                    }
                    break;

                case "U":
                   // Console.WriteLine("Up, {0} steps; ", steps);
                    while (steps > 0)
                    {
                        refW1_path_X.Add(refW1_path_X[last_index - 1]);
                        refW1_path_Y.Add(refW1_path_Y[last_index - 1] + 1);
                            sFullPath.Add(refW1_path_X[last_index].ToString() + ";" + refW1_path_Y[last_index].ToString());
                            last_index++;
                        steps--;
                    }
                    break;

                case "D":
                   // Console.WriteLine("Down, {0} steps; ", steps);
                    while (steps > 0)
                    {
                        refW1_path_X.Add(refW1_path_X[last_index - 1]);
                        refW1_path_Y.Add(refW1_path_Y[last_index - 1] - 1);
                            sFullPath.Add(refW1_path_X[last_index].ToString() + ";" + refW1_path_Y[last_index].ToString());
                            last_index++;
                        steps--;
                    }
                    break;

                default:
                   // Console.WriteLine("Something wrong");
                    break;
            }
            i++;
            }
            
            return sFullPath;
        }
    }
}
