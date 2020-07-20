using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Bash.Handler
{
    public static class Strings
    {
        public static string InsertVar(string argument, Dictionary<string, string> variables)
        {
            StringBuilder result = new StringBuilder();
            int i = 0;
            while (i < argument.Length)
            {
                if (argument[i] == '$')
                {
                    StringBuilder name = new StringBuilder();
                    int j = i;
                    int countOfDollars = 0;
                    while (j < argument.Length && argument[j] != ' ')
                    {
                        if (argument[j] == '$')
                        {
                            if (countOfDollars == 1)
                                break;
                            countOfDollars++;
                        }
                        name.Append(argument[j]);
                        j++;
                    }
                    if (variables.ContainsKey(name.ToString()))
                    { 
                        result.Append(variables[name.ToString()]);
                        i += name.Length - 1;
                    }
                    else
                        result.Append(argument[i]);   
                }
                else
                    result.Append(argument[i]);
                i++;
            }
            return result.ToString();
        }

        public static int CountWords(string input)
        {
            int result = 0;

            bool isPrevSpace = true;
            bool isPrevLetter = false;
            int i = 0;
            while (i < input.Length)
            {
                if (input[i] == ' ' || input[i] == '\n')
                {
                    isPrevSpace = true;
                    isPrevLetter = false;
                }
                else if (isPrevSpace && !isPrevLetter)
                {
                    isPrevSpace = false;
                    isPrevLetter = true;
                    result++;
                }
                i++;
            }

            return result;
        }

        public static int CountLines(string input)
        {
            int result = 0;

            int i = 0;
            while (i < input.Length)
            {
                if (input[i] == '\n')
                    result++;
                i++;
            }

            return result;
        }

        public static string SplitToCommand(string input)
        {
            StringBuilder command = new StringBuilder();
            int i = 0;
            while (i < input.Length && input[i] == ' ')
                i++;
            while (i < input.Length)
            {
                if (input[i] == ' ')
                    break;
                else
                    command.Append(input[i]);
                i++;
            }
            return command.ToString();
        }

        public static string RemoveFirstWord(string input)
        {
            int i = 0;
            while (i < input.Length && input[i] == ' ')
                i++;
            while (i < input.Length)
            {
                if (input[i] == ' ')
                    break;
                i++;
            }
            while (i < input.Length && input[i] == ' ')
                i++;
            return input.Substring(i);
        }

        public static ICommand ChooseCommand(string input)
        {
            ICommand result;
            string command = Strings.SplitToCommand(input);
            switch (command)
            {
                case "echo":
                    {
                        result = new Echo();
                        break;
                    }
                case "pwd":
                    {
                        result = new Pwd();
                        break;
                    }
                case "cat":
                    {
                        result = new Cat();
                        break;
                    }
                case "wc":
                    {
                        result = new Wc();
                        break;
                    }
                case "|":
                    {
                        result = new Pipe();
                        break;
                    }
                default:
                    {
                        result = new Unknown();
                        break;
                    }
            }
            return result;
        }

        public static StringBuilder RemoveToFirstSymbol(string input, char key)
        {
            StringBuilder result = new StringBuilder();
            int i = 0;
            while (i < input.Length)
            {
                if (input[i] == key)
                {
                    i++;
                    break;
                }
                i++;
            }
            while (i < input.Length)
            {
                result.Append(input[i]);
                i++;
            }
            return result;
        }

        public static string SubstringToSymbol(string input, char key)
        {
            StringBuilder result = new StringBuilder();
            int i = 0;
            while (i < input.Length)
            {
                if (input[i] == key)
                    break;
                result.Append(input[i]);
                i++;
            }
            return result.ToString().Trim();
        }
    }
}
