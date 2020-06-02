using System;
using System.IO;

namespace Bash.Commands
{
    class Pwd : ICommand
    {
        private Action<string> Output;

        public Pwd(Action<string> output)
        {
            Output = output;
        }

        public void Execute()
        {
            string directory = Directory.GetCurrentDirectory();

            Output(directory);

            foreach (string fileName in Directory.GetFiles(directory))
            {
                Output(fileName);
            }
        }
    }
}
