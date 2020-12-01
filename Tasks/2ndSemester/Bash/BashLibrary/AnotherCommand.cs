using System;
using System.Diagnostics;

namespace BashLibrary
{
    
    public class AnotherCommand : ICommand
    {
        public string Execute(string input)
        {
            try
            {
                string[] command = input.Split(' ');
                if (command.Length == 1)
                    Process.Start(input);
                else
                    Process.Start(command[0], string.Join(" ", command, 1, command.Length - 1));
                return "Try to Execute...";
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return "Unknown command or invalid input. Try again.";
            }
        }
    }
}
