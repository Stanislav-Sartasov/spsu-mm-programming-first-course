namespace BashLibrary
{
    public class Exit : ICommand
    {
        public string Execute(string args)
        {
            if (args == "")
                try
                {
                    System.Environment.Exit(0);
                }
                catch
                {
                    return "Error. Try again.\n";
                }
            return "Error. Invalid input.\n";
        }
    }
}
