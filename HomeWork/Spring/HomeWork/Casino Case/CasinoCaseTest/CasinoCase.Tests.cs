using Casino_Case;
using Casino_Case.Bot;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Casino_Case.Logic;

namespace CasinoCaseTest
{
    [TestClass]
    public class CasinoCaseTest
    {
        [TestMethod]
        public void DalamberBotTest()
        {
            DalamberBot DalamberTest = new DalamberBot();
            BetInfo.balance = 5000;
            DalamberTest.Action(400, 2);
        }
        [TestMethod]
        public void MartingaleBotTest()
        {
            MartingaleBot MartingaleTest = new MartingaleBot();
            BetInfo.balance = 5000;
            MartingaleTest.Action(400, 2);
        }
    }
}
