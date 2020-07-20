using Microsoft.VisualStudio.TestTools.UnitTesting;
using Roulette.Casino;
using Roulette.Players;

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

            int expected = -1;
            int actual = table.ViewRoundResult();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGame()
        {
            Table table = new Table(100_000);
            List<Bot> bots = new List<Bot>() { CreateInstance.SelectBot(1), CreateInstance.SelectBot(2)};

            for (int i = 0; i < 400; i++)
            {
                List<Bot> delList = new List<Bot>();
                foreach (Bot bot in bots)
                    bot.MakeBet(table.ShowAmountOfMoney());
                table.Iteration(bots);
                foreach (Bot bot in bots)
                {
                    int x = bot.ShowMoney();
                    if (x < 1)
                        delList.Add(bot);
                }
                foreach (Bot delBot in delList)
                    bots.Remove(delBot);
                if (table.ShowAmountOfMoney() < 1)
                    break;
                if (bots.Count == 0)
                    break;
            }

            Assert.IsTrue(0 <= table.ShowAmountOfMoney() && table.ShowAmountOfMoney() <= (100_000 + 3 * 50_000));
            Assert.IsTrue(0 <= bots.Count && bots.Count <= 2);
        }
    }
}
