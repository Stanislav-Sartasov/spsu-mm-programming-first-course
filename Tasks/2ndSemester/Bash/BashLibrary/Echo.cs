namespace BashLibrary
{
    public class Echo : ICommand
    {
        public string Execute(string input)
        {
            return input;
        }
    }
}
