
namespace Task10
{
    class Unidentified : ICommand
    {
        public string Process(string input, out CommandHandler.Keys key)
        {
            try
            {
                key = CommandHandler.Keys.ok;
                return System.Diagnostics.Process.Start(input).StandardOutput.ReadToEnd();
            }
            catch
            {
                try
                {
                    key = CommandHandler.Keys.ok;
                    return System.Diagnostics.Process.Start(input.Substring(0, input.IndexOf(' ')), input.Substring(input.IndexOf(' ') + 1, input.Length)).StandardOutput.ReadToEnd();
                }
                catch
                {
                    key = CommandHandler.Keys.error;
                    return null;
                }
            }
        }
    }
}
