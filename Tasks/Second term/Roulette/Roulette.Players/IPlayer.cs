using System;

namespace Roulette.Players
{
    public interface IPlayer
    {
        void MakeBet(int maxBet);
        int ShowMoney();
        int ShowBet();

        void MoneyRecount(int multiplier, int cashTable);
    }
}
