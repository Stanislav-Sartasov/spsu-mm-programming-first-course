using Microsoft.VisualStudio.TestTools.UnitTesting;
using CasinoIncidentRoulette.Roulette;
using CasinoIncidentRoulette.Bots;
using CasinoIncidentRoulette.Player;
using Unity;
using Unity.Injection;

namespace IoC
{
    [TestClass]
    public class Test
    {
        [TestMethod]
        public void TestIoC()
        {
            IUnityContainer unityContainer = new UnityContainer();
            unityContainer.RegisterType<Table>();
            unityContainer.RegisterType<AbstractPlayer, MartingaleBot>();
            unityContainer.RegisterType<AbstractPlayer, MakarovBot>();

            Table table = unityContainer.Resolve<Table>();
            MartingaleBot martingaleBot = unityContainer.Resolve<MartingaleBot>();
            MakarovBot makarovBot = unityContainer.Resolve<MakarovBot>();

            martingaleBot.PlayerBet = martingaleBot.Bet(table);
            makarovBot.PlayerBet = makarovBot.Bet(table);

            table.Roll();

            martingaleBot.CheckResult(martingaleBot.PlayerBet, table.LastExodus);
            makarovBot.CheckResult(makarovBot.PlayerBet, table.LastExodus);

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
