using System;
using System.Collections.Generic;
using System.Text;

namespace Roulette.Bet
{
    public class ParityBet : IBet
    {
        int parity;
        public ParityBet(int bet)
        {
            parity = bet % 2;
        }
        public bool Matches(int result, out int multiplier)
        {
            multiplier = 1;
            if (result == 0)
                return false;
            return (result % 2) == parity;
        }

        public string Print()
        {
            string result = $"Bet is ";
            if (parity == 0)
                result += "even";
            else
                result += "odd";
            return result;
        }
    }
}
