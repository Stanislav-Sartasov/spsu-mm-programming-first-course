using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Bash.Commands
{
    class Cat : ICommand
    {
        string path;
        public Cat(string path)
        {
            this.path = path;
        }

        public void AddLines(string path)
        {
            List<string> lines = new List<string>();
            try 
            {
                lines.AddRange(from line in File.ReadAllLines(path)
                               select line);
                Bash.Output.AddRange(lines);
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
                AddLines(path);
            else if (tempOutput.Count != 0)
                foreach (var path in tempOutput)
                    AddLines(path);   
            else
                Bash.Printer.Print("cat: invalid arguments!");
        }
    }
}
