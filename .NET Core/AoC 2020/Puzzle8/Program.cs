using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Puzzle8
{
   
    class Program
{

    static void Main()
    {
        Console.Clear();
        Console.WriteLine(DateTime.Now);
        StreamReader file = new StreamReader(@".\data.txt");

        int vPartOneAnswer = 0;
        var vPartTwoAnswer = "";

        AOC2020 Mark1 = new AOC2020();

        while (!file.EndOfStream)
        {
            string S = file.ReadLine();
            Mark1.AddCommand(S);
        }

            int nPosition = 0;
            AOC2020 Mark2 = new AOC2020(Mark1);
            while (true) 
            {
                vPartOneAnswer = Mark2.Execute();
                if (Mark2.RunStatus())
                    break;
                Mark2 = new AOC2020(Mark1);
                nPosition = Mark2.FixJumpOrNop(nPosition);
            }
            


        Console.WriteLine("--------------------------");
        Console.WriteLine("PartOne: {0}", vPartOneAnswer);


    }
}
}


public class AOC2020
{
    public static bool DEBUG_ENABLED = false;
    //public Tuple<string, int> Command;
    private class Instruction
    {
        public int ID;
        public string Command;
        public int Argument;
        public int ExecutionCount;

        public Instruction() { }
        public Instruction(Instruction Instr) 
        {
            ID = Instr.ID;
            Command = Instr.Command;
            Argument = Instr.Argument;
            ExecutionCount = Instr.ExecutionCount;
        }
    }


    private int LastExecutedCommandID { get; set; }
    private int Acc { get; set; }
    private int CommandCount { get; set; }
    private int ExecutionPosition { get; set; }
    private bool SuccessfullRun { get; set; }

    private List<Instruction> ExecutionQueue = new List<Instruction>();

    public AOC2020() 
    {
        Reset();
    }

    public AOC2020(AOC2020 Instance)
    {
        Acc = Instance.Acc;
        ExecutionPosition = Instance.ExecutionPosition;
        CommandCount = Instance.CommandCount;
        SuccessfullRun = Instance.SuccessfullRun;

        foreach (Instruction Instr in Instance.ExecutionQueue)
        {
            Instruction NewInstr = new Instruction(Instr);
            ExecutionQueue.Add(NewInstr);
        }

    }


    public void Reset()
    {
        Acc = 0;
        ExecutionPosition = 0;
        CommandCount = 0;
        SuccessfullRun = false;
    }

    public bool RunStatus()
    {
        return SuccessfullRun;
    }

    public void AddCommand(string S)
    {
        string[] NameAndArgument = S.Split(" ");

        Instruction newC = new Instruction();
        newC.ID = CommandCount;
        newC.Command   = NameAndArgument[0];
        _ = int.TryParse(NameAndArgument[1],out newC.Argument);
        newC.ExecutionCount = 0;
        
        CommandCount++;
        ExecutionQueue.Add(newC);
    }

    public int Execute()
    {
        while(true && ExecutionPosition < ExecutionQueue.Count)
        {
            Instruction Instr = ExecutionQueue[ExecutionPosition];
            Instr.ExecutionCount++;
            ExecutionPosition++; // Need to decrease it (-1) in case of JUMP command

            if (WatchDog(Instr))
                return Acc;


            // Execution
            switch (Instr.Command)
            {
                case "nop":
                        break;
                case "acc":
                    {
                        Acc += Instr.Argument;
                        break;
                    }
                case "jmp":
                    {
                        ExecutionPosition += Instr.Argument - 1; 
                        break;
                    }
                default:
                    break;
            }
            LastExecutedCommandID = Instr.ID;
        }

        SuccessfullRun = true;
        return Acc;
    }

    private bool WatchDog(Instruction Instr)
    {
        // Part one - the program would run an instruction a second time
        if (Instr.ExecutionCount >= 2)
        {
            DebugInfo(Instr);
            return true;
        }
        else
            return false;
    }

    public int FixJumpOrNop(int nPosition)
    {
        int NextPosition = 0;

        for (int i = nPosition; i < ExecutionQueue.Count; i++)
        {
            if (ExecutionQueue[i].Command.ToLower() == "nop")
            {
                ExecutionQueue[i].Command = "jmp";
                NextPosition = i + 1;
                break;
            }
            if (ExecutionQueue[i].Command.ToLower() == "jmp")
            {
                ExecutionQueue[i].Command = "nop";
                NextPosition = i + 1;
                break;
            }
        }

        return NextPosition;
    }

    private void DebugInfo(Instruction Instr)
    {
        if (DEBUG_ENABLED)
        {
            Console.WriteLine("Last executed command: {0}, {1}", ExecutionQueue[LastExecutedCommandID].Command, ExecutionQueue[LastExecutedCommandID].Argument);
            Console.WriteLine("Execution Position:    {0}", ExecutionPosition);
            Console.WriteLine("Acc:                   {0}", Acc);
        }
    }
}