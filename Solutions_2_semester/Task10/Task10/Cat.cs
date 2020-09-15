using System.IO;

namespace Task10
{
    class Cat : ICommand
    {
        public string Process(string input, out CommandHandler.Keys key)
        {
            if (!File.Exists(input))
            {
                key = CommandHandler.Keys.error;
                return "File \"" + input + "\" does not exist";
            }

            try
            {
                string output = File.ReadAllText(input);
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
