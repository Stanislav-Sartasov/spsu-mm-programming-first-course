using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Task10
{
    public class InputHandler
    {
        class ParsingMachine
        {
            internal ParsingMachine()
            {
                primer["echo"] = 0;
                primer["exit"] = 1;
                primer["pwd"] = 2;
                primer["cat"] = 3;
                primer["wc"] = 4;
                primer["$"] = 5;
                primer["|"] = 6;
                primer[""] = 7;
                primer["\""] = 8;
            }

            internal int state { get; private set; } = 0;
            Hashtable primer = new Hashtable();
            int[] statesCode = new int[] { 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 1, 5 };
            int[,] table = new int[,]
            {
                { 1, 8, 8, 3, 3, 1, 0, 0, 9, 9 },
                { 2, 2, 2, 2, 2, 2, 0, 2, 2, 2 },
                { 2, 2, 2, 2, 2, 2, 0, 4, 5, 2 },
                { 6, 6, 6, 6, 6, 6, 6, 6, 7, 6 },
                { 10, 10, 10, 10, 10, 10, 0, 7, 10, 10 },
                { 11, 11, 11, 11, 11, 11, 11, 11, 11, 11 }
            };
            int[][] actions = new int[][]
            {
                new int[] { },
                new int[] { 0, 6 },
                new int[] { 1, 6 },
                new int[] { 0, 3, 7 },
                new int[] { 3 },
                new int[] { 2, 3, 5 },
                new int[] { 1, 3 },
                new int[] { 2, 3, 4 },
                new int[] { 0, 3 },
                new int[] { 8 },
                new int[] { 9, 1 },
                new int[] { 1 }
            };
            internal int[] Put(string input)
            {
                state = table[statesCode[state], primer[input] == null ? 9 : (int)primer[input]];
                return actions[state];
            }

            internal void ForceStateChange(int newState)
            {
                if (newState >= statesCode.Length || newState < 0)
                    return;
                state = newState;
            }

            internal void Reset()
            {
                state = 0;
            }
        }

        public InputHandler()
        {
            machineActions = new Action<string>[]
            {
                (input) => 
                {
                    parsed.Add(input);
                    parsed.Add("");
                },
                (input) => 
                {
                    if (forcedSplited)
                        parsed[^1] += input;
                    else
                        parsed[^1] += input + " ";
                },
                (input) =>
                {
                    if (!forcedSplited)
                        parsed[^1] += " ";
                },
                (input) =>
                {
                    if (forcedSplited)
                        errorString += input;
                    else
                        errorString += input + " ";
                },
                (input) => { quotesError = false; },
                (input) => { quotesError = true; },
                (input) => { quotesSplit = false; },
                (input) => { quotesSplit = true; },
                (input) =>
                {
                    parsed.Add("unidentified");
                    parsed.Add(input + " ");
                },
                (input) =>
                {
                    parsed[^2] = "unidentified";
                    parsed[^1] = errorString;
                },
            };
        }

        ParsingMachine machine = new ParsingMachine();
        List<string> parsed = new List<string>();
        Action<string>[] machineActions;

        bool quotesSplit;
        string errorString;
        bool quotesError;
        bool forcedSplited;

        public List<Command> Parse(string input)
        {
            if (input == "")
                return null;
            machine.Reset();
            parsed.Clear();
            quotesSplit = false;
            errorString = null;
            quotesError = false;
            forcedSplited = false;

            input = input.TrimStart();

            if (input[0..2] == "| ")
            {
                parsed.Add("pipe");
                parsed.Add("");
                input = input[2..];
            }

            foreach (string s in input.Split(' '))
                MachineHandler(machine, s);

            if (quotesError)
            {
                parsed[^2] = "unidentified";
                parsed[^1] = errorString;
            }

            List<Command> output = new List<Command>();
            for (int i = 0; i < parsed.Count; i += 2)
            {
                output.Add(new Command() { Name = parsed[i] });
                if (i + 1 < parsed.Count && parsed[i + 1] != "")
                    output[^1].Argument = parsed[i + 1][0..^1];
                else
                    output[^1].Argument = "";
            }

            return output;
        }

        void MachineHandler(ParsingMachine machine, string input)
        {
            if (quotesSplit && input.IndexOf('"') != -1 && input.Length > 1)
            {
                int count = input.Count((x) => x == '"');
                if (!((machine.state == 3 || machine.state == 4) && input[0] != '"'))
                    if (count > 2 || ((machine.state == 5 || machine.state == 6) && count > 1))
                    {
                        machine.ForceStateChange(10);
                        parsed[^2] = "unidentified";
                        parsed[^1] = errorString + input + " ";
                        quotesError = false;
                        quotesSplit = false;
                        return;
                    }

                forcedSplited = true;

                if (input.Substring(0, input.IndexOf('"')) != "")
                    MachineHandler(machine, input.Substring(0, input.IndexOf('"')));

                if (input.IndexOf('"') == input.Length - 1)
                    forcedSplited = false;

                MachineHandler(machine, "\"");

                forcedSplited = false;

                if (input.Substring(input.IndexOf('"') + 1) != "")
                    MachineHandler(machine, input.Substring(input.IndexOf('"') + 1));

                return;
            }

            if (input.Length > 1)
                if (input[0] == '$' && machine.state == 0)
                {
                    string preErrorString = errorString;
                    MachineHandler(machine, "$");
                    MachineHandler(machine, input.Substring(1));
                    errorString = preErrorString + input;
                    return;
                }

            foreach (int i in machine.Put(input))
                machineActions[i](input);
        }
    }
}
