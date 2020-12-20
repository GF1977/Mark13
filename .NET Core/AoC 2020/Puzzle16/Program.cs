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


            public bool CheckValue(int nValue)
            {
                if (nValue >= RuleSetOne[0] && nValue <= RuleSetOne[1] || nValue >= RuleSetTwo[0] && nValue <= RuleSetTwo[1])
                    return true;
                else
                    return false;
            }

        }

        class Ticket
        {
            public List<int> TicketValue = new List<int>();


            // function returns the SUMM of invalid values in the ticket
            // if the ticket is valid, the SUMM = 0, so 0 means - the ticket is valid
            public int IsTicketValid(List<TicketsRules> Rules)
            {
                int nResult = 0;
                foreach (int nValue in TicketValue)
                {
                    bool bInvalid = true;
                    foreach (TicketsRules TR in Rules)
                        if (TR.CheckValue(nValue))
                        {
                            bInvalid = false;
                            break;
                        }
                    if (bInvalid)
                        nResult += nValue;
                }
                return nResult;
            }
        }

    static void Main()
    {
        Console.Clear();
        Console.WriteLine(DateTime.Now);

            List<string> fileInput = new List<string>();
            fileInput = GetData();


            // Parsing
            // Rules
            int LinePosition = 0;
            List<TicketsRules> Rules = new List<TicketsRules>();
            foreach (string S in fileInput)
            {
                LinePosition++;
                if (S == "") break; // blank line is detected, end of the rules section

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
            LinePosition++;
            Ticket myTicket = new Ticket();
            foreach (string S in fileInput.Skip(LinePosition))
            {
                LinePosition++;
                if (S == "") break; // blank line is detected, end of the my ticket section
                myTicket = ParseToTicket(S);
            }
            //nearby Tickets
            LinePosition++;
            List<Ticket> NearbyTickets = new List<Ticket>();
            foreach (string S in fileInput.Skip(LinePosition))
                NearbyTickets.Add(ParseToTicket(S));
            // Parsing End

            // Tickets validation
            List<Ticket> ValidNearbyTickets = new List<Ticket>();
            long vPartOneAnswer = 0;
            foreach (Ticket T in NearbyTickets)
            {
                int res = T.IsTicketValid(Rules); 
                vPartOneAnswer += res;

                if (res == 0)
                    ValidNearbyTickets.Add(T);
            }
            // tickets validation end

            ValidNearbyTickets.Add(myTicket);

            // Looking for the right fields order
            // In the KeyValue we colelct the field name and the value position in the ticket
            List<KeyValuePair<string,int>> PossibleRules = new List<KeyValuePair<string, int>>();
            int nValueCount = myTicket.TicketValue.Count;
            foreach (TicketsRules Rule in Rules)
                for (int nValuePosition = 0; nValuePosition < nValueCount; nValuePosition++)
                {
                    int X = 0; 

                    foreach (Ticket T in ValidNearbyTickets)
                        if (Rule.CheckValue(T.TicketValue[nValuePosition]))
                            X++;

                    // We checked All tickets, and field nValuePosition fits to the rule X time
                    // if X = AllTickets.Count, it means this field fits for the rule
                    if (X == ValidNearbyTickets.Count)
                        PossibleRules.Add(new KeyValuePair<string, int>(Rule.RuleName, nValuePosition));
                }


            long vPartTwoAnswer = 1;
            while (PossibleRules.Count > 0)
                foreach (TicketsRules R in Rules)
                    if (PossibleRules.Count(n => n.Key.Contains(R.RuleName)) == 1)
                    // if we found this, it means only one field fits for this rule
                    // there might be a case when 1 is unavailable then we need to search all valid options
                    {
                        KeyValuePair<string, int> KVP = PossibleRules.Find(n => n.Key.Contains(R.RuleName));
                        string Rule  = KVP.Key;
                        int Value    = KVP.Value;

                        if (Rule.Contains("departure"))
                            vPartTwoAnswer *= myTicket.TicketValue[Value];

                        PossibleRules.RemoveAll(n => n.Value ==Value);
                    }

            

        Console.WriteLine("--------------------------");
        Console.WriteLine("PartOne: {0}", vPartOneAnswer);
        Console.WriteLine("PartTwo: {0}", vPartTwoAnswer);

    }

        private static Ticket ParseToTicket(string S)
        {
            Ticket T = new Ticket();
            string[] myTicketRaw = S.Split(",");
            
            foreach (string sValue in myTicketRaw)
                T.TicketValue.Add(int.Parse(sValue));

            return T;
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
