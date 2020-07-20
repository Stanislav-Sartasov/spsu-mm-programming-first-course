using System;
using System.Collections.Generic;
using System.Text;

namespace Roulette.Bet
{
    public class TierBet : IBet
    {
        int tier;
        public TierBet(int bet)
        {
            tier = bet;
        }
        public bool Matches(int result, out int multiplier)
        {
            multiplier = 2;
            if (result == 0)
                return false;
            return tier == ((result - 1) / 3);
        }

        public string Print()
        {
            string result = $"Bet is ";
            if (tier == 0)
                result += "tier 1";
            else if (tier == 1)
                result += "tier 2";
            else
                result += "tier 3";
            return result;
        }
    }
}
