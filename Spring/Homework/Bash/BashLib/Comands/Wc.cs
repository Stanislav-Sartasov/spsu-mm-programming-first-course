using System.Collections.Generic;
using System.IO;

namespace Bash.Commands
{
    class Wc : ICommand
    {
        string path;

        public Wc(string path)
        {
            

            this.path = path;
        }

        public void AddInfo(string path)
        {
            string[] lines;
            int wordsCount = 0;
            long bytesCount;
            try
            {
                lines = File.ReadAllLines(path);
                foreach (string line in lines)
                {
                    wordsCount += (line.Trim().Split(' ')).Length;
                }
                bytesCount = new FileInfo(path).Length;
                Bash.Output.Add($"{path}:");
                Bash.Output.Add("Number of lines: " + lines.Length);
                Bash.Output.Add("Number of words: " + wordsCount);
                Bash.Output.Add("Number of bytes: " + bytesCount);
            }
            catch
            {
                Bash.Printer.Print($"{path}: can't open file!");
            }
        }

        public void Execute()
        {
            var tempOutput = new List<string>(Bash.Output);
            Bash.Output.Clear();
            if (path.Length != 0)
            {
                AddInfo(path);
            }
            else if (tempOutput.Count != 0)
            {
                foreach (var element in tempOutput)
                    AddInfo(element);
            }
            else
                Bash.Printer.Print("wc: invalid arguments!");
        }
    }
}
