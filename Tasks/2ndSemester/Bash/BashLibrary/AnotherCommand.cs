using System.Diagnostics;

namespace BashLibrary
{
    
    public class AnotherCommand : ICommand
    {
        public string Execute(string input)
        {
            try
            {
                Process process = new Process
                {
                    StartInfo = new ProcessStartInfo(input)
                    {
                        UseShellExecute = true
                    }
                };
                process.Start();
                return "Try to Execute...\n";
            }
            catch
            {
                return "Unknown command or invalid input. Try again.\n";
            }
        }
    }
}
