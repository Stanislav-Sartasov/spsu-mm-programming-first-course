using System;
using System.Collections.Generic;
using System.Text;

namespace Bash.Handler
{
    public class Echo : ICommand
    {
        public string Processing(string input)
        {
            input = Strings.RemoveFirstWord(input);
            return input;
        }
    }
}
