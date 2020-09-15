using System.IO;

namespace Task10
{
    class Pwd : ICommand
    {
        public string Process(string input, out CommandHandler.Keys key)
        {
            try
            {
                string directoryPath = Directory.GetCurrentDirectory();
                string output = directoryPath + "\n";
                DirectoryInfo directory = new DirectoryInfo(directoryPath);
                foreach (FileInfo i in directory.GetFiles()[0..^1])
                    output += "├──" + i.Name + "\n";
                output += "└──" + directory.GetFiles()[^1].Name;
                key = CommandHandler.Keys.ok;
                return output;
            }
            catch
            {
                key = CommandHandler.Keys.error;
                return null;
            }
        }
    }
}
