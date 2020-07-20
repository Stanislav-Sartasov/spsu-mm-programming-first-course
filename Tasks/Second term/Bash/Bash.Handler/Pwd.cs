using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Bash.Handler
{
    public class Pwd : ICommand
    {
        public string Processing(string input)
        {
            input = Strings.RemoveFirstWord(input);
            DirectoryInfo directory = new DirectoryInfo(Directory.GetCurrentDirectory());
            StringBuilder result = new StringBuilder(directory.FullName + '\n');
            foreach (var file in directory.GetFiles())
                result.Append(file.Name + "\n");
            return result.ToString();
        }
    }
}
