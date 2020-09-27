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
            History = new List<Table.Cell>();
        }

        public bool CanIBet()
        {
            return Money >= 1;
        }

        //Favorite number bet
        public override Tuple<Table.Cell, int, int> Bet() // <a, b, c>, a - Cell, b - stake amount, c - type of bet(1 - bet on number, 2 - bet on color, 3 - bet on parity, 4 - bet on dozen)
        {
            return Tuple.Create(Table.GetCell(25), 1, 1);
        }

        public override void CheckResult(Tuple<Table.Cell, int, int> cellBet, Table.Cell exodus)
        {
            History.Add(exodus);
            if (cellBet.Item3 == 1 && cellBet.Item1.Number == exodus.Number ||
                cellBet.Item3 == 2 && cellBet.Item1.Color == exodus.Color ||
                cellBet.Item3 == 3 && cellBet.Item1.Parity == exodus.Parity ||
                cellBet.Item3 == 4 && cellBet.Item1.Dozen == exodus.Dozen)
            {
                AddMoney(cellBet.Item2 * ((cellBet.Item3 == 1) ? 35 : (cellBet.Item3 == 4) ? 2 : 1));
            }
            else
            {
                AddMoney(-cellBet.Item2);
            }

        }
    }
}
