using Bash.Commands;
using System;
using System.Collections.Generic;

namespace Bash
{
    public class Parser
    { 
        public List<ICommand> Parse(string input)
        {
            List<ICommand> commands = new List<ICommand>();

            if (input.Length == 0)
            {
                return null;
            }

            string[] pipelineCommands = input.Split('|');

            for (int i = 0; i < pipelineCommands.Length; i++)
            {
                if (pipelineCommands[i].Length != 0)
                {
                    pipelineCommands[i] = pipelineCommands[i].TrimStart();
                    if (AddVariable(pipelineCommands[i]))
                    {
                        continue;
                    }

                    AddCommand(commands, pipelineCommands[i]);
                }
            }
            return commands;
        }

        private void AddCommand(List<ICommand> commands, string fullCommand)
        {
            var commandAndArg = GetCommandAndArg(fullCommand);
            switch (commandAndArg.Item1)
            {
                case "cat":
                    {
                        commands.Add(new Cat(commandAndArg.Item2));
                        break;
                    }
                case "echo":
                    {
                        commands.Add(new Echo(commandAndArg.Item2));
                        break;
                    }
                case "exit":
                    {
                        commands.Add(new Exit());
                        break;
                    }
                case "pwd":
                    {
                        commands.Add(new Pwd());
                        break;
                    }

                case "wc":
                    {
                        commands.Add(new Wc(commandAndArg.Item2));
                        break;
                    }
                default:
                    {
                        commands.Add(new SystemProcess(commandAndArg.Item1 + commandAndArg.Item2));
                        break;
                    }
            }
        }

        private Tuple<string, string> GetCommandAndArg(string fullCommand)
        {
            string arg = string.Empty;
            string command = fullCommand.Split(' ')[0];
            string[] partsOfArg = fullCommand.Remove(0, command.Length).Split(' ');

            foreach (string part in partsOfArg)
            {
                if (part.Length != 0)
                {
                    arg += CheckVariables(part) + ' ';
                }
            }
            if (arg.Length != 0)
                arg = arg.Remove(arg.Length - 1);
            return new Tuple<string, string>(CheckVariables(command), arg);
        }

        private string CheckVariables(string str)
        {
            if (str.Length != 0)
            {
                if (str[0] == '$')
                {
                    string newPart = str.Remove(0, 1);

                    if (!newPart.Contains("="))
                    {
                        if (Bash.Variables.TryGetValue(newPart, out string variable))
                            str = variable;
                    }
                }
            }
            return str;
        }

        private bool AddVariable(string command)
        {
            if (command[0].Equals('$'))
            {
                string newPart = command.Remove(0, 1);
                if (newPart.Contains("="))
                {
                    string[] splittedPart = newPart.Split('=');
               
                    if (splittedPart.Length == 2)
                    {
                        splittedPart[0] = splittedPart[0].Trim();
                        splittedPart[1] = splittedPart[1].Trim();

                        if (splittedPart[0].Length > 0)
                        {
                            if (Bash.Variables.ContainsKey(splittedPart[0]))
                                Bash.Variables[splittedPart[0]] = splittedPart[1];
                            else
                                Bash.Variables.Add(splittedPart[0], splittedPart[1]);
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}
