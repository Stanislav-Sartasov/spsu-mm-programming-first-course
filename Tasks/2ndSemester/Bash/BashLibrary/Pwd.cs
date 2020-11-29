﻿namespace BashLibrary
{
    public class Pwd : ICommand
    {
        public string Execute(string input)
        {
            if (input == "")
                return string.Join("\n", System.IO.Directory.GetFiles(System.IO.Directory.GetCurrentDirectory())) + "\n";
            return "Error. Invalid input.\n";
        }
    }
}
