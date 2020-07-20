using System;

namespace Bash.Handler
{
    public interface ICommand
    {
        string Processing(string input);
    }
}
