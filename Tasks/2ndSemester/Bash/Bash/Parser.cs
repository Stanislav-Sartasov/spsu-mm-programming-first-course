using System;
using System.Collections.Generic;
using BashLibrary;

namespace Bash
{
    public static class Parser
    {
        static string argToNext = null;

        public static List<Tuple<ICommand, string> > Parse(string input)
        {
            List<Tuple<ICommand, string>> commands = new List<Tuple<ICommand, string>>();
            argToNext = null;
            foreach (string command in input.Split('|'))
                if (!AddLocalVariable(command.Trim()))
                    if (argToNext != null)
                    {
                        commands.Add(ParseCommand(command.Trim() + " " + argToNext));
                    }
                    else
                    {
                        commands.Add(ParseCommand(command.Trim()));
                    }
                else
                {
                    argToNext = null;
                }
            return commands;
        }

        private static Tuple<ICommand, string> ParseCommand(string input)
        {
            string[] inputSplit = input.Split(' ');
            string arg = string.Join(' ', inputSplit, 1, inputSplit.Length - 1);
            if (inputSplit[0] == "echo")
            {
                arg = ChangeArg(arg);
                argToNext = arg;
            }
            else
            {
                argToNext = null;
            }
            return (inputSplit[0]) switch
            {
                "echo" => new Tuple<ICommand, string>(new Echo(), arg),
                "exit" => new Tuple<ICommand, string>(new Exit(), arg),
                "pwd" => new Tuple<ICommand, string>(new Pwd(), arg),
                "cat" => new Tuple<ICommand, string>(new Cat(), arg),
                "wc" => new Tuple<ICommand, string>(new Wc(), arg),
                _ => new Tuple<ICommand, string>(new AnotherCommand(), input),
            };
        }

        private static bool AddLocalVariable(string command)
        {
            if (command[0].Equals('$'))
                if (command.Contains("="))
                {
                    command = command.Remove(0, 1);
                    string[] splittedPart = command.Split('=');

                    if (splittedPart.Length == 2)
                    {
                        splittedPart[0] = splittedPart[0].Trim();
                        splittedPart[1] = splittedPart[1].Trim();

                        if (splittedPart[1][0].Equals('$'))
                        {
                            splittedPart[1] = splittedPart[1].Remove(0, 1);
                            if (Bash.GetLocalVariable(splittedPart[1]) != null)
                                return Bash.AddLocalVariable(splittedPart[0], Bash.GetLocalVariable(splittedPart[1]));
                        }
                        else
                        {
                            return Bash.AddLocalVariable(splittedPart[0], splittedPart[1]);
                        }
                    }
                }
            return false;
        }

        private static string ChangeArg(string command)
        {
            string[] args = command.Split(" ");
            string result = "";
            foreach (string arg in args)
                if (arg != "")
                    result += arg[0] == '$' && Bash.GetLocalVariable(arg.Remove(0, 1)) != null ? Bash.GetLocalVariable(arg.Remove(0, 1)).ToString() + " " : arg.ToString() + " ";

            return result;
         }
    }
}