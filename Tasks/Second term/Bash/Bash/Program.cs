using System;
using System.IO;
using System.Text;
using Bash.Application;

namespace Bash
{
    class Program
    {
        static void Main(string[] args)
        {
            DirectoryInfo directory = new DirectoryInfo(Directory.GetCurrentDirectory());

            string input = "asd  asdf fasdf  gdafg asdf";
            int firstSpace = input.IndexOf(" ");
            string command = input.Substring(0, firstSpace);
            StringBuilder argument = new StringBuilder(input.Substring(firstSpace + 1));
            Console.WriteLine(command);
            Console.WriteLine(argument);
            MyConsole console = new MyConsole();
            //Console.WriteLine(console.CountLinesInString(input));
            //Console.WriteLine(input.Split());
            console.Run();
        }
    }
}
