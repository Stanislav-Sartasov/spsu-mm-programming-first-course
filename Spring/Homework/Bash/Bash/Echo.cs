using System;


namespace Bash.Commands
{
    class Echo : ICommand
    {
        string parameter;
        private Action<string> Output;

        public Echo(string parameter, Action<string> output)
        {
            this.parameter = parameter;
            Output = output;
        }

        public void Execute()
        {
            Output(parameter);
        }
    }
}
