﻿using System;
using System.Collections.Generic;
using CasinoIncidentRoulette.Roulette;

namespace CasinoIncidentRoulette.Player
{
    public abstract class AbstractPlayer
    {
        public enum TypeBet
        {
            OnNumber,
            OnColor,
            OnParity,
            OnDozen
        }
        public int Money { get; protected set; }
        public List<Cell> History { get; protected set; }
        public abstract Tuple<Cell, int, TypeBet> Bet(Table Table);
        public abstract void CheckResult(Tuple<Cell, int, TypeBet> cellBet, Cell exodus);
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
