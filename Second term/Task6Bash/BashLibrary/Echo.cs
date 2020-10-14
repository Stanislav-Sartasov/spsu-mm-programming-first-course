using System;
using ParserLibrary;

namespace BashLibrary
{
    public class Echo : ICommand
    {
        public string Execute(string args)
        {
            return args;
        }
    }
}
