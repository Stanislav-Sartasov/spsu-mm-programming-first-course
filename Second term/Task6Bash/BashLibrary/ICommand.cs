using System;
using System.Collections.Generic;
using System.Text;

namespace BashLibrary
{
    public interface ICommand
    {
        string Execute(string input);
    }
}
