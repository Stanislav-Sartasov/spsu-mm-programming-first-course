using System;
using System.Collections.Generic;
using CasinoIncidentRoulette.Player;
using CasinoIncidentRoulette.Roulette;

namespace CasinoIncidentRoulette.Bots
{
    public class MakarovBot : AbstractPlayer
    {
        public MakarovBot()
        {
            Money = 1000;
            History = new List<Cell>();
        }

        public override bool CanIBet()
        {
            return Money >= 1;
        }

        //Favorite number bet
        public override Tuple<Cell, int, TypeBet> Bet(Table Table) // <a, b, c>, a - Cell, b - stake amount, c - type of bet
        {
            return Tuple.Create(Table.GetCell(25), 1, TypeBet.OnNumber);
        }

        public override void CheckResult(Tuple<Cell, int, TypeBet> cellBet, Cell exodus)
        {
            History.Add(exodus);
            if (cellBet.Item3 == TypeBet.OnNumber && cellBet.Item1.Number == exodus.Number ||
                cellBet.Item3 == TypeBet.OnColor && cellBet.Item1.Color == exodus.Color ||
                cellBet.Item3 == TypeBet.OnParity && cellBet.Item1.Parity == exodus.Parity ||
                cellBet.Item3 == TypeBet.OnDozen && cellBet.Item1.Dozen == exodus.Dozen)
            {
                AddMoney(cellBet.Item2 * ((cellBet.Item3 == TypeBet.OnNumber) ? 35 : (cellBet.Item3 == TypeBet.OnDozen) ? 2 : 1));
            }
            else
            {
                AddMoney(-cellBet.Item2);
            }

        }
    }
}
