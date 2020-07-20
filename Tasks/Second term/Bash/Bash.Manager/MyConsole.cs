using Bash.Handler;

using System;
using System.Collections.Generic;

namespace Bash.Manager
{
    public class MyConsole
    {
        private ICommand myCommand;
        private Dictionary<string, string> variables = new Dictionary<string, string>();
        public void Start()
        {
            Console.WriteLine("Start");
            while (true)
            {
                Console.Write(">>> ");
                string input = Console.ReadLine().Trim();
                if (input.Equals("exit"))
                {
                    Console.WriteLine("*** Exit. ***");
                    break;
                }
                if (input[0] == '$')
                {
                    int equalSign = input.IndexOf('=');
                    if (equalSign == -1)
                        if (variables.ContainsKey(input.Trim()))
                            Console.WriteLine(variables[input.Trim()]);
                        else
                            Console.WriteLine("*** !Error. This variable does not exist. ***");
                    else
                    {
                        try
                        {
                            string name = input.Substring(0, equalSign).Trim();
                            string value = input.Substring(equalSign + 1).Trim();
                            if (variables.ContainsKey(name))
                                variables[name] = value;
                            else
                                variables.Add(name, value);
                        }
                        catch
                        {
                            Console.WriteLine("*** !Error. Input error. ***");
                        }
                    }
                }
                else
                {
                    input = Strings.InsertVar(input, variables);
                    myCommand = Strings.ChooseCommand(input);
                    Console.WriteLine(myCommand.Processing(input));
                }
            }
        }
    }
}
