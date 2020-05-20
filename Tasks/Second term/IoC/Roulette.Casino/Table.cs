using Roulette.People;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Roulette.Casino
{
    public class Table
    {
        private int cashTable;
        private int roundResult = -1;
        private Random rotation = new Random();

        public Table()
        {
            cashTable = 50_000;
        }
        public Table(int cash)
        {
            cashTable = cash;
        }
        
        public void Iteration(List<AbstractPlayer> players)
        {
            roundResult = rotation.Next(0, 37);
            Recount(players);
        }

        private void Recount(List<AbstractPlayer> players)
        {
            foreach (AbstractPlayer player in players)
            {
                char[] bet = player.ViewBet();
                char[] tierResult = new char[] {'T', ((roundResult) / 12 + 1).ToString()[0] };
                char[] fieldResult = (roundResult / 10 == 0 ? new char[] { '0', roundResult.ToString()[0] } : roundResult.ToString().ToCharArray());
                char[] parityResult = (roundResult % 2 == 0 ? new char[] { 'E', 'V' } : new char[] { 'O', 'D' });
                char[] colorResult = (roundResult % 2 == 0 ? new char[] { 'B', 'L' } : new char[] { 'R', 'E' });

                if (Enumerable.SequenceEqual(bet, new char[] { 'Z', 'e' }) || Enumerable.SequenceEqual(bet, new char[] { '0', '0' }))
                {
                    player.MoneyRecount(true, 35, cashTable);
                    cashTable -= Math.Min(cashTable, 35 * player.ViewSumBet());
                }
                else if (Enumerable.SequenceEqual(bet, tierResult))
                {
                    player.MoneyRecount(true, 2, cashTable);
                    cashTable -= Math.Min(cashTable, 2 * player.ViewSumBet());
                }
                else if (Enumerable.SequenceEqual(bet, fieldResult))
                {
                    player.MoneyRecount(true, 35, cashTable);
                    cashTable -= Math.Min(cashTable, 35 * player.ViewSumBet());
                }
                else if (Enumerable.SequenceEqual(bet, parityResult))
                {
                    player.MoneyRecount(true, 1, cashTable);
                    cashTable -= Math.Min(cashTable, 1 * player.ViewSumBet());
                }
                else if (Enumerable.SequenceEqual(bet, colorResult))
                {
                    player.MoneyRecount(true, 1, cashTable);
                    cashTable -= Math.Min(cashTable, 1 * player.ViewSumBet());
                }
                else
                {
                    player.MoneyRecount(false, 0, cashTable);
                    cashTable += player.ViewSumBet();
                }
            }
        }

        public int ShowAmountOfMoney()
        {
            return cashTable;
        }
        public string ShowStatus()
        {
            string result = $"The amount of money at the table = {cashTable}\n" +
                            $"The result of the round is {roundResult}\n";

            return result;
        }
    }
}
