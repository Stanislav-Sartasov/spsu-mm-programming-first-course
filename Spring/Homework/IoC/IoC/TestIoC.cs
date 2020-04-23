using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity;
using Unity.Injection;
using Blackjack;
using System.Collections.Generic;
using System;

namespace IoC
{
    [TestClass]
    public class TestIoC
    {
        [TestMethod]
        public void TestRunIoC()
        {
            var container = new UnityContainer();
            container.RegisterType<Deck>();
            container.RegisterType<Player, RandomBot>("RandomBot",
                new InjectionConstructor(5000, "RandomBot", PlayerStatus.Bot));
            container.RegisterType<Player, BasicStrategyBot>("BasicStrategyBot",
                new InjectionConstructor(5000, "BasicStrategyBot", PlayerStatus.Bot));
            container.RegisterType<Croupier>(new InjectionConstructor(PlayerStatus.Bot));
            container.RegisterType<IList<Player>, Player[]>();
            container.RegisterType<Game>();

            var game = container.Resolve<Game>();

            game.StartGame();

            Console.WriteLine($"Scores:");
            Console.WriteLine($"Random_Bot: {game.Players[0].Chips}\nBSBot: {game.Players[1].Chips}");
        }
    }
}
