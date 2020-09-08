using System;
using System.Collections.Generic;
using System.Text;

namespace Casino_Case.Logic
{
    public class ParityBet : Bet, IBet
    {
        public void Betting(int parity, int bid)
        {
            if (parity < 0 || parity > 1)    // чет нечет
                throw new ArgumentException("Entered wrong parity");
            InputChecking(bid);

            Bet.AmountOfBets++;
            Random rnd = new Random();
            int value = rnd.Next(0, 36);
            Console.WriteLine("Winning number " + value);
            if (value % 2 == parity)
            {
                OutputWin(bid, 1);
            }
            else
            {
                OutputLose(bid);
            }
        }
    }
}
