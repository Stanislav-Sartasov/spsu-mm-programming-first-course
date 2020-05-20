using Microsoft.VisualStudio.TestTools.UnitTesting;
using Roulette.People;
using System.Collections.Generic;

namespace Roulette.Tests
{
    [TestClass]
    public class PlayerTest
    {
        private Bot botM = new Bot(1);
        private Bot botD = new Bot(2);
        private Bot botR = new Bot(3);
        private List<char[]> results = new List<char[]>();
        [TestMethod]
        public void TestAmountOfMoney()
        {
            int expectedM = 50_000;
            int expectedD = 50_000;
            int expectedR = 50_000;

            int actualM = botM.ViewAmountOfMoney();
            int actualD = botD.ViewAmountOfMoney();
            int actualR = botR.ViewAmountOfMoney();

            Assert.AreEqual(expectedM, actualM);
            Assert.AreEqual(expectedD, actualD);
            Assert.AreEqual(expectedR, actualR);
        }

        [TestMethod]
        public void TestBet()
        {
            botM.SetBet(50_000);
            botD.SetBet(50_000);
            botR.SetBet(50_000);
            for (int i = 0; i < 37; i++)
                results.Add(i / 10 == 0 ? new char[] { '0', i.ToString()[0] } : i.ToString().ToCharArray());
            for (int i = 1; i < 4; i++)
                results.Add(new char[] { 'T', i.ToString()[0] });
            results.Add(new char[] { 'R', 'E' });
            results.Add(new char[] { 'B', 'L' });
            results.Add(new char[] { 'E', 'V' });
            results.Add(new char[] { 'O', 'D' });

            char[] betM = botM.ViewBet();
            char[] betD = botD.ViewBet();
            char[] betR = botR.ViewBet();
            bool resM = false;
            bool resD = false;
            bool resR = false;

            foreach (char[] bet in results)
            {
                if (betM[0] == bet[0] && betM[1] == bet[1])
                    resM = true;
                if (betR[0] == bet[0] && betR[1] == bet[1])
                    resR = true;
                if (betD[0] == bet[0] && betD[1] == bet[1])
                    resD = true;
            }

            Assert.IsTrue(resM);
            Assert.IsTrue(resD);
            Assert.IsTrue(resR);
        }

        [TestMethod]
        public void TestSumBet()
        {
            botM.SetBet(50_000);
            botD.SetBet(50_000);
            botR.SetBet(50_000);

            int expectedM = botM.ViewAmountOfMoney() / 1000;
            int expectedD = botD.ViewAmountOfMoney() / 20;
            int expectedRLeftBound = 1;
            int expectedRRightBound = 50_000;

            int actualM = botM.ViewSumBet();
            int actualD = botD.ViewSumBet();
            int actualR = botR.ViewSumBet();

            Assert.AreEqual(expectedM, actualM);
            Assert.AreEqual(expectedD, actualD);
            Assert.IsTrue(expectedRLeftBound <= actualR && actualR <= expectedRRightBound);
        }

        [TestMethod]
        public void TestRecount()
        {
            botM.SetBet(50_000);
            botD.SetBet(50_000);
            botR.SetBet(50_000);
            int sumM = botM.ViewAmountOfMoney();
            int sumD = botD.ViewAmountOfMoney();
            int sumR = botR.ViewAmountOfMoney();
            botM.MoneyRecount(true, 1, 50_000);
            botD.MoneyRecount(true, 1, 50_000);
            botR.MoneyRecount(true, 1, 50_000);


            int expectedM = sumM + sumM / 1000;
            int expectedD = sumD + sumD / 20;
            int expectedRLeftBound = 50_001;
            int expectedRRightBound = 100_000;

            int actualM = botM.ViewAmountOfMoney();
            int actualD = botD.ViewAmountOfMoney();
            int actualR = botR.ViewAmountOfMoney();

            Assert.AreEqual(expectedM, actualM);
            Assert.AreEqual(expectedD, actualD);
            Assert.IsTrue(expectedRLeftBound <= actualR && actualR <= expectedRRightBound);

            botM.MoneyRecount(false, 1, 50_000);
            botD.MoneyRecount(false, 1, 50_000);
            botR.MoneyRecount(false, 1, 50_000);
            actualM = botM.ViewAmountOfMoney();
            actualD = botD.ViewAmountOfMoney();
            actualR = botR.ViewAmountOfMoney();

            Assert.AreEqual(sumM, actualM);
            Assert.AreEqual(sumD, actualD);
            Assert.AreEqual(sumR, actualR);
        }

        [TestMethod]
        public void TestStatus()
        {
            char[] betM = botM.ViewBet();
            char[] betD = botD.ViewBet();
            char[] betR = botR.ViewBet();
            int sumBetM = botM.ViewSumBet();
            int sumBetD = botD.ViewSumBet();
            int sumBetR = botR.ViewSumBet();

            string expectedM = "Information about Martingejl\n" +
                            "Amount of money a player has = 50000. " +
                            $"Last bet is {betM[0]}{betM[1]} with the amount of money {sumBetM}\n";
            string expectedD = "Information about D'Alamber\n" +
                            "Amount of money a player has = 50000. " +
                            $"Last bet is {betD[0]}{betD[1]} with the amount of money {sumBetD}\n";
            string expectedR = "Information about Rich beginner\n" +
                            "Amount of money a player has = 50000. " +
                            $"Last bet is {betR[0]}{betR[1]} with the amount of money {sumBetR}\n";

            string actualM = botM.ShowStatus();
            string actualD = botD.ShowStatus();
            string actualR = botR.ShowStatus();

            Assert.AreEqual(expectedM, actualM);
            Assert.AreEqual(expectedD, actualD);
            Assert.AreEqual(expectedR, actualR);
        }
    }
}
