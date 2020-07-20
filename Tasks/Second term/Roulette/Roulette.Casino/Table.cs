using Roulette.Bet;
using Roulette.Players;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Roulette.Casino
{
    public class Table
    {
        private int cashTable;
        private int fieldResult = -1;
        private Random rotation = new Random();

        public Table()
        {
            cashTable = 50_000;
        }
        public Table(int cash)
        {
            cashTable = cash;
        }

        public void Iteration(List<Bot> players)
        {
            fieldResult = rotation.Next(0, 37);

            Recount(players);
        }

        private void Recount(List<Bot> players)
        {
            foreach (Bot player in players)
            {
                
                int multiplier;
                IBet playerBet = player.ShowField();
                if (playerBet.Matches(fieldResult, out multiplier))
                {
                    player.MoneyRecount(multiplier, cashTable);
                    cashTable = cashTable - Math.Min(multiplier * player.ShowBet(), cashTable);
                }
                else
                {
                    cashTable += player.ShowBet();
                    player.MoneyRecount(0, cashTable);
                }
                    
            }
        }

        public int ShowAmountOfMoney()
        {
            return cashTable;
        }

        public int ViewRoundResult()
        {
            return fieldResult;
        }

        public string ShowStatus()
        {
            string result = $"The amount of money at the table = {cashTable}\n" +
                            $"The result of the round is {fieldResult}\n";

            return result;
        }
    }
}
