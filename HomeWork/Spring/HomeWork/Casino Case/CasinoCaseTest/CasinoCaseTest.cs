using Casino_Case;
using Casino_Case.Bot;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CasinoCaseTest
{
    [TestClass]
    public class CasinoCaseTest
    {
        [TestMethod]
        public void BotsTest()
        {
            DalamberBot DalamberTest = new DalamberBot();
            Bet.balance = 5000;
            DalamberTest.Action(400, 2);

            MartingaleBot MartingaleTest = new MartingaleBot();
            Bet.balance = 5000;
            MartingaleTest.Action(400, 2);
        }
    }
}
