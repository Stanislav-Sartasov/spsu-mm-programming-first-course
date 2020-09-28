using System;
using System.Collections.Generic;
using System.Security;
using System.Text;

namespace BashLibrary
{
    public class Exit : ICommand
    {
        public string Execute(string args)
        {
            try
            {
                Environment.Exit(0);
                return null;
            }
            catch (SecurityException e)
            {
                return e.Message;
            }
        }
    }
}
