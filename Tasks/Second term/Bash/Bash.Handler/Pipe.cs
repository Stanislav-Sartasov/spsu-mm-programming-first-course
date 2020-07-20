using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bash.Handler
{
    public class Pipe : ICommand
    {
        public string Processing(string input)
        {
            int amount = input.Count(c => c == '|');
            StringBuilder processingInput = new StringBuilder(input);
            processingInput = Strings.RemoveToFirstSymbol(processingInput.ToString(), '|');
            ICommand command = Strings.ChooseCommand(processingInput.ToString());
            string argument = Strings.SubstringToSymbol(processingInput.ToString(), '|');
            argument = command.Processing(argument);
            for (int i = 1; i < amount - 1; i++)
            {
                processingInput = Strings.RemoveToFirstSymbol(processingInput.ToString(), '|');
                command = Strings.ChooseCommand(processingInput.ToString());
                argument = command.Processing(Strings.SubstringToSymbol(processingInput.ToString(), '|') + " " + argument);
            }
            return argument;
        }
    }
}
