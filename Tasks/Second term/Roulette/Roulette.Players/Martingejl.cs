using Roulette.Bet;

using System;
using System.Collections.Generic;
using System.Text;

namespace Roulette.Players
{
    class Martingejl : Bot
    {
        private int previousGamesResult;
        private int previousCashBet;
        private readonly int firstBet;

        public Martingejl()
        {
            name = "Martingejl";
            amountMoney = 50_000;
            previousGamesResult = 1;
            firstBet = amountMoney / 1000;
        }

        public override void MoneyRecount(int multiplier, int cashTable)
        {
            base.MoneyRecount(multiplier, cashTable);
            if (multiplier == 0)
                previousGamesResult = 0;
            else
                previousGamesResult = 1;
        }

        public override void MakeBet(int maxBet)
        {
            maxBet = Math.Min(maxBet, amountMoney);
            myBet = ChooseField();
            currentCashBet = ChooseBet(maxBet);
            previousCashBet = currentCashBet;
        }

        private IBet ChooseField()
        {
            int x = brain.Next(0, 4);
            if (x == 0)
                return new ColorBet(0);
            if (x == 1)
                return new ColorBet(1);
            if (x == 2)
                return new ParityBet(0);
            if (x == 3)
                return new ParityBet(1);
            return null;
        }

        private int ChooseBet(int maxBet)
        {
            int temp;
            if (previousGamesResult == 0)
                temp = previousCashBet * 2;
            else 
                temp = firstBet;
            temp = Math.Min(temp, maxBet);
            return temp;
        }
    }
}
