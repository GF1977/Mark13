using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using MyClasses;

    
namespace Puzzle11
{
    class Program
    {
        static void Main(string[] args)
        {
            // Int64 StartValue = 0; // Part #1 
            Int64 StartValue = 1;    // Part #2 
 
            StreamReader file = new StreamReader(@".\data.txt");
            string line = file.ReadLine();
            string[] words = line.Split(',');

            List<Int64> commands_vanile = new List<Int64>();
            foreach (string word in words)
                commands_vanile.Add(Int64.Parse(word));

            for (int ii = 0; ii < 1000; ii++)
                commands_vanile.Add(0);

            List<Int64> commands = new List<Int64>(commands_vanile);

            Int64[,,] theHull = new Int64[120,120,2];
            int posX = 60;
            int posY = 60;

            theHull[posX, posY, 0] = StartValue;
            char Direction = 'N';

            
            Int64 nStep = 0;
            Int64[] res;
            Int64[] Arguments = { 0, 0 }; // color , direction
            int i = 0;
            int nPaintedPanel = 0;

            Image image = new Bitmap(1400, 1400);
            Graphics graph = Graphics.FromImage(image);
            graph.Clear(Color.Azure);


            do
            {
                StartValue = theHull[posX, posY, 0];
                TheCommand myCommand = new TheCommand(nStep, ref commands);
                res = myCommand.ExecuteOneCommand(nStep, StartValue, commands);
                nStep = res[1];
                Int64 nCurrentColor = res[0];

                if (nCurrentColor >= 0)
                {
                    // Console.WriteLine("Result: {0}", res[0]);
                    Arguments[i] = res[0];
                    i++;

                    // First, the color to paint the panel the robot is over: 0 means to paint the panel black, and 1 means to paint the panel white.
                    // Second,the direction the robot should turn: 0 means it should turn left 90 degrees, and 1 means it should turn right 90 degrees.
                    if (i >= 2) // Move and Paint!
                    {
                        Int64 nNewColor = Arguments[0];
                       
                        i = 0;
                        //let's check and count this panel
                        if (theHull[posX, posY, 1] != 1)
                        {
                            theHull[posX, posY, 1] = 1; // marked as painted
                            nPaintedPanel++;
                            //Console.WriteLine("Paint square [{0}][{1}] to {2} was {3}", posX, posY, nNewColor, nCurrentColor);
                        }
                        theHull[posX, posY, 0] = nNewColor; // painting
                        DrawRectangleRectangle(graph, posX, posY, nNewColor);
                        Direction = MoveTheRobot(ref posX, ref posY, Direction, Arguments[1]);
                    }
                }
            }
            while (nStep <= commands.Count && nStep > 0);

            image.Save("myImage.png", System.Drawing.Imaging.ImageFormat.Png);
            Console.WriteLine("Painting is finished: panels painetd {0}", nPaintedPanel);
        }

        public static void DrawRectangleRectangle(Graphics graph, int X, int Y, Int64 Color)
        {
            Pen pen = new Pen(Brushes.Black);
            Brush brushToFill = Brushes.Black;

            if (Color == 1)   brushToFill = Brushes.White;

            Rectangle rect = new Rectangle(X*10, Y*10, 10, 10);
            graph.FillRectangle(brushToFill, rect);
            graph.DrawRectangle(pen, rect);
        }

        public static char MoveTheRobot(ref int X, ref int Y, char old_direction, Int64 new_direction)
        {
            char cResult = '*';
            //new direction 0 = left, 1 = right
            if (new_direction==0)
            {
                switch (old_direction)
                {
                    case 'N':X --; cResult = 'W'; break;
                    case 'W':Y ++; cResult = 'S'; break;
                    case 'E':Y --; cResult = 'N'; break;
                    case 'S':X ++; cResult = 'E'; break;
                    default:
                        Console.WriteLine("Wrong direction: {0}", old_direction);
                        break;
                }
            }
            else
            {
                switch (old_direction)
                {
                    case 'N': X++; cResult = 'E'; break;
                    case 'W': Y--; cResult = 'N'; break;
                    case 'E': Y++; cResult = 'S'; break;
                    case 'S': X--; cResult = 'W'; break;
                    default:
                        Console.WriteLine("Wrong direction: {0}", old_direction);
                        break;
                }
            }
            return cResult;
        }
    }
}
