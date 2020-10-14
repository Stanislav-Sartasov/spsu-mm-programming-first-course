using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ParserLibrary;

namespace BashLibrary
{
    public class Cat : ICommand
    {
        public string Execute(string args)
        {
            try
            {
                return File.ReadAllText(args);
            }
            catch (FileNotFoundException e)
            {
                return e.Message;
            }
            catch
            {
                return "Error. Try again.";
            }
        }
    }
}
