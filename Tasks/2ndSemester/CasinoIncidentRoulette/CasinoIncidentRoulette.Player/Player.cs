using System;
using System.Collections.Generic;
using CasinoIncidentRoulette.Roulette;

namespace CasinoIncidentRoulette.Player
{
    public abstract class AbstractPlayer
    {
        public int Money { get; protected set; }
        public List<Cell> History { get; protected set; }
        public abstract Tuple<Cell, int, int> Bet();
        public abstract void CheckResult(Tuple<Cell, int, int> cellBet, Cell exodus);
        public int GetMoney()
        {
            return Money;
        }
        public void AddMoney(int amount)
        {
            Money += amount;
        }

    }
}
