using Microsoft.VisualStudio.TestTools.UnitTesting;
using AbstractPlayer;
using System;

namespace Task2Roulette.Tests
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestRoulette()
        {

            int averageOliverMoney = 0;
            int averageStepanMoney = 0;
            for (int i = 0; i < 50; i++)
            {
                BotOliver botOliver = new BotOliver();
                BotStepan botStepan = new BotStepan();
                botOliver.Bet();
                botStepan.Bet();
                averageOliverMoney += botOliver.wallet;
                averageStepanMoney += botStepan.wallet;
            }
            Console.WriteLine(averageOliverMoney / 50);
            Console.WriteLine(averageStepanMoney / 50);

        }
    }
}
