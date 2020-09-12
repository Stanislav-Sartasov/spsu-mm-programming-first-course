using System;
using System.Collections.Generic;
using System.Text;

namespace Casino_Case.Logic
{
    public class ColumnBet : Bet, IBet
    {
        protected internal Table table = new Table();
        public void Betting(int column, int bid)
        {
            if (column < 1 || column > 3)
                throw new ArgumentException("Entered wrong column");
            InputChecking(bid);

            Random rnd = new Random();
            int value = rnd.Next(0, 36);
            Console.WriteLine("Winning number " + value);
            if (table.Wheel[value].Column == column)
            {
                OutputWin(bid, 2);
            }
            else
            {
                OutputLose(bid);
            }
        }
    }
}
