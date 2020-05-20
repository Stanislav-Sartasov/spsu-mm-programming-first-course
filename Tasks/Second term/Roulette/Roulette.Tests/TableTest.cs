using Microsoft.VisualStudio.TestTools.UnitTesting;
using Roulette.Casino;
using Roulette.People;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roulette.Tests
{
    [TestClass]
    public class TableTest
    {
        [TestMethod]
        public void TestConstructorsAndShowAmountOfMoney()
        {
            int expectedFirst = 50_000;
            int expectedSecond = 100_000;
            Table tableFirst = new Table();
            Table tableSecond = new Table(100_000);

            Assert.IsNotNull(tableFirst);
            Assert.IsNotNull(tableSecond);

            int actualFirst = tableFirst.ShowAmountOfMoney();
            int actualSecond = tableSecond.ShowAmountOfMoney();

            Assert.AreEqual(expectedFirst, actualFirst);
            Assert.AreEqual(expectedSecond, actualSecond);
        }

        [TestMethod]
        public void TestShowStatus()
        {
            Table table = new Table(100_000);

            string expected = $"The amount of money at the table = 100000\n" +
                              $"The result of the round is -1\n";
            string actual = table.ShowStatus();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGame()
        {
            Table table = new Table(100_000);
            List<AbstractPlayer> bots = new List<AbstractPlayer>() { new Bot(0), new Bot(1), new Bot(2) };

            for (int i = 0; i < 400; i++)
            {
                List<AbstractPlayer> delList = new List<AbstractPlayer>();
                foreach (AbstractPlayer bot in bots)
                    bot.SetBet(table.ShowAmountOfMoney());
                table.Iteration(bots);
                foreach (AbstractPlayer bot in bots)
                {
                    int x = bot.ViewAmountOfMoney();
                    if (x < 1)
                        delList.Add(bot);
                }
                foreach (AbstractPlayer delBot in delList)
                    bots.Remove(delBot);
                if (table.ShowAmountOfMoney() < 1)
                    break;
                if (bots.Count == 0)
                    break;
            }

            Assert.IsTrue(0 <= table.ShowAmountOfMoney() && table.ShowAmountOfMoney() <= (100_000 + 3 * 50_000));
            Assert.IsTrue(0 <= bots.Count && bots.Count <= 3);
        }
    }
}
