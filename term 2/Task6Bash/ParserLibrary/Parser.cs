using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace ParserLibrary
{
    public class Parser
    {
        public static string GetCommand(string input)
        {
            StringBuilder command = new StringBuilder();
            
            input = input.Trim();
            for (int i = 0; i < input.Length && input[i] != ' '; i++)
                command.Append(input[i]);
            return command.ToString();

        }

        public static string GetArguments(string input)
        {
            StringBuilder args = new StringBuilder();
            string command = GetCommand(input);
            int index = input.IndexOf(command);
            char previousChar = input[index];
            for (int i = index + command.Length; i < input.Length; i++)
            {
                if (input[i] != ' ')
                {
                    if (previousChar == ' ' && args.Length > 0)
                        args.Append(' ');
                    args.Append(input[i]);
                }
                previousChar = input[i];    
            }
            return args.ToString();
        }

        public static int CountWords(string input)
        {
            int amount = 0;
            char previousChar;
            if (input != null)
                previousChar = input[0];
            else
                return amount;

            if (input[0] != ' ' && input[0] != '\n')
                amount = 1;

            for (int i = 1; i < input.Length; i++)
            {
                if (input[i] != ' ' && input[i] != '\n' && (previousChar == ' ' || previousChar == '\n'))
                {
                    amount++;
                }
                previousChar = input[i];
                    
            }
            return amount;
        }

        public static int CountLines(string input)
        {
            int amount = 0;
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == '\n')
                    amount++;
            }
            return amount;
        }

        public static int ParsePipeline(string input, int index)
        {
            for (int i = index; i < input.Length; i++)
            {
                if (input[i] == '|')
                    return i;
            }
            return -1;

        }

        public static KeyValuePair<string, string> ParseLocalVariable(string input)
        {
            input = input.Trim();
            KeyValuePair<string, string> pair = new KeyValuePair<string, string>();
            int index;
            if (input[0] == '$')
            {
                index = input.IndexOf('=');
                if (index == -1 || index == input.Length - 1)
                    return pair;
                else
                {
                    string keySubstring = input.Substring(0, index);
                    string valueSubstring = input.Substring(index + 1);
                    pair = new KeyValuePair<string, string>(keySubstring, valueSubstring);
                }
            }
            return pair;
        }
        public static string Replace(string input, Dictionary<string, string> dictionary)
        {
            string updatedInput = input;
            foreach (var pair in dictionary)
                updatedInput = updatedInput.Replace(pair.Key, pair.Value);

            return updatedInput;
        }
    }

    
}
