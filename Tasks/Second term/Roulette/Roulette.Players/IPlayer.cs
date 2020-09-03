using Roulette.Bet;

using System;

namespace Roulette.Players
{
    public interface IPlayer
    {
        void MakeBet(int maxBet);
        int ShowMoney();
        int ShowBet();
        IBet ShowField();
        void MoneyRecount(int multiplier, int cashTable);
        string ShowStatus();
    }
}
