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
        const int ROOM_SIZE = 50;
        static int nRoomDimensionX = 0;
        static int nRoomDimensionY = 0;

        static void Main(string[] args)
        {

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

            Int64[,] Room = new Int64[ROOM_SIZE, ROOM_SIZE];

            Int64 nStatus = -1;
            int nRobotPosX = 0;
            int nRobotPosY = 0;
            string sRobotDirection = "N";

            int posX = 0;
            int posY = 0;
            do
            {
                TheCommand myCommand = new TheCommand(nProgrammStep, ref commands);
                Int64[] res = myCommand.ExecuteOneCommand(nProgrammStep, 0, commands);
                nStatus = res[0];
                nProgrammStep = res[1];

                if (nStatus == 10)
                {
                    Console.WriteLine("");
                    posY++;
                    posX = 0;
                    nRoomDimensionY = Math.Max(nRoomDimensionY, posY);
                }
                else if (nStatus > 20 && nStatus < 128)
                {
                    char C = (char)nStatus;
                    Console.Write(C.ToString());
                    Room[posX, posY] = nStatus;
                    if (C == '^')
                    {
                        nRobotPosX = posX;
                        nRobotPosY = posY;
                    }
                    posX++;
                    nRoomDimensionX = Math.Max(nRoomDimensionX, posX);
                }


            }
            while (nProgrammStep != 0);

            Console.WriteLine("");

            int nCalibration = 0;

            for (int x = 0; x < ROOM_SIZE; x++)
                for (int y = 0; y < ROOM_SIZE; y++)
                {
                    if (Room[x, y] == 35)
                        if (IsIntersection(Room, x, y))
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
            string sDirection = "";
            string sTurn = "";
            string sCommands = "";
            Int64 nPartTwoResult = 0;
            while (true)
            {
                int nXstep = 0;
                int nYstep = 0;
                sDirection = WhereTheRoad(Room, nRobotPosX, nRobotPosY, sDirection);
                if (sDirection == "")
                    break;

                switch (sDirection)
                {
                    case "N": nYstep = -1; if (sRobotDirection == "W") sTurn = "R"; else sTurn = "L"; break;
                    case "S": nYstep = 1; if (sRobotDirection == "W") sTurn = "L"; else sTurn = "R"; break;
                    case "W": nXstep = -1; if (sRobotDirection == "N") sTurn = "L"; else sTurn = "R"; break;
                    case "E": nXstep = 1; if (sRobotDirection == "N") sTurn = "R"; else sTurn = "L"; break;
                    default: break;
                }


                sRobotDirection = sDirection;

                int nStep = 0;
                while (true)
                {
                    if (isInRoom(nRobotPosX + nXstep, nRobotPosY + nYstep))
                        if (Room[nRobotPosX + nXstep, nRobotPosY + nYstep] == 35)
                        {
                            nRobotPosX += nXstep;
                            nRobotPosY += nYstep;
                            nStep++;
                        }
                        else
                            break;
                    else
                        break;
                }

                sCommands += (sTurn);
                sCommands += (",");
                sCommands += nStep.ToString();
                sCommands += (",");

                //  Console.Write("{0},{1}, ", sTurn, nStep);
            }

            string sFunA = "";
            string sFunB = "";
            string sFunC = "";

            string sMainFun = GetMainFunction(sCommands, ref sFunA, ref sFunB, ref sFunC);

            Console.SetCursorPosition(0, 54);
            Console.WriteLine("Main function: {0}", sMainFun);
            Console.WriteLine("Function A: {0}", sFunA);
            Console.WriteLine("Function B: {0}", sFunB);
            Console.WriteLine("Function C: {0}", sFunC);

            // convert the functions to the command array
            int[] nFunctionsASCII = new int[1000];
            int nPos = 0;

            nPos = ConvertToASCII(sMainFun, ref nFunctionsASCII, nPos);
            nPos = ConvertToASCII(sFunA, ref nFunctionsASCII, nPos);
            nPos = ConvertToASCII(sFunB, ref nFunctionsASCII, nPos);
            nPos = ConvertToASCII(sFunC, ref nFunctionsASCII, nPos);
            nFunctionsASCII[nPos] = (char)'n';
            nFunctionsASCII[nPos + 1] = 10;

            commands2 = new List<Int64>(commands_vanile);
            commands2[0] = 2;
            int X = 0;
            int nInputparameter = 0;
            do
            {
                TheCommand myCommand = new TheCommand(nProgrammStep, ref commands2);
                if (myCommand.GetCommand() == 3)
                {
                    nInputparameter = nFunctionsASCII[X++];
                    Console.Write((char)nInputparameter);
                }

                Int64[] res = myCommand.ExecuteOneCommand(nProgrammStep, nInputparameter, commands2);
                nStatus = res[0];
                if (nStatus > 0) nPartTwoResult = nStatus;
                nProgrammStep = res[1];

                if (nStatus == 10)
                {
                    Console.WriteLine("");
                }
                else if (nStatus > 20 && nStatus < 128)
                {
                    char C = (char)nStatus;
                    Console.Write(C.ToString());
                }
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
        private static string GetMainFunction(string sCommands, ref string sFunA, ref string sFunB, ref string sFunC)
        {
            string sTemp = sCommands;

             sFunA = GetSubFunction(ref sTemp);
             sFunB = GetSubFunction(ref sTemp);
             sFunC = GetSubFunction(ref sTemp);

            sCommands = sCommands.Replace(sFunA, "A,");
            sCommands = sCommands.Replace(sFunB, "B,");
            sCommands = sCommands.Replace(sFunC, "C,");

            return sCommands;
        }

        private static bool isInRoom(int x, int y)
        {
            if (x >= 0 && x < nRoomDimensionX && y >= 0 && y < nRoomDimensionY)
                return true;
            else
                return false;
        }

        private static string WhereTheRoad(long[,] room, int x, int y , string sDirection)
        {
            string res = "";

                if (x>0 && room[x - 1, y] == 35 && sDirection != "E")
                    res = "W";
                else if (room[x + 1, y] == 35 && sDirection != "W")
                    res = "E";
                else if (y > 0 && room[x, y - 1] == 35 && sDirection != "S")
                    res = "N";
                else if (room[x, y + 1] == 35 && sDirection != "N")
                    res = "S";

            return res;
        }

        private static bool IsIntersection(long[,] room, int x, int y)
        {
            bool res = false;
            try
            {
                if (room[x - 1, y] == 35 && room[x + 1, y] == 35 && room[x, y - 1] == 35 && room[x, y + 1] == 35)
                    res = true;
            }
            catch
            {
                return false;
            }

            return res;
        }
    }
}
