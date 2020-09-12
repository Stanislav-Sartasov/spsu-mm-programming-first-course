using System;
using System.Collections.Generic;
using System.Text;

namespace Casino_Case.Logic
{
    public class DozenBet : Bet, IBet
    {
        protected internal Table table = new Table();
        public void Betting(int dozen, int bid)
        {
            if (dozen < 1 || dozen > 3)
                throw new ArgumentException("Entered wrong dozen");
            InputChecking(bid);

            Random rnd = new Random();
            int value = rnd.Next(0, 36);
            Console.WriteLine("Winning number " + value);
            if (table.Wheel[value].Dozen == (Table.Dozens)Convert.ToInt16(dozen))
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
