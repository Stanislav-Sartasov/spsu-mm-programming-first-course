using Casino_Case;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class UnitTest
    { 
        [TestMethod]
        public void BotTest()
        {
            Bots dalamberBotTest = new Bots();
            dalamberBotTest.SetBalance(5000);
            dalamberBotTest.DalamberBot(400, 2);

            Bots martingaleBotTest = new Bots();
            martingaleBotTest.SetBalance(5000);
            martingaleBotTest.MartingaleBot(400, 2);

        }
    }
}
