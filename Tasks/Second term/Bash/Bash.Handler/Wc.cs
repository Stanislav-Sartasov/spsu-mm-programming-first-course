using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Bash.Handler
{
    public class Wc : ICommand
    {
        public string Processing(string input)
        {
            input = Strings.RemoveFirstWord(input);
            try
            {
                byte[] bytesOfText = File.ReadAllBytes(input.ToString());
                string fileText = File.ReadAllText(input.ToString());
                int amountOfWords = Strings.CountWords(fileText);
                int amountOfLines = Strings.CountLines(fileText);
                return amountOfLines.ToString() + " lines,  " + amountOfWords.ToString() + " words, " + bytesOfText.Length.ToString() + " bytes.";
            }
            catch
            {
                return "*** !Error. Invalid path ***";
            }
        }
    }
}
