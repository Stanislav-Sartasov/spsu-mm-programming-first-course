using System;
using System.Collections.Generic;
using System.Text;

namespace Roulette.Bet
{
    public class ColorBet : IBet
    {
        int color;

        public ColorBet(int bet)
        {
            color = bet % 2;
        }
        public bool Matches(int result, out int multiplier)
        {
            multiplier = 1;
            if (result == 0)
                return false;
            return (result % 2) == color;
        }

        public string Print()
        {
            string result = $"Bet is ";
            if (color == 0)
                result += "red";
            else
                result += "black";
            return result;
        }
    }
}
