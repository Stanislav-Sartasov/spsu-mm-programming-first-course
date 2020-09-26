using System;
using System.Collections.Generic;
using System.Collections;

namespace Task10
{
    public class MyBash
    {
        CommandHandler commandHandler = new CommandHandler();
        string output = null;

        public void Expectation()
        {
            for(;;)
            {
                Print(commandHandler.Process(Console.ReadLine(), out CommandHandler.Keys key));
                if (key == CommandHandler.Keys.Exit)
                {
                    if (output != null)
                        Console.Write(output);
                    return;
                }
                else if (output != null)
                {
                    Console.Write(output);
                    output = null;
                }
            }
        }

        void Print(string input)
        {
            if (input == null)
                return;
            output = input + "\n";
        }
    }
}
