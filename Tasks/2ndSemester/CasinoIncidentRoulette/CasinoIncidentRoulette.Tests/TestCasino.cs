using System;
using CasinoIncidentRoulette.Roulette;
using CasinoIncidentRoulette.Bots;
using CasinoIncidentRoulette.Player;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CasinoIncidentRoulette.UnitTests
{
    [TestClass]
    public class TestCasino
    {
        [TestMethod]
        public void CasinoIncidentRouletteTest()
        {
            Table table1 = new Table();
            table1.CreateTable();
            Cell exodus;

            MartingaleBot martingaleBot = new MartingaleBot();
            MakarovBot makarovBot = new MakarovBot();

            for (int i = 0; i < 400; i++)
            {
                if (martingaleBot.CanIBet())
                    martingaleBot.PlayerBet = martingaleBot.Bet(table1);
                if (makarovBot.CanIBet())
                    makarovBot.PlayerBet = makarovBot.Bet(table1);

                exodus = table1.Roll();

                if (martingaleBot.CanIBet())
                    martingaleBot.CheckResult(martingaleBot.PlayerBet, exodus);
                if (makarovBot.CanIBet())
                    makarovBot.CheckResult(makarovBot.PlayerBet, exodus);
            }

            if (martingaleBot.CanIBet())
                Assert.IsTrue(martingaleBot.GetMoney() >= 1 << martingaleBot.LoseStreak);
            else
                Assert.IsTrue(martingaleBot.GetMoney() < 1 << martingaleBot.LoseStreak);

            if (makarovBot.CanIBet())
                Assert.IsTrue(makarovBot.GetMoney() > 0);
            else
                Assert.AreEqual(makarovBot.GetMoney(), 0);
        }
    }
}
