using System;

namespace Bash
{
    class BashController : IController
    {
        public string GetCommand()
        {
            return Console.ReadLine();
        }
    }
}
