using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity;
using Casino_Case;
using Casino_Case.Bot;
using Casino_Case.Logic;

namespace IoC.Tests
{
    [TestClass]
    public class IoCTests
    {
        [TestMethod]
        public void DalamberBotTest()
        {
            var container = new UnityContainer();
            container.RegisterType<DalamberBot>();
            var DalamberTest = container.Resolve<DalamberBot>();
            BetInfo.balance = 50000;
            DalamberTest.Action(400, 2);
        }
        [TestMethod]
        public void MartingaleBotTest()
        {
            var container = new UnityContainer();
            container.RegisterType<DalamberBot>();
            var MartingaleTest = container.Resolve<DalamberBot>();
            BetInfo.balance = 50000;
            MartingaleTest.Action(400, 2);
        }
    }
}
