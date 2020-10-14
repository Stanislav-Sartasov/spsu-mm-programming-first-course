using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ParserLibrary;

namespace BashLibrary
{
    public class Wc : ICommand
    {
        public string Execute(string args)
        {
            try
            {
                string path = args;
                byte[] bytes = File.ReadAllBytes(path);
                string text = File.ReadAllText(path);
                string result = "Amount of lines: " + Parser.CountLines(text)
                    + "\nAmount of words: " + Parser.CountWords(text) +
                    "\nAmount of bytes: " + bytes.Length;
                return result;
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
