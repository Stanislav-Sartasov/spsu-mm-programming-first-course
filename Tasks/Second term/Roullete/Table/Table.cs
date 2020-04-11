using System;
using System.Collections.Generic;
using Players;

namespace Casino
{
    public class Table
    {
        private struct Field
        {
            public string Color;
            public string Parity; // Zero - zero, 1 - odd, 0 - even
            public string Tier; // Tier 1 - 1 : 12, Tier 2 - 13 : 24, Tier 3 25 - 36 and zero - Tier 0
        }
        public Table(int money)
        {
            CashTable = money;
            wheel[0].Color = "Green";
            wheel[0].Parity = "Zero";
            wheel[0].Tier = "Tier 0";

            var rand = new Random();
            int temp = rand.Next(36);
            for (int i = 1; i < 37; i++)
            {
                if (i % 2 == 0)
                {
                    wheel[i].Parity = "Even";
                    wheel[(temp + i) % 36].Color = "Red";
                    wheel[(temp + i) % 36 + 1].Color = "Black";
                }
                else
                    wheel[i].Parity = "Odd";
                wheel[i].Tier = "Tier " + ((i - 1) / 12 + 1).ToString();
            }
        }
        public static List<string> RoundResult = new List<string> { "None", "None", "None", "None" }; // number, color, parity, tier
        public static int CashTable;
        private static Field[] wheel = new Field[37];
        private static Random whirligig = new Random();
        private static int spinResult;
        private int multiplier;
        private static void SpinningTop()
        {
            spinResult = whirligig.Next(0, 36);
        }

        public void Iteration()
        {
            SpinningTop();
            RoundResult[0] = spinResult.ToString();
            RoundResult[1] = wheel[spinResult].Color;
            RoundResult[2] = wheel[spinResult].Parity.ToString();
            RoundResult[3] = wheel[spinResult].Tier.ToString();
            if (spinResult == 0)
                RoundResult[0] = "Zero";
        }

        public void MoneyRecount(AbstractPlayer player)
        {
            if (player.CurrentBet.Equals(RoundResult[0]))
            {
                multiplier = 35;
                player.CurrentGameResult = 1;
                player.PreviousGamesResult.Add(1);
                player.PreviousGamesResult.RemoveAt(0);
            }
            else if (player.CurrentBet.Equals(RoundResult[1]))
            {
                multiplier = 1;
                player.CurrentGameResult = 1;
                player.PreviousGamesResult.Add(1);
                player.PreviousGamesResult.RemoveAt(0);
            }
            else if (player.CurrentBet.Equals(RoundResult[2]))
            {
                multiplier = 1;
                player.CurrentGameResult = 1;
                player.PreviousGamesResult.Add(1);
                player.PreviousGamesResult.RemoveAt(0);
            }
            else if (player.CurrentBet.Equals(RoundResult[3]))
            {
                multiplier = 2;
                player.CurrentGameResult = 1;
                player.PreviousGamesResult.Add(1);
                player.PreviousGamesResult.RemoveAt(0);
            }
            else
            {
                multiplier = 0;
                player.CurrentGameResult = 0;
                player.PreviousGamesResult.Add(0);
                player.PreviousGamesResult.RemoveAt(0);
            }
            Console.WriteLine(player.CurrentGameResult);
            int x = Math.Min(-player.CurrentCashBet + player.CurrentCashBet * (multiplier + 1 * player.CurrentGameResult), CashTable);
            CashTable -= x;
            player.PlayerCash += x;
        }

        public void DisplayInfo()
        {
            Console.WriteLine($"*******\nThe amount of money at the table - {CashTable}");
            Console.WriteLine("Round result:");
            foreach (string i in RoundResult)
                Console.WriteLine($"{i}");
            Console.WriteLine("*******");
        }
    }
}
