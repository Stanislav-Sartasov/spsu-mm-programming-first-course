using System;
using Players;
using Casino;
using System.Collections.Generic;

namespace Roullete
{
    class Roullete
    {
        static void Main(string[] args)
        {
            Player player = new Player();
            Table table = new Table(1_000_000);
            player.Initialization();
            Console.WriteLine("Enter the number of bots");
            int numberOfBots = int.Parse(Console.ReadLine());
            List<Bot> bots = new List<Bot>(numberOfBots);
            var rand = new Random();
            for (int j = 0; j < numberOfBots; j++)
            {
                byte temp = (byte)rand.Next(1, 4);
                bots.Add(new Bot(temp));
            }

            Console.WriteLine("Enter the maximum number of games");
            int numberOfGames = int.Parse(Console.ReadLine());
            for (int i = 0; i < numberOfGames; i++)
            {
                Console.WriteLine($"Game - {i + 1}");
                player.SetBet(Table.CashTable);
                for (int j = 0; j < numberOfBots; j++)
                    bots[j].SetBet(Table.CashTable);
                table.Iteration();
                table.MoneyRecount(player);
                for (int j = 0; j < numberOfBots; j++)
                    table.MoneyRecount(bots[j]);
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
                if (Table.CashTable < 1)
                {
                    Console.WriteLine("Casino are bankrupt, congratulations, you won");
                    break;
                }
            }
        }
    }
}
