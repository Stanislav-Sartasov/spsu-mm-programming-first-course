using System;
using System.Collections.Generic;
using Players;

namespace Casino
{
    public class Table
    {
        private struct Field
        {
            public string color;
            public string parity; // 3 - zero, 1 - odd, 0 - even
            public string tier;
        }
        public Table(int money)
        {
            CashTable = money;
            Wheel[0].color = "Green";
            Wheel[0].parity = "Zero";
            Wheel[0].tier = "Tier 0";

            var rand = new Random();
            int temp = rand.Next(36);
            for (int i = 1; i < 37; i++)
            {
                if (i % 2 == 0)
                {
                    Wheel[i].parity = "Even";
                    Wheel[(temp + i) % 36].color = "Red";
                    Wheel[(temp + i) % 36 + 1].color = "Black";
                }
                else
                    Wheel[i].parity = "Odd";
                Wheel[i].tier = "Tier " + (i / 12 + 1).ToString();
            }
        }
        public static List<string> RoundResult = new List<string> { "None", "None", "None", "None" }; // number, color, parity, tier
        public static int CashTable;
        private static Field[] Wheel = new Field[37];
        private static Random Whirligig = new Random();
        private static int SpinResult;
        private int Multiplier;
        public static void SpinningTop()
        {
            SpinResult = Whirligig.Next(0, 36);
        }

        public void Iteration()
        {
            SpinningTop();
            RoundResult[0] = SpinResult.ToString();
            RoundResult[1] = Wheel[SpinResult].color;
            RoundResult[2] = Wheel[SpinResult].parity.ToString();
            RoundResult[3] = Wheel[SpinResult].tier.ToString();
            if (SpinResult == 0)
                RoundResult[0] = "Zero";
        }
        public int MoneyRecount(string playerBet, int betCash, int playerOldCash)
        {
            if (playerBet.Equals(RoundResult[0]))
                Multiplier = 35;
            else if (playerBet.Equals(RoundResult[1]))
                Multiplier = 1;
            else if (playerBet.Equals(RoundResult[2]))
                Multiplier = 1;
            else if (playerBet.Equals(RoundResult[3]))
                Multiplier = 2;
            else
                Multiplier = 0;
            CashTable -= betCash * (Multiplier - 1);
            return playerOldCash + betCash * (Multiplier - 1);
        }

        public void MoneyRecount(AbstractPlayer player)
        {
            if (player.CurrentBet.Equals(RoundResult[0]))
            {
                Multiplier = 35;
                player.CurrentGameResult = 1;
                player.PreviousGamesResult.Add(1);
                player.PreviousGamesResult.RemoveAt(0);
            }
            else if (player.CurrentBet.Equals(RoundResult[1]))
            {
                Multiplier = 1;
                player.CurrentGameResult = 1;
                player.PreviousGamesResult.Add(1);
                player.PreviousGamesResult.RemoveAt(0);
            }
            else if (player.CurrentBet.Equals(RoundResult[2]))
            {
                Multiplier = 1;
                player.CurrentGameResult = 1;
                player.PreviousGamesResult.Add(1);
                player.PreviousGamesResult.RemoveAt(0);
            }
            else if (player.CurrentBet.Equals(RoundResult[3]))
            {
                Multiplier = 2;
                player.CurrentGameResult = 1;
                player.PreviousGamesResult.Add(1);
                player.PreviousGamesResult.RemoveAt(0);
            }
            else
            {
                Multiplier = 0;
                player.CurrentGameResult = 0;
                player.PreviousGamesResult.Add(0);
                player.PreviousGamesResult.RemoveAt(0);
            }
            Console.WriteLine(player.CurrentGameResult);
            int x = Math.Min(-player.CurrentCashBet + player.CurrentCashBet * (Multiplier + 1 * player.CurrentGameResult), CashTable);
            CashTable = CashTable - x;
            player.PlayerCash = player.PlayerCash + x;
        }

        public void DisplayInfo()
        {
            Console.WriteLine($"*******\nThe amount of money at the table - {CashTable}");
            Console.WriteLine("Round result:");
            foreach (string i in RoundResult)
            {
                Console.WriteLine($"{i}");
            }
            Console.WriteLine("*******");
        }
    }
}
