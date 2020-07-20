using System;

namespace Roulette.Bet
{
    public interface IBet
    {
        bool Matches(int result, out int multiplier);
        string Print();
    }
}
