using Microsoft.VisualStudio.TestTools.UnitTesting;
using Roulette.Casino;
using Roulette.People;
using System.Collections.Generic;
using Unity;
using Unity.Injection;

namespace IoC
{
    [TestClass]
    public class Program
    {
        [TestMethod]
        public void TestGame()
        {
            IUnityContainer container = new UnityContainer();
            container.RegisterType(typeof(Table), new InjectionConstructor());
            container.RegisterType(typeof(Bot), "Martingejl", new InjectionConstructor(1));
            container.RegisterType(typeof(Bot), "D'Alamber", new InjectionConstructor(2));
            container.RegisterType(typeof(Bot), "Beginner", new InjectionConstructor(3));

            Table table = (Table)container.Resolve(typeof(Table));
            List<AbstractPlayer> bots = new List<AbstractPlayer>() { (Bot)container.Resolve(typeof(Bot), "Martingejl"),
                    (Bot)container.Resolve(typeof(Bot), "D'Alamber"), (Bot)container.Resolve(typeof(Bot), "Beginner") };

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
