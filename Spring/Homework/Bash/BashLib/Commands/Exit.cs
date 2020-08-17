namespace Bash.Commands
{
    class Exit : ICommand
    {
        public void Execute()
        {
            Bash.IsWorking = false;
        }
    }
}
