using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Bash.Handler
{
    public class Unknown : ICommand
    {
        public string Processing(string input)
        {
            int firstSpace = input.IndexOf(" ");
            if (firstSpace == -1)
                firstSpace = input.Length;
            string command = input.Substring(0, firstSpace);
            StringBuilder argument = new StringBuilder(input.Substring(firstSpace).Trim());
            try
            {
                if (input.Length != 0)
                    return Process.Start(command, argument.ToString()).StandardOutput.ReadToEnd();
                else
                    return Process.Start(command).StandardOutput.ReadToEnd();
                //return "*** Success ***";
            }
            catch
            {
                return "*** !Error. File doesn't exists or could not be opened. ***";
            }
        }
    }
}
