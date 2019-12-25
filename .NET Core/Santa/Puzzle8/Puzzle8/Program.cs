using System;
using System.Collections.Generic;
using System.IO;

//The image you received is 25 pixels wide and 6 pixels tall.
//To make sure the image wasn't corrupted during transmission, the Elves would like you to find the layer that contains the fewest 0 digits.
//On that layer, what is the number of 1 digits multiplied by the number of 2 digits?

namespace Puzzle8
{
    class Program
    {
        static void Main(string[] args)
        {
            StreamReader file = new StreamReader(@".\data.txt");
            string line = file.ReadToEnd();
            //PartOne(line);
            PartTwo(line);
        }

        static void PartTwo(string line)
        {
            char[] pixels = line.ToCharArray();
            char[] pixelFlat = new char[150];

           for (int i = 0; i < 150; i++)
           {
                int nLayer = 0;
                while (nLayer < line.Length)
                {
                    int pix = int.Parse(pixels[nLayer + i].ToString());
                    if (pix == 0)
                    {
                        pixelFlat[i] = ' ';
                        break;
                    }
                    if (pix == 1)
                    {
                        pixelFlat[i] = '@';
                        break;
                    }
                    nLayer+=150;
                }
            }

            for (int c = 0; c < 6; c++)
            {
                string lineToPrint = "";
                for (int r = 0; r < 25; r++)
                    lineToPrint += pixelFlat[c * 25 + r];
                Console.WriteLine(lineToPrint);
            }

        }

        static void PartOne(string line)
        {
            int nLayer = 0;
            int res = 0;
            int minZeros = 150;
            while (nLayer < line.Length / 150)
            {
                int zeroes = 0, ones = 0, twos = 0;
                char[] pixels = line.ToCharArray(nLayer * 150, 150);
                for (int i = 0; i < 150; i++)
                {
                    switch (int.Parse(pixels[i].ToString()))
                    {
                        case 0:
                            zeroes++;
                            break;

                        case 1:
                            ones++;
                            break;

                        case 2:
                            twos++;
                            break;

                        default:
                            Console.WriteLine("Error");
                            break;
                    }

                }
                if (zeroes < minZeros)
                {
                    minZeros = zeroes;
                    res = ones * twos;
                }
                Console.WriteLine("Layer: {0}     0: {1}", nLayer, zeroes);
                Console.WriteLine("Layer: {0}     1: {1}", nLayer, ones);
                Console.WriteLine("Layer: {0}     2: {1}", nLayer, twos);
                nLayer++;
            }

            Console.WriteLine("----------------------------");
            Console.WriteLine("Res: {0}", res);
        }

    }
}
