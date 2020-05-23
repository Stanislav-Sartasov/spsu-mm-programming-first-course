using System;

namespace CorrectInput
{
    public class CorrectInput
    {
        public static int IntInput(int input)
        {
            while (!(int.TryParse(Console.ReadLine(), out input)))
                Console.WriteLine("Incorrect input! Try again.");
            return input;
        }
    }
}
