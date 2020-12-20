using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace Puzzle16
{
    class Program
{

    class TicketsRules
        {
            public string RuleName;
            public int[] RuleSetOne = new int[2];
            public int[] RuleSetTwo = new int[2];
        }

        class Ticket
        {
            public List<int> TicketValue = new List<int>();
        }

    static void Main()
    {
        Console.Clear();
        Console.WriteLine(DateTime.Now);

            List<TicketsRules> Rules = new List<TicketsRules>();
            List<Ticket> NearbyTickets = new List<Ticket>();
            Ticket myTicket = new Ticket();
            TicketsRules TR;

            List<string> fileInput = new List<string>();
            fileInput = GetData();


            // Parsing
            // Rules
            int LinePosition = 0;
            foreach(string S in fileInput)
            {
                LinePosition++;
                if (S == "") break;

                TicketsRules Rule = new TicketsRules();

                string[] RulesRaw = S.Split(":");
                Rule.RuleName = RulesRaw[0];

                RulesRaw = RulesRaw[1].Split(" or ");
                string[] RulesSetOneRaw = RulesRaw[0].Split("-");
                Rule.RuleSetOne[0] = int.Parse(RulesSetOneRaw[0]);
                Rule.RuleSetOne[1] = int.Parse(RulesSetOneRaw[1]);


                string[] RulesSetTwoRaw = RulesRaw[1].Split("-");
                Rule.RuleSetTwo[0] = int.Parse(RulesSetTwoRaw[0]);
                Rule.RuleSetTwo[1] = int.Parse(RulesSetTwoRaw[1]);

                Rules.Add(Rule);

            }
            //my Ticket
            foreach (string S in fileInput.Skip(LinePosition+1))
            {
                LinePosition++;
                if (S == "") break;
                string[] myTicketRaw = S.Split(",");
                foreach (string sValue in myTicketRaw)
                    myTicket.TicketValue.Add(int.Parse(sValue));
            }
            //nearby Tickets
            foreach (string S in fileInput.Skip(LinePosition+2))
            {
                string[] nearTicketRaw = S.Split(",");
                Ticket nearTicket = new Ticket();
                foreach (string sValue in nearTicketRaw)
                    nearTicket.TicketValue.Add(int.Parse(sValue));

                NearbyTickets.Add(nearTicket);
            }
            // Parsing End



            // Tickets validation
            long vPartOneAnswer = 0;
            foreach (Ticket T in NearbyTickets)
            {
                foreach(int nValue in T.TicketValue)
                {
                    bool bInvalid = true;
                    foreach (TicketsRules tRules in Rules)
                    {
                        if (CheckRules(nValue, tRules))
                        {
                            bInvalid = false;
                            break;
                        }
                     
                    }
                    if(bInvalid)
                        vPartOneAnswer += nValue;

                }

            }


        // tickets validation end
            
        
        var vPartTwoAnswer = "";

        Console.WriteLine("--------------------------");
        Console.WriteLine("PartOne: {0}", vPartOneAnswer);
        Console.WriteLine("PartTwo: {0}", vPartTwoAnswer);

    }

        private static bool CheckRules(int nValue, TicketsRules ticketsRules)
        {

            if (nValue >= ticketsRules.RuleSetOne[0] && nValue <= ticketsRules.RuleSetOne[1] || nValue >= ticketsRules.RuleSetTwo[0] && nValue <= ticketsRules.RuleSetTwo[1])
                return true;
            else
                return false;
        }

        static List<string> GetData()
    {
        List<string> fileInput = new List<string>();
        string fileName = ".\\data.txt";
        if (File.Exists(@fileName))
        {
            using StreamReader file = new StreamReader(@fileName);
            while (!file.EndOfStream)
            {
                string S = file.ReadLine();
                fileInput.Add(S);
            }
        }
        else
        {
            var myFile = File.CreateText(@fileName);
            myFile.Close();
            Process.Start(@"C:\Program Files\Notepad++\notepad++.exe", fileName);
        }

        return fileInput;
    }
}
}
