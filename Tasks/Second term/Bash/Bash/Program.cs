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
            MyConsole console = new MyConsole();
            bool continueRun = true;
            Console.WriteLine("Start");
            while (continueRun)
            {
                Console.Write(">>> ");
                string output = console.Run(Console.ReadLine().Trim());
                Console.WriteLine(output);
                if (output.Equals("*** Shutdown. ***"))
                    continueRun = false;
            }
        }
    }
}
