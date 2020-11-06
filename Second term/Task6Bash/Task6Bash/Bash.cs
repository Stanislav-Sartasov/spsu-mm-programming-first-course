using System;   
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BashLibrary;
using ParserLibrary;

namespace Task6Bash
{
    public class Bash
    {
        private ICommand command;
        private Dictionary<string, string> localVariables = new Dictionary<string, string>();
        public void Start()
        {
            while (true)
            {
                string input = Console.ReadLine().Trim();
                if (input == "")
                {
                    Console.WriteLine("Enter a command.");
                    continue;
                }
                    
                int index = 0;
                string substring;
                string args;
                string result = null;
                bool flag = false;
                KeyValuePair<string, string> newVariable = Parser.ParseLocalVariable(input);
                if (newVariable.Equals(new KeyValuePair<string, string>()))
                {
                    input = Parser.Replace(input, localVariables);
                    do
                    {
                        int next = Parser.ParsePipeline(input, index);
                        if (next > 0)
                        {
                            substring = input.Substring(index, next - index);
                            flag = true;
                        }
                        else if (next == -1 && flag == true)
                            substring = input.Substring(index);
                        else
                            substring = input;

                        command = GetCommand(substring);

                        if (index == 0)
                        {
                            if (Equals(command.GetType(), (new Unknown()).GetType()))
                                result = command.Execute(substring);
                            else
                            {
                                args = Parser.GetArguments(substring);
                                result = command.Execute(args);
                            }
                        }
                        else
                        {
                            if (Equals(command.GetType(), (new Unknown()).GetType()))
                                result = command.Execute(Parser.GetCommand(substring) + " " + result);
                            else
                                result = command.Execute(result);
                        }
                        index = next + 1;
                    }
                    while (index > 0);
                    Console.WriteLine(result);
                }
                else
                {
                    try
                    {
                        localVariables.Add(newVariable.Key, newVariable.Value);
                    }
                    catch (ArgumentException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

            }
        }

        public static ICommand GetCommand(string input)
        {
            ICommand command;
            string commandName = Parser.GetCommand(input);
            if (commandName == "exit")
                command = new Exit();
            else if (commandName == "echo")
                command = new Echo();
            else if (commandName == "cat")
                command = new Cat();
            else if (commandName == "pwd")
                command = new Pwd();
            else if (commandName == "wc")
                command = new Wc();
            else
                command = new Unknown();

            return command;
        }
    }
}
