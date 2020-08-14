using System.Collections.Generic;

namespace Bash
{
    public static class Bash
    {
        private static Parser parser;
        public static IController BashController { get; set; }
        public static IPrinter Printer { get; set; }
        internal static bool IsWorking { get; set; }
        internal static List<string> Output { get; set; }
        internal static Dictionary<string, string> Variables { get; set; }
        

        static Bash()
        {
            IsWorking = false;
            BashController = new BashController();
            parser = new Parser();
            Variables = new Dictionary<string, string>();
            Printer = new BashPrinter();
            Output = new List<string>();
        }

        public static void Start()
        {
            IsWorking = true;
            Printer.Print("Started!");
            Printer.Print(GetHelp());

            while (IsWorking)
            {
                string input = BashController.GetCommand();

                List<ICommand> commands = parser.Parse(input);

                if (commands != null)
                {
                    for (int i = 0; i < commands.Count; i++)
                    {
                        ICommand command = commands[i];
                        if (command == null)
                        {
                            return;
                        }
                        command.Execute();
                    }
                }
                foreach (var line in Output)
                {
                    Printer.Print(line);
                }
                Output.Clear();
            }
        }

        private static string GetHelp()
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
