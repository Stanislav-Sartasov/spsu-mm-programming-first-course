using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using ParserLibrary;

namespace BashLibrary
{
    public class Unknown : ICommand
    {
        public string Execute(string input)
        {
            string command = Parser.GetCommand(input);
            string args = Parser.GetArguments(input);
            Process process = new Process();
            process.StartInfo.FileName = command;
            process.StartInfo.Arguments = args;
            process.StartInfo.UseShellExecute = true;
            try
            {
                process.Start();
                return command + " is opening...";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        
    }
}
