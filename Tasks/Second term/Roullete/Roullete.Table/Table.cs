using System;
using System.Collections.Generic;
using Roullete.Players;

namespace Roullete.Casino
{
    public class Table
    {
        private struct Field
        {
            public string color;
            public string parity; // Zero - zero, 1 - odd, 0 - even
            public string tier; // Tier 1 - 1 : 12, Tier 2 - 13 : 24, Tier 3 25 - 36 and zero - Tier 0
        }

        public Table(int money)
        {
            cashTable = money;
            wheel[0].color = "Green";
            wheel[0].parity = "Zero";
            wheel[0].tier = "Tier 0";

            var rand = new Random();
            int temp = rand.Next(36);
            for (int i = 1; i < 37; i++)
            {
                if (i % 2 == 0)
                {
                    wheel[i].parity = "Even";
                    wheel[(temp + i) % 36].color = "Red";
                    wheel[(temp + i) % 36 + 1].color = "Black";
                }
                else
                    wheel[i].parity = "Odd";
                wheel[i].tier = "Tier " + ((i - 1) / 12 + 1).ToString();
            }
        }

        private List<string> roundResult = new List<string> { "None", "None", "None", "None" }; // number, color, parity, tie
        private int cashTable;
        public int CashTable => cashTable;
        private Field[] wheel = new Field[37];
        private Random whirligig = new Random();
        private int spinResult;
        private int multiplier;

        private void SpinningTop()
        {
            spinResult = whirligig.Next(0, 37);
        }

        public void Iteration()
        {
            SpinningTop();
            roundResult[0] = spinResult.ToString();
            roundResult[1] = wheel[spinResult].color;
            roundResult[2] = wheel[spinResult].parity.ToString();
            roundResult[3] = wheel[spinResult].tier.ToString();
            if (spinResult == 0)
                roundResult[0] = "Zero";
        }

        public int MoneyRecount(AbstractPlayer player)
        {
            byte gameResult = 0;
            if (player.CurrentBet.Equals(roundResult[0]))
            {
                multiplier = 35;
                gameResult = 1;
            }
            else if (player.CurrentBet.Equals(roundResult[1]))
            {
                multiplier = 1;
                gameResult = 1;
            }
            else if (player.CurrentBet.Equals(roundResult[2]))
            {
                multiplier = 1;
                gameResult = 1;
            }
            else if (player.CurrentBet.Equals(roundResult[3]))
            {
                multiplier = 2;
                gameResult = 1;
            }
            else
                multiplier = 0;
            int x = Math.Min(-player.CurrentCashBet + player.CurrentCashBet * (multiplier + 1 * gameResult), CashTable);
            cashTable -= x;
            return player.PlayerCash + x;
        }

        public void DisplayInfo()
        {
            Console.WriteLine($"*******\nThe amount of money at the table - {CashTable}");
            Console.WriteLine("Round result:");
            foreach (string i in roundResult)
                Console.WriteLine($"{i}");
            Console.WriteLine("*******");
        }

        public List<string> ReturnRoundResult()
        {
            return roundResult;
        }
    }
}
