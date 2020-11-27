namespace BashLibrary
{
    public class Cat : ICommand
    {
        public string Execute(string input)
        {
            try
            {
                return System.IO.File.ReadAllText(input) + '\n';
            }
            catch
            {
                return "Error. Try again.\n";
            }
        }
    }
}
