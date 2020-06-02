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
                process.StartInfo.UseShellExecute = false;
                process.Start();
            }
            catch
            {
                throw new Exception("Unable to start the process!");
            }
        }
    }
}
