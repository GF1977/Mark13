using MyClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


// Solution -> Add existing project (MyClasses)
// PuzzleNN -> Add reference (shared project)

namespace Puzzle17
{

    class Program
    {
        static Int64 nProgrammStep = 0;
        static List<Int64> commands;
        static List<Int64> commands2;
        const int ROOM_SIZE = 50; // it is holistic value, need to adjust if the real room is bigger
        static int[,] Room = new int[ROOM_SIZE, ROOM_SIZE];

        static int nRoomDimensionX = 0; // it will be the real dimension
        static int nRoomDimensionY = 0;

        static void Main(string[] args)
        {
            // Initialization
            StreamReader file = new StreamReader(@".\data.txt");
            string line = file.ReadLine();
            string[] words = line.Split(',');

            List<Int64> commands_vanile = new List<Int64>();
            foreach (string word in words)
                commands_vanile.Add(int.Parse(word));

            for (int i = 0; i < 8000; i++)
                commands_vanile.Add(0);

            commands = new List<Int64>(commands_vanile);

            // Part One
            Console.SetWindowSize(60, 60);

            Int64 nStatus = -1; 
            int nRobotnMapPosX = 0; 
            int nRobotnMapPosY = 0;
            string sRobotDirection = "N"; // ^ means North,  > East .. etc

            int nMapPosX = 0;
            int nMapPosY = 0;
            // Drawing the map, check the room dimension, define the Bot coordinates
            do
            {
                TheCommand myCommand = new TheCommand(nProgrammStep, ref commands);
                Int64[] res = myCommand.ExecuteOneCommand(nProgrammStep, 0, commands);
                nStatus = res[0];
                nProgrammStep = res[1];

                if (nStatus == 10)
                {
                    Console.WriteLine("");
                    nMapPosY++;   nMapPosX = 0; // Move to the next line
                    nRoomDimensionY = Math.Max(nRoomDimensionY, nMapPosY);
                }
                else if (nStatus > 20 && nStatus < 128)
                {
                    Console.Write((char)nStatus);
                    Room[nMapPosX, nMapPosY] = (int)nStatus;
                    if ((char)nStatus == '^')
                    {
                        nRobotnMapPosX = nMapPosX;
                        nRobotnMapPosY = nMapPosY;
                    }
                    nMapPosX++;
                    nRoomDimensionX = Math.Max(nRoomDimensionX, nMapPosX);
                }
            }
            while (nProgrammStep != 0);

            Console.WriteLine("");
            int nCalibration = 0; 

            for (int x = 0; x < nRoomDimensionX; x++)
                for (int y = 0; y < nRoomDimensionY; y++)
                {
                    if (Room[x, y] == 35)
                        if (IsIntersection(x, y))
                        {
                            Console.SetCursorPosition(x, y);
                            Console.WriteLine("O");
                            nCalibration += x * y;
                        }
                }

            Console.SetCursorPosition(0, 50);
            Console.WriteLine("Part One - Answer: {0}", nCalibration);

            // Part Two:
            Console.WriteLine("");
            Console.WriteLine("--Part Two--");
            string sDirection = ""; // direction of the road (N, S, W, E)
            string sTurn = "";     // where to turn (R,L)
            string sCommands = ""; // set of commands for the Bot
            Int64 nPartTwoResult = 0;

            // generating the movemenet command line "sCommands"
            while (true)
            {
                int nXstep = 0;  int nYstep = 0; // Delta -1 or 0 or +1 , depends on the direction
                int nStep = 0; // number of steps to do in one direction, before the next turn

                sDirection = WhereTheRoad(nRobotnMapPosX, nRobotnMapPosY, sDirection);
                if (sDirection == "")  break;

                switch (sDirection)
                {
                    case "N": nYstep = -1; sTurn = sRobotDirection == "W" ? "R" : "L"; break;
                    case "S": nYstep =  1; sTurn = sRobotDirection == "W" ? "L" : "R"; break;
                    case "W": nXstep = -1; sTurn = sRobotDirection == "N" ? "L" : "R"; break;
                    case "E": nXstep =  1; sTurn = sRobotDirection == "N" ? "R" : "L"; break;
                    default: break;
                }
                sRobotDirection = sDirection; // to orient the Bot in the right direction

                // counting the number of steps in the given direction                    
                while (ReadRoom(nRobotnMapPosX + nXstep, nRobotnMapPosY + nYstep) == 35)
                {
                            nRobotnMapPosX += nXstep;
                            nRobotnMapPosY += nYstep;
                            nStep++;
                }
                sCommands += sTurn + "," + nStep.ToString() + (",");
            }

            string sFunctionA = "";
            string sFunctionB = "";
            string sFunctionC = "";

            string sMainFun = GetMainFunction(sCommands, ref sFunctionA, ref sFunctionB, ref sFunctionC);

            Console.SetCursorPosition(0, 54);
            Console.WriteLine("Main function: {0}", sMainFun);
            Console.WriteLine("Function A: {0}", sFunctionA);
            Console.WriteLine("Function B: {0}", sFunctionB);
            Console.WriteLine("Function C: {0}", sFunctionC);

            // convert the functions to the command array
            int[] nFunctionsASCII = new int[1000];
            int nInputParameterPosition = 0;
            
            int nPos = 0;
            nPos = ConvertToASCII(sMainFun,   ref nFunctionsASCII, nPos);
            nPos = ConvertToASCII(sFunctionA, ref nFunctionsASCII, nPos);
            nPos = ConvertToASCII(sFunctionB, ref nFunctionsASCII, nPos);
            nPos = ConvertToASCII(sFunctionC, ref nFunctionsASCII, nPos);

            nFunctionsASCII[nPos] = (char)'n'; // no video feed
            nFunctionsASCII[nPos + 1] = 10; // end the line

            // now we are ready to launch the Bot
            commands2 = new List<Int64>(commands_vanile);
            commands2[0] = 2;
            int nInputparameter = 0;
            do
            {
                TheCommand myCommand = new TheCommand(nProgrammStep, ref commands2);
                if (myCommand.GetCommand() == 3)
                {
                    nInputparameter = nFunctionsASCII[nInputParameterPosition++];
                    Console.Write((char)nInputparameter);
                }

                Int64[] res = myCommand.ExecuteOneCommand(nProgrammStep, nInputparameter, commands2);
                nStatus = res[0];
                if (nStatus > 0) nPartTwoResult = nStatus;
                nProgrammStep = res[1];

                if (nStatus == 10)
                    Console.WriteLine("");

                else if (nStatus > 20 && nStatus < 128)
                    Console.Write((char)nStatus);
            }
            while (nProgrammStep != 0);
           
            Console.WriteLine("Part two answer: {0}", nPartTwoResult);
        }
            
