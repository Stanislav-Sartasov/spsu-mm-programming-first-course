using System;
using Roullete.Players;
using System.Collections.Generic;
using Roullete.Casino;

namespace Roullete
{
    class Roullete
    {

        // Example of bet:
        // Zero
        // 0 // equivalent to Zero
        // Tier 2
        // 24
        // Even
        // Black
        static void Main(string[] args)
        {
            Player player = new Player();

            Console.WriteLine("Enter the number of bots");
            int numberOfBots = int.Parse(Console.ReadLine());
            List<Bot> bots = new List<Bot>(numberOfBots);
            var rand = new Random();
            for (int j = 0; j < numberOfBots; j++)
                bots.Add(new Bot((byte)rand.Next(1, 4)));

            Table table = new Table(1_000_000);
            
            Console.WriteLine("Enter the maximum number of games");
            int numberOfGames = int.Parse(Console.ReadLine());

            for (int i = 0; i < numberOfGames; i++)
            {
                Console.WriteLine($"Game - {i + 1}");

                player.SetBet(table.CashTable);
                for (int j = 0; j < numberOfBots; j++)
                    bots[j].SetBet(table.CashTable);

                table.Iteration();

                player.UpdateInformation(table.ReturnRoundResult(), table.MoneyRecount(player));

                for (int j = 0; j < numberOfBots; j++)
                    bots[j].UpdateInformation(table.ReturnRoundResult(), table.MoneyRecount(bots[j]));

                table.DisplayInfo();
                player.DisplayInfo();
                for (int j = 0; j < numberOfBots; j++)
                    bots[j].DisplayInfo();

                if (player.PlayerCash < 1)
                {
                    Console.WriteLine("You are bankrupt");
                    break;
                }
                for (int j = 0; j < numberOfBots; j++)
                    if (bots[j].PlayerCash < 1)
                    {
                        Console.WriteLine($"{bots[j].PlayerName} are bankrupt");
                        bots.RemoveAt(j);
                        numberOfBots--;
                    }
                if (table.CashTable < 1)
                {
                    Console.WriteLine("Casino are bankrupt, congratulations, you won");
                    break;
                }
            }
        }
    }
}
