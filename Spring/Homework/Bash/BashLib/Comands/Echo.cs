namespace Bash.Commands
{
    class Echo : ICommand
    {
        string argument;

        public Echo(string argument)
        {
            this.argument = argument;
        }

        public void Execute()
        {
            if (argument.Length != 0)
            {
                Bash.Output.Clear();
                Bash.Output.Add(argument);
            }
        }
    }
}
