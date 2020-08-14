using System;

namespace Bash
{
    class ConsoleController : IController
    {
        public string GetCommand()
        {
            return Console.ReadLine();
        }
    }
}
