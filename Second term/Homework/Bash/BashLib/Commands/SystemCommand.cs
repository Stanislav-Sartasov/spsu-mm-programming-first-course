using System;
using System.Diagnostics;

namespace Bash.Commands
{
    class SystemProcess : ICommand
    {
        string command;

        public SystemProcess(string command)
        {
            this.command = command;
        }

        public void Execute()
        {
            try
            {
                var process = new Process();
                process.StartInfo = new ProcessStartInfo(command);
                process.StartInfo.UseShellExecute = true;
                process.Start();
            }
            catch
            {
                Bash.Printer.Print("Unable to start the process!");
            }
        }
    }
}
