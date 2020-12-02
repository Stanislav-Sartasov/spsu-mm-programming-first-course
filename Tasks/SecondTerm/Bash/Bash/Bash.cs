using System;
using System.Collections.Generic;
using BashLibrary;

namespace Bash
{
    public static class Bash
    {
        private static Dictionary<string, string> localVariables = new Dictionary<string, string>();

        public static void Start()
        {
            while (true)
            {
                Console.Write("> ");
                string input = Console.ReadLine().Trim();
                if (input == "")
                {
                    Console.WriteLine("Enter a command.");
                    continue;
                }

                List<Tuple<ICommand, string>> commands = Parser.Parse(input);

                string result = "";
                for (int i = 0; i < commands.Count - 1; i++)
                {
                    result = commands[i].Item1.Execute(commands[i].Item2);
                    commands[i + 1] = new Tuple<ICommand, string>(commands[i + 1].Item1, commands[i + 1].Item2 + result);
                    Console.WriteLine(result);
                }
                if (commands.Count > 0)
                {
                    Console.WriteLine(commands[commands.Count - 1].Item1.Execute(commands[commands.Count - 1].Item2));
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
