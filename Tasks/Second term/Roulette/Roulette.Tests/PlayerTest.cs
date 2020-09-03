using Microsoft.VisualStudio.TestTools.UnitTesting;

using Roulette.Bet;
using Roulette.Players;

using System.Collections.Generic;

namespace Roulette.Tests
{
    [TestClass]
    public class PlayerTest
    {
        private IPlayer botM = CreateInstance.SelectBot(1);
        private IPlayer botR = CreateInstance.SelectBot(2);
        private List<char[]> results = new List<char[]>();
        [TestMethod]
        public void TestAmountOfMoney()
        {
            int expectedM = 50_000;
            int expectedR = 50_000;

            int actualM = botM.ShowMoney();
            int actualR = botR.ShowMoney();

            Assert.AreEqual(expectedM, actualM);
            Assert.AreEqual(expectedR, actualR);
        }

        [TestMethod]
        public void TestBet()
        {
            botM.MakeBet(50_000);
            botR.MakeBet(50_000);

            IBet betM = botM.ShowField();
            IBet betR = botR.ShowField();

            Assert.IsNotNull(betM);
            Assert.IsNotNull(betR);
        }

        [TestMethod]
        public void TestSumBet()
        {
            botM.MakeBet(50_000);
            botR.MakeBet(50_000);

            int expectedM = botM.ShowMoney() / 1000;
            int expectedRLeftBound = 1;
            int expectedRRightBound = 50_000;

            int actualM = botM.ShowBet();
            int actualR = botR.ShowBet();

            Assert.AreEqual(expectedM, actualM);
            Assert.IsTrue(expectedRLeftBound <= actualR && actualR <= expectedRRightBound);
        }

        [TestMethod]
        public void TestRecount()
        {
            botM.MakeBet(50_000);
            botR.MakeBet(50_000);
            int sumM = botM.ShowMoney();
            int sumR = botR.ShowMoney();
            botM.MoneyRecount(1, 50_000);
            botR.MoneyRecount(1, 50_000);


            int expectedM = sumM + sumM / 1000;
            int expectedRLeftBound = 50_001;
            int expectedRRightBound = 100_000;

            int actualM = botM.ShowMoney();
            int actualR = botR.ShowMoney();

            Assert.AreEqual(expectedM, actualM);
            Assert.IsTrue(expectedRLeftBound <= actualR && actualR <= expectedRRightBound);

            botM.MoneyRecount(0, 50_000);
            botR.MoneyRecount(0, 50_000);
            actualM = botM.ShowMoney();
            actualR = botR.ShowMoney();

            Assert.AreEqual(sumM, actualM);
            Assert.AreEqual(sumR, actualR);
        }

        //[TestMethod]
        //public void TestStatus()
        //{
        //    int betM = botM.ShowField();
        //    int betR = botR.ShowField();
        //    int sumBetM = botM.ShowBet();
        //    int sumBetR = botR.ShowBet();

        //    string expectedM = "Information about Martingejl\n" +
        //                    "Amount of money a player has = 50000. " +
        //                    $"Last bet is {betM[0]}{betM[1]} with the amount of money {sumBetM}\n";
        //    string expectedR = "Information about Beginner\n" +
        //                    "Amount of money a player has = 50000. " +
        //                    $"Last bet is {betR[0]}{betR[1]} with the amount of money {sumBetR}\n";

        //    string actualM = botM.ShowStatus();
        //    string actualR = botR.ShowStatus();

        //    Assert.AreEqual(expectedM, actualM);
        //    Assert.AreEqual(expectedR, actualR);
        //}
    }
}
