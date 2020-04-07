using Microsoft.VisualStudio.TestTools.UnitTesting;
using Players;
using Casino;
using System;
using System.Collections.Generic;

namespace RoulleteTest
{
    [TestClass]
    public class TestGame
    {
        [TestMethod]
        public void GameOfBots()
        {
            int numberOfGames = 50;
            int numberOfBots = 3;
            List<Bot> bots = new List<Bot>(3);
            bots.Add(new Bot(1));
            bots.Add(new Bot(2));
            bots.Add(new Bot(5));
            Table table = new Table(1_000_000);

            for (int i = 0; i < numberOfGames; i++)
            {
                for (int j = 0; j < numberOfBots; j++)
                    bots[j].SetBet(Table.CashTable);
                table.Iteration();
                for (int j = 0; j < numberOfBots; j++)
                    table.MoneyRecount(bots[j]);
                for (int j = 0; j < numberOfBots; j++)
                    bots[j].DisplayInfo();
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
