using System;
using System.Collections.Generic;
using BashLibrary;

namespace Bash
{
    public static class Bash
    {
        private static readonly Dictionary<string, string> localVariables = new Dictionary<string, string>();

        public static void Start()
        {
            while(true)
            {
                string input = Console.ReadLine().Trim();
                if (input == "")
                {
                    Console.WriteLine("Enter a command.");
                    continue;
                }

                List<Tuple<ICommand, string> > commands = Parser.Parse(input);

                foreach (Tuple<ICommand, string> command in commands)
                {
                    Console.Write(command.Item1.Execute(command.Item2));
                }

            }
        }

        public static string GetLocalVariable(string key)
        {
            return localVariables.TryGetValue(key, out string value) ? value : null;
        }

        public static bool AddLocalVariable(string key, string value)
        {
            bool result = localVariables.TryAdd(key, value);
            if (!result)
            {
                localVariables[key] = value;
                return true;
            }
            return result;
        }
    }
}
