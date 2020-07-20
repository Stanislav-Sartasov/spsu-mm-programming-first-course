using System;
using System.Collections.Generic;
using System.Text;

namespace Roulette.Bet
{
    public class NumberBet : IBet
    {
        int number;
        public NumberBet(int bet)
        {
            number = bet;
        }
        public bool Matches(int result, out int multiplier)
        {
            multiplier = 35;
            return number == result;
        }

        public string Print()
        {
            string result = $"Bet is {number}";
            return result;
        }
    }
}
