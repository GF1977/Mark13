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

        int vPartOneAnswer;
        var vPartTwoAnswer = "";

        AOC2020 Mark1 = new AOC2020();

        while (!file.EndOfStream)
        {
            string S = file.ReadLine();
                Mark1.AddCommand(S);
        }


            vPartOneAnswer = Mark1.Execute();



        Console.WriteLine("--------------------------");
        Console.WriteLine("PartOne: {0}", vPartOneAnswer);
        Console.WriteLine("PartTwo: {0}", vPartTwoAnswer);

    }
}
}


public class AOC2020
{
    //public Tuple<string, int> Command;
    private class Instruction
    {
        public int ID;
        public string   Command;
        public int      Argument ;
        public int ExecutionCount;
       
    }

    private int Acc { get; set; }
    private int CommandCount { get; set; }
    private int ExecutionPosition { get; set; }

    private List<Instruction> ExecutionQueue = new List<Instruction>();

    public AOC2020() 
    {
        Acc = 0;
        ExecutionPosition = 0;
        CommandCount = 0;
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
        }


        return Acc;
    }

    private bool WatchDog(Instruction Instr)
    {
        // Part one - the program would run an instruction a second time
        if (Instr.ExecutionCount >= 2)
            return true;
        else
            return false;
    }

}