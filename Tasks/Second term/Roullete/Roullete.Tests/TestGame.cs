using Microsoft.VisualStudio.TestTools.UnitTesting;
using Roullete.Players;
using System;
using System.Collections.Generic;
using Roullete.Casino;

namespace Roullete.Tests
{
    [TestClass]
    public class TestGame
    {
        [TestMethod]
        public void GameOfBots()
        {
            int numberOfGames = 400;
            int numberOfBots = 3;
            List<Bot> bots = new List<Bot>(3);
            bots.Add(new Bot(1));
            bots.Add(new Bot(2));
            bots.Add(new Bot(5));
            Table table = new Table(1_000_000);

            for (int i = 0; i < numberOfGames; i++)
            {
                for (int j = 0; j < numberOfBots; j++)
                    bots[j].SetBet(table.CashTable);

                table.Iteration();

                for (int j = 0; j < numberOfBots; j++)
                    bots[j].UpdateInformation(table.ReturnRoundResult(), table.MoneyRecount(bots[j]));

                for (int j = 0; j < numberOfBots; j++)
                    bots[j].DisplayInfo();
                table.DisplayInfo();

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
            Assert.IsTrue(true);
        }
    }
}
