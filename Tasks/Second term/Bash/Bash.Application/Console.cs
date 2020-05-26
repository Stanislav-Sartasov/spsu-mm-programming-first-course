using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Bash.Application
{
    public class MyConsole
    {
        private Dictionary<string, string> variables;

        public MyConsole()
        {
            variables = new Dictionary<string, string>();
        }

        public string Run(string input)
        { 
            input = input.Trim();
            if (input[0] == '$' || input[0] == '|')
                return OpportunityHandler(input);
            else
            {
                int firstSpace = input.IndexOf(" ");
                if (firstSpace == -1)
                    firstSpace = input.Length;
                string command = input.Substring(0, firstSpace);
                StringBuilder argument = new StringBuilder(input.Substring(firstSpace).Trim());
                return CommandHandler(command, argument);
            }
        }

        private string CommandHandler(string command, StringBuilder argument)
        {
            switch (command)
            {
                case "echo":
                    {
                        int i = 0;
                        while (i < argument.Length)
                        {
                            if (argument[i] == '$')
                            {
                                int j = i + 1;
                                StringBuilder name = new StringBuilder(100);
                                while (j < argument.Length && argument[j] != ' ')
                                {
                                    name.Append(argument[j]);
                                    j++;
                                }
                                if (variables.ContainsKey(name.ToString()))
                                {
                                    argument.Remove(i, j - i);
                                    argument.Insert(i, variables[name.ToString()]);
                                    i += variables[name.ToString()].Length;
                                }
                                else
                                    i = j;
                            }
                            i++;
                        }
                        return argument.ToString();
                    }
                case "pwd":
                    {
                        DirectoryInfo directory = new DirectoryInfo(Directory.GetCurrentDirectory());
                        StringBuilder result = new StringBuilder(directory.FullName + '\n');
                        foreach (var file in directory.GetFiles())
                            result.Append(file.Name + "\n");
                        return result.ToString();
                    }
                case "cat":
                    {
                        try
                        {
                            return File.ReadAllText(argument.ToString());
                        }
                        catch
                        {
                            return "*** !Error. Invalid path ***";
                        }
                    }
                case "wc":
                    {
                        try
                        {
                            byte[] bytesOfText = File.ReadAllBytes(argument.ToString());
                            string fileText = File.ReadAllText(argument.ToString());
                            int amountOfWords = CountWordsInString(fileText);
                            int amountOfLines = CountLinesInString(fileText);
                            return amountOfLines.ToString() + " lines,  " + amountOfWords.ToString() + " words, " + bytesOfText.Length.ToString() + " bytes.";
                        }
                        catch
                        {
                            return "*** !Error. Invalid path ***";
                        }
                    }
                case "exit":
                    {
                        return "*** Shutdown. ***";
                    }
                default:
                    {
                        try
                        {
                            if (argument.Length != 0)
                                Process.Start(command, argument.ToString());
                            else
                                Process.Start(command);
                            return "*** Success ***";
                        }
                        catch
                        {
                            return "*** !Error. File doesn't exists or could not be opened. ***";
                        }
                    }
            }
        }

        public static int CountWordsInString(string input)
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

        public static int CountLinesInString(string input)
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

        private string OpportunityHandler(string input)
        {
            char typeOfOperation = input[0];
            input = input.Remove(0, 1);
            switch (typeOfOperation)
            {
                case '$':
                    {
                        int equalSign = input.IndexOf('=');
                        if (equalSign == -1)
                            if (variables.ContainsKey(input.Trim()))
                                return variables[input.Trim()];
                            else
                                return "*** !Error. This variable does not exist. ***";
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
                                return "*** Success ***";
                            }
                            catch
                            {
                                return "*** !Error. Input error. ***";
                            }
                        }
                    }
                case '|':
                    {
                        string[] commands = input.Split();
                        int i = 0;
                        while (commands[i] == "")
                            i++;
                        string prevResult = CommandHandler(commands[i], new StringBuilder());
                        //Console.WriteLine("Result = {0}", prevResult);
                        for (i = i + 1; i < commands.Length; i++)
                            if (commands[i - 1] != "")
                                if (commands[i] != "")
                                {
                                    prevResult = CommandHandler(commands[i], new StringBuilder(prevResult));
                                    //return $"Result = {prevResult}";
                                }
                        return prevResult;
                    }
            }
            return "*** End. ***";
        }
    }
}