using System;
using System.Collections.Generic;
using System.IO;
using MyClasses;

namespace MyClassTemplate
{
    class Program
    {

        public class networkPackage
        {
            public Int64 source;
            public Int64 destination;
            public Int64 X;
            public Int64 Y;
            public int XorY;  // 0 = X , 1 = Y
            public bool isReady;

            public networkPackage()
            {
                this.destination = -1;
                this.source = -1;
                this.X = -1;
                this.Y = -1;
                this.XorY = -1;
                this.isReady = false;
            }
        }

        static Int64[] nProgrammStep = new Int64[50];
        static Int64[] nStartValue   = new Int64[50];
        static bool[]  bAddressProvided = new bool[50];
        static List<networkPackage> PackagesQueue = new List<networkPackage>();
        static List<Int64>[] commands = new List<Int64>[50];
        static networkPackage tempPackage;
        static bool bStop;


        static void RunTheProgramm(int nComputerNumber)
        {
            Int64 nStatus;
            do
            {
                TheCommand myCommand = new TheCommand(nProgrammStep[nComputerNumber], ref commands[nComputerNumber]);
                Int64[] res = myCommand.ExecuteOneCommand(nProgrammStep[nComputerNumber], nStartValue[nComputerNumber], commands[nComputerNumber]);
                nStatus = res[0];
                nProgrammStep[nComputerNumber] = res[1];


                if (myCommand.GetCommand() == 3) // Input
                {
                    if (!bAddressProvided[nComputerNumber])
                    {
                        bAddressProvided[nComputerNumber] = true;
                        //Console.WriteLine("Computer N:{0} Network address has been assigned", nComputerNumber);
                    }
                    else
                    {
                        //Console.WriteLine("Computer N:{0} request for package", nComputerNumber);
                        int nPackage = PackagesQueue.FindIndex(n => n.destination == nComputerNumber);
                        if (nPackage >= 0)
                        {
                            //Console.WriteLine("{0} has recieved package {1} from {2}", nComputerNumber, nPackage, PackagesQueue[nPackage].source);
                            if (PackagesQueue[nPackage].XorY == 0)
                            {
                                nStartValue[nComputerNumber] = PackagesQueue[nPackage].X;
                                PackagesQueue[nPackage].XorY = 1;
                                Console.WriteLine("{0}<-{1} X:{2}", nComputerNumber, PackagesQueue[nPackage].source, PackagesQueue[nPackage].X);
                            }
                            else
                            {
                                nStartValue[nComputerNumber] = PackagesQueue[nPackage].Y;
                                Console.WriteLine("{0}<-{1} Y:{2}", nComputerNumber, PackagesQueue[nPackage].source, PackagesQueue[nPackage].Y);
                                PackagesQueue.RemoveAt(nPackage);
                            }
                        }
                        else
                        {
                            nStartValue[nComputerNumber] = -1;
                            //Console.WriteLine("Computer N:{0} is waiting for package", nComputerNumber);
                            break;
                        }
                    }
                }

                if (myCommand.GetCommand() == 4) // Output
                {
                    if (tempPackage.X >= 0 && tempPackage.Y < 0)
                    {
                        tempPackage.Y = nStatus;
                        tempPackage.XorY = 0;  // 0 means X is the first value to read
                        tempPackage.isReady = true;
                        PackagesQueue.Add(tempPackage);

                        Console.WriteLine("Computer N:{0} Package is ready:", nComputerNumber);
                        Console.WriteLine("                    Destination:{0}", tempPackage.destination.ToString());
                        Console.WriteLine("                              X:{0}", tempPackage.X.ToString());
                        Console.WriteLine("                              Y:{0}", tempPackage.Y.ToString());
                    }

                    if (tempPackage.source >= 0 && tempPackage.X < 0)
                            tempPackage.X = nStatus;


                    if (tempPackage.source < 0)
                    {
                        tempPackage.source = nComputerNumber;
                        tempPackage.destination = nStatus;
                    }


                    if (tempPackage.isReady)
                    {
                        if (tempPackage.destination == 255)
                        {
                            Console.WriteLine("----------------------------------------------");
                            Console.WriteLine("Y:{0}", tempPackage.Y.ToString());

                            bStop = true;
                        }
                        tempPackage = new networkPackage();
                    }
                }


                if (nStatus == 10)
                {
                    Console.WriteLine("");
                }
                else if (nStatus > 20 && nStatus < 128)
                {
                    Console.Write((char)nStatus);
                }
            }
            while (nProgrammStep[nComputerNumber] != 0);
        }


        static void Main(string[] args)
        {
            bStop = false;
            StreamReader file = new StreamReader(@".\data.txt");
            string line = file.ReadLine();
            string[] words = line.Split(',');

            List<Int64> commands_vanile = new List<Int64>();
            foreach (string word in words)
                commands_vanile.Add(Int64.Parse(word));

            for (int ii = 0; ii < 200000; ii++)
                commands_vanile.Add(0);



            tempPackage = new networkPackage();

            for (int i = 0; i < 50; i++)
            {
                bAddressProvided[i] = false;
                nStartValue[i] = i;
                nProgrammStep[i] = 0;
                commands[i] = new List<Int64>(commands_vanile);
                RunTheProgramm(i);
            }


            while(!bStop)
            for (int i = 0; i < 50; i++)
            {
                RunTheProgramm(i);
            }


            Console.ReadKey();
        }



 

    }
}
