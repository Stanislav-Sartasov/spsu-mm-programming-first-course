using System;

namespace Bash
{
    class BashPrinter : IPrinter
    {
        public void Print(string value)
        {
            Console.WriteLine(value);
        }
    }
}
