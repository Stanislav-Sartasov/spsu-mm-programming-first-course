using System;
using System.IO;

namespace Bash.Commands
{
    class Cat : ICommand
    {
        string path;
        private Action<string> Output;

        public Cat(string path, Action<string> output)
        {
            if (path.Length == 0)
            {
                throw new ArgumentException("No params!");
            }
            Output = output;
            this.path = path;
        }

        public void Execute()
        {
            string[] lines;

            try
            {
                lines = File.ReadAllLines(path);
            }
            catch
            {
                throw new Exception("Can't open file!");
            }

            foreach (string line in lines)
            {
                Output(line);
            }
        }
    }
}
