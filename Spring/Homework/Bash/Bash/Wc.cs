using System;
using System.IO;

namespace Bash.Commands
{
    class Wc : ICommand
    {
        string path;
        private Action<string> Output;

        public Wc(string path, Action<string> output)
        {
            if (path.Length == 0)
            {
                throw new ArgumentException("No arguments!");
            }
            Output = output;
            this.path = path;
        }

        public void Execute()
        {
            string[] lines;
            int wordsCount = 0;
            long bytesCount;
            try
            {
                lines = File.ReadAllLines(path);
                foreach (string line in lines)
                {
                    wordsCount += (line.Split(' ')).Length;
                }
                bytesCount = new FileInfo(path).Length;
            }
            catch
            {
                throw new Exception("Can't open file!");
            }

            Output("Number of lines: " + lines.Length);
            Output("Number of words: " + wordsCount);
            Output("Number of bytes: " + bytesCount);
        }
    }
}
