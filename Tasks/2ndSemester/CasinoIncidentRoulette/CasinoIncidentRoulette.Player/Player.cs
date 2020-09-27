using System;
using System.Collections.Generic;
using CasinoIncidentRoulette.Roulette;

namespace CasinoIncidentRoulette.Player
{
    public abstract class AbstractPlayer
    {
        public int Money { get; protected set; }
        public List<Table.Cell> History { get; protected set; }
        public abstract Tuple<Table.Cell, int, int> Bet();
        public abstract void CheckResult(Tuple<Table.Cell, int, int> cellBet, Table.Cell exodus);
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
