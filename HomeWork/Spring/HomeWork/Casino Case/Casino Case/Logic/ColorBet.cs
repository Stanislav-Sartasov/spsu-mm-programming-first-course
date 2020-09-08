using System;
using System.Collections.Generic;
using System.Text;

namespace Casino_Case.Logic
{
    public class ColorBet : Bet, IBet
    {
        protected internal Table table = new Table();
        public void Betting(int color, int bid)
        {
            if (color < 0 || color > 1)
                throw new ArgumentException("Entered wrong color");
            InputChecking(bid);

            Bet.AmountOfBets++;
            Random rnd = new Random();
            int value = rnd.Next(0, 36);
            Console.WriteLine("Winning number " + value);
            if (table.Wheel[value].Color == (Table.Colors)Convert.ToInt16(color))
            {
                OutputWin(bid, 1);
            }
            else
            {
                OutputLose(bid);
            }
            Console.WriteLine();
        }
    }
}
