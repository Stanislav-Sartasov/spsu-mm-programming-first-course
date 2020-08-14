using System.IO;

namespace Bash.Commands
{
    class Pwd : ICommand
    {

        public void Execute()
        {
            string directory = Directory.GetCurrentDirectory();

            Bash.Output.Add(directory);

            foreach (string fileName in Directory.GetFiles(directory))
            {
                Bash.Output.Add(fileName);
            }
        }
    }
}
