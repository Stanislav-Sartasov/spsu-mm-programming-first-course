using Bash.Commands;
using System;
using System.Collections.Generic;


namespace Bash
{
    public class Interpreter
    {
        private Action<string> Output;
        private Dictionary<string, string> variables;
        public Interpreter(Action<string> output)
        {
            Output = output;
            variables = new Dictionary<string, string>();
        }

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
            var commandAndParam = GetCommandAndParameter(fullCommand);
            switch (commandAndParam.Item1)
            {
                case "cat":
                    {
                        commands.Add(new Cat(commandAndParam.Item2, Output));
                        break;
                    }
                case "echo":
                    {
                        commands.Add(new Echo(commandAndParam.Item2, Output));
                        break;
                    }
                case "exit":
                    {
                        commands.Add(new Exit());
                        break;
                    }
                case "pwd":
                    {
                        commands.Add(new Pwd(Output));
                        break;
                    }

                case "wc":
                    {
                        commands.Add(new Wc(commandAndParam.Item2, Output));
                        break;
                    }
                default:
                    {
                        commands.Add(new SystemProcess(fullCommand));
                        break;
                    }
            }
        }
        private Tuple<string, string> GetCommandAndParameter(string fullCommand)
        {
            string parameter = string.Empty;
            string command = fullCommand.Split(' ')[0];
            string[] partsOfParam = fullCommand.Remove(0, command.Length).Split(' ');

            foreach (string part in partsOfParam)
            {
                if (part.Length != 0)
                {
                    if (part[0] == '$')
                    {
                        string newPart = part.Remove(0, 1);

                        if (!newPart.Contains("="))
                        {
                            if (variables.TryGetValue(newPart, out string variable))
                                parameter += variable + ' ';
                            else
                                parameter += '$' + newPart + ' ';
                        }
                        else
                            parameter += '$' + newPart + ' ';
                    }
                    else
                    {
                        parameter += part + ' ';
                    }
                }
            }
            if (parameter.Length != 0)
                parameter = parameter.Remove(parameter.Length - 1);
            return new Tuple<string, string>(command, parameter);
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
                        if (splittedPart[0].Length > 0)
                        {
                            variables.Add(splittedPart[0], splittedPart[1].Replace(" ", string.Empty));
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}
