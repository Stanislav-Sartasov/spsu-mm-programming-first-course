using Roulette.Bet;

using System;
using System.Collections.Generic;
using System.Text;

namespace Roulette.Players
{
    class Beginner : Bot
    {
        public Beginner()
        {
            name = "Beginner";
            amountMoney = 50_000;
        }

        public override void MakeBet(int maxBet)
        {
            myBet = ChooseField();
            currentCashBet = ChooseBet(maxBet);
        }

        private IBet ChooseField()
        {
            int x = brain.Next(0, 44);
            if (x < 37)
                return new NumberBet(x);
            if (x == 37)
                return new ColorBet(1);
            if (x == 38)
                return new ParityBet(0);
            if (x == 39)
                return new ParityBet(1);
            if (x == 40)
                return new ColorBet(0);
            if (x == 41)
                return new TierBet(0);
            if (x == 42)
                return new TierBet(1);
            if (x == 43)
                return new TierBet(2);
            return null;
        }

        private int ChooseBet(int maxBet)
        {
            return Math.Min(brain.Next(1, amountMoney + 1), maxBet);
        }

    }
}