        private static int ConvertToASCII (string Function, ref int[] nFunctionsASCII, int nIndex)
        {
            foreach (char C in Function)
                nFunctionsASCII[nIndex++] = (int)C;

            nFunctionsASCII[nIndex-1] = 10;
            return nIndex;
        }

        private static string GetSubFunction(ref string sTemp)
        {
            string sRes = "";
            string sRest;
            int nMaxLen = 20;
            while (nMaxLen > 0)
            {
                sRes = sTemp.Substring(0, nMaxLen);
                sRest = sTemp.Substring(nMaxLen, sTemp.Length - nMaxLen);
                int x = sRes.Length - 1;
                while (sRes[x] != ',' || sRes[x - 1] == 'R' || sRes[x - 1] == 'L')
                    sRes = sRes.Substring(0, x--);

                nMaxLen -= 4;

                if (sRest.Contains(sRes) && !sRes.Contains("A") && !sRes.Contains("B") && !sRes.Contains("C"))
                {
                    sTemp = sTemp.Replace(sRes, "");
                    break;
                }
            }
            return sRes;
        }
        private static string GetMainFunction(string sCommands, ref string sFunctionA, ref string sFunctionB, ref string sFunC)
        {
            string sTemp = sCommands;

             sFunctionA = GetSubFunction(ref sTemp);
             sFunctionB = GetSubFunction(ref sTemp);
             sFunC = GetSubFunction(ref sTemp);

            sCommands = sCommands.Replace(sFunctionA, "A,");
            sCommands = sCommands.Replace(sFunctionB, "B,");
            sCommands = sCommands.Replace(sFunC, "C,");

            return sCommands;
        }

        private static int ReadRoom(int x, int y)
        {
            if (x >= 0 && x < nRoomDimensionX && y >= 0 && y < nRoomDimensionY)
                return Room[x,y];
            else
                return -1;
        }

        private static string WhereTheRoad( int x, int y , string sDirection)
        {
            string res = "";

                if      (ReadRoom(x - 1, y) == 35 && sDirection != "E")                    res = "W";
                else if (ReadRoom(x + 1, y) == 35 && sDirection != "W")                    res = "E";
                else if (ReadRoom(x, y - 1) == 35 && sDirection != "S")                    res = "N";
                else if (ReadRoom(x, y + 1) == 35 && sDirection != "N")                    res = "S";

            return res;
        }

        private static bool IsIntersection( int x, int y)
        {
            if (ReadRoom(x - 1, y) == 35 && ReadRoom(x + 1, y) == 35 && ReadRoom(x, y - 1) == 35 && ReadRoom(x, y + 1) == 35)
                return true;
            else
                return false;
        }
    }
}
