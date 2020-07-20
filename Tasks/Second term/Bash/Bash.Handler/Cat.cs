using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Bash.Handler
{
    public class Cat : ICommand
    {
        public string Processing(string input)
        {
            input = Strings.RemoveFirstWord(input);
            try
            {
                return File.ReadAllText(input);
            }
            catch
            {
                return "*** !Error. Invalid path ***";
            }
        }
    }
}
