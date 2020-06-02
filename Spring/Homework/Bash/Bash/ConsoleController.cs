using System;
using System.Collections.Generic;
using System.Text;

namespace Bash
{
    class ConsoleController : IBashController
    {
        public string GetCommand()
        {
            return Console.ReadLine();
        }
    }
}
