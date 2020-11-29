using System;
using System.Collections.Generic;
using System.IO;
using MyClasses;

namespace MyClassTemplate
{
    class Program
    {
        public class networkPacket
        {
            public Int64 source;
            public Int64 destiNATion;
            public Int64 X;
            public Int64 Y;
            public char  XorY;  // Indicates which Value needs to be send
            public bool isReady;

            public networkPacket()
            {
                this.destiNATion = -1;
                this.source = -1;
                this.X = -1;
                this.Y = -1;
                this.XorY = 'X';
                this.isReady = false;
            }

            public networkPacket(networkPacket NP)
            {
                this.destiNATion = NP.destiNATion;
                this.source = NP.source;
                this.X = NP.X;
                this.Y = NP.Y;
                this.XorY = NP.XorY;
                this.isReady = NP.isReady;
            }
        }

        static Int64[] nProgrammStep = new Int64[50];
        static Int64[] nStartValue   = new Int64[50];
        static bool[]  bAddressProvided = new bool[50];
        static List<networkPacket> PacketsQueue = new List<networkPacket>();
        static List<Int64>[] commands = new List<Int64>[50];
        static networkPacket tempPacket;
        static networkPacket NATPacket;
        static bool bStop;

        static Int64 NAT_Y;

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
                    }
                    else
                    {
                        int nPacket = PacketsQueue.FindIndex(n => n.destiNATion == nComputerNumber);
                        if (nPacket >= 0)
                        {
                            if (PacketsQueue[nPacket].XorY == 'X')
                            {
                                nStartValue[nComputerNumber] = PacketsQueue[nPacket].X;
                                PacketsQueue[nPacket].XorY = 'Y';
                            }
                            else
                            {
                                nStartValue[nComputerNumber] = PacketsQueue[nPacket].Y;
                                PacketsQueue.RemoveAt(nPacket);
                            }
                        }
                        else // new packet
                        {
                            nStartValue[nComputerNumber] = -1;
                            break;
                        }
                    }
                }

                if (myCommand.GetCommand() == 4) // Output
                {
                    tempPacket = CreatePacket(tempPacket, nStatus, nComputerNumber);

                    if (tempPacket.isReady)
                    {
                        // 255 - NAT
                        if (tempPacket.destiNATion == 255)
                        {
                            // Need to decetc first packet to 255 (NAT)
                            if (NATPacket is null)
                                Console.WriteLine("PART ONE   Y:{0}", tempPacket.Y.ToString());

                            NATPacket = new networkPacket(tempPacket);
                        }
                        // NAT packet is not added to the queue
                        else
                            PacketsQueue.Add(tempPacket);

                        tempPacket = new networkPacket();
                    }
                }
            }
            while (nProgrammStep[nComputerNumber] != 0);
        }


        static networkPacket CreatePacket(networkPacket tempPacket, Int64 nValue, Int64 source)
        {

            if (tempPacket.source < 0)
            {
                tempPacket.source = source;
                tempPacket.destiNATion = nValue;
            }
            else if (tempPacket.source >= 0 && tempPacket.X < 0)
                tempPacket.X = nValue;
            else if (tempPacket.X >= 0 && tempPacket.Y < 0)
            {
                tempPacket.Y = nValue;
                tempPacket.XorY = 'X';
                tempPacket.isReady = true;
            }

            return tempPacket;
        }

        static void Main(string[] args)
        {
            bStop = false;
            NAT_Y = -1; // Y value in NAT package to detecect two Y in a row

            StreamReader    file    = new StreamReader(@".\data.txt");
            string          line    = file.ReadLine();
            string[]        words   = line.Split(',');

            List<Int64> commands_vanilla = new List<Int64>();
            foreach (string word in words)
                commands_vanilla.Add(Int64.Parse(word));

            // Extending command space 
            for (int ii = 0; ii < 200000; ii++) 
                commands_vanilla.Add(0);


            tempPacket = new networkPacket();

            // initialization
            for (int i = 0; i < 50; i++)
            {
                bAddressProvided[i] = false;
                nStartValue[i] = i;
                nProgrammStep[i] = 0;
                commands[i] = new List<Int64>(commands_vanilla);
                RunTheProgramm(i);
            }

            // processing
            while (!bStop)
            {
                // Part ONE
                // Complete cycle - run all computers
                for (int i = 0; i < 50; i++)
                    RunTheProgramm(i);

                // Part TWO
                // Checking the queue lenght and NAT package
                if (PacketsQueue.Count == 0 && NATPacket != null)
                {
                    NATPacket.destiNATion = 0;
                    PacketsQueue.Add(NATPacket);

                    if (NATPacket.XorY == 'Y')
                    {
                        if (NAT_Y == NATPacket.Y)
                        {
                            Console.WriteLine("PART TWO   Y:{0}", NAT_Y.ToString());
                            bStop = true;
                        }
                        else
                            NAT_Y = NATPacket.Y;
                    }
                }
            }
            Console.WriteLine("Press any key");
            Console.ReadKey();
        }
    }
}
