using Bash.Commands;
using System;
using System.Collections.Generic;

namespace Bash
{
    public class Bash
    {
        IBashController bashController;
        private Action<string> Output;
        private Interpreter interpreter;
        private bool isWorking;

        public Bash(IBashController bashController, Action<string> output)
        {
            isWorking = false;
            this.bashController = bashController;
            interpreter = new Interpreter(output);
            Output = output;
        }

        public Bash()
            : this(new ConsoleController(), s => Console.WriteLine(s))
        {

        }
        public void Start()
        {
            isWorking = true;
            Output("Started!");
            Output(GetHelp());

            while (isWorking)
            {
                try
                {
                    string input = bashController.GetCommand();

                    List<ICommand> commands = interpreter.Parse(input);

                    foreach (ICommand command in commands)
                    {
                        if (command == null)
                        {
                            return;
                        }
                        if (command is Exit)
                        {
                            isWorking = false;
                        }
                        else
                        {
                            command.Execute();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Output(ex.Message);
                }
            }
        }

        private string GetHelp()
        {
            return "echo - display argument(-s)\n" +
            "exit - exit interpreter\n" +
            "pwd - display the current working directory (name and list of files)\n" +
            "cat[FILENAME] - show file content\n" +
            "wc[FILENAME] - show number of strings, words and bytes\n" +
            "operator $ - assigning and using of local session variables\n" +
            "operator | - commmands pipelining\n";
        }
    }
}
