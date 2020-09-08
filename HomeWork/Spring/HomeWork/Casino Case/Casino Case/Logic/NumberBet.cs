using System;
using System.Collections.Generic;
using System.Text;

namespace Casino_Case.Logic
{
    public class NumberBet : Bet, IBet
    {
        public void Betting(int number, int bid)
        {
            if (number > 36 || number < 0)
                throw new ArgumentException("Entered wrong number");
            InputChecking(bid);

            Bet.AmountOfBets++;
            Random rnd = new Random();
            int value = rnd.Next(0, 36);
            Console.WriteLine("Winning number " + value);
            if (value == number)
            {
                OutputWin(bid, 35);
            }
            else
            {
                OutputLose(bid);
            }
        }
    }
}
