using Casino_Case;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestTableInit()
        {
            Table testTable = new Table();

            Assert.AreEqual(0, testTable.wheel[0].number);
            Assert.AreEqual(-1, testTable.wheel[0].color);
            Assert.AreEqual(-1, testTable.wheel[0].dozen);
            Assert.AreEqual(-1, testTable.wheel[0].column);

            // first dozen test
            for (int i = 1; i <= 12; i++) 
            {
                Assert.AreEqual(i, testTable.wheel[i].number);
                Assert.AreEqual(1, testTable.wheel[i].dozen);
                Assert.AreEqual(0, testTable.wheel[i].bid);
            }
            Assert.AreEqual(0, testTable.wheel[1].color);
            Assert.AreEqual(1, testTable.wheel[1].column);

            Assert.AreEqual(1, testTable.wheel[2].color);
            Assert.AreEqual(2, testTable.wheel[2].column);

            Assert.AreEqual(0, testTable.wheel[3].color);
            Assert.AreEqual(3, testTable.wheel[3].column);

            Assert.AreEqual(1, testTable.wheel[4].color);
            Assert.AreEqual(1, testTable.wheel[4].column);

            Assert.AreEqual(0, testTable.wheel[5].color);
            Assert.AreEqual(2, testTable.wheel[5].column);

            Assert.AreEqual(1, testTable.wheel[6].color);
            Assert.AreEqual(3, testTable.wheel[6].column);

            Assert.AreEqual(0, testTable.wheel[7].color);
            Assert.AreEqual(1, testTable.wheel[7].column);

            Assert.AreEqual(1, testTable.wheel[8].color);
            Assert.AreEqual(2, testTable.wheel[8].column);

            Assert.AreEqual(0, testTable.wheel[9].color);
            Assert.AreEqual(3, testTable.wheel[9].column);

            Assert.AreEqual(1, testTable.wheel[10].color);
            Assert.AreEqual(1, testTable.wheel[10].column);

            Assert.AreEqual(1, testTable.wheel[11].color);
            Assert.AreEqual(2, testTable.wheel[11].column);

            Assert.AreEqual(0, testTable.wheel[12].color);
            Assert.AreEqual(3, testTable.wheel[12].column);

            // second dozen test
            for (int i = 13; i <= 24; i++)
            {
                Assert.AreEqual(i, testTable.wheel[i].number);
                Assert.AreEqual(2, testTable.wheel[i].dozen);
                Assert.AreEqual(0, testTable.wheel[i].bid);
            }
            Assert.AreEqual(1, testTable.wheel[13].color);
            Assert.AreEqual(1, testTable.wheel[13].column);

            Assert.AreEqual(0, testTable.wheel[14].color);
            Assert.AreEqual(2, testTable.wheel[14].column);

            Assert.AreEqual(1, testTable.wheel[15].color);
            Assert.AreEqual(3, testTable.wheel[15].column);

            Assert.AreEqual(0, testTable.wheel[16].color);
            Assert.AreEqual(1, testTable.wheel[16].column);

            Assert.AreEqual(1, testTable.wheel[17].color);
            Assert.AreEqual(2, testTable.wheel[17].column);

            Assert.AreEqual(0, testTable.wheel[18].color);
            Assert.AreEqual(3, testTable.wheel[18].column);

            Assert.AreEqual(0, testTable.wheel[19].color);
            Assert.AreEqual(1, testTable.wheel[19].column);

            Assert.AreEqual(1, testTable.wheel[20].color);
            Assert.AreEqual(2, testTable.wheel[20].column);

            Assert.AreEqual(0, testTable.wheel[21].color);
            Assert.AreEqual(3, testTable.wheel[21].column);

            Assert.AreEqual(1, testTable.wheel[22].color);
            Assert.AreEqual(1, testTable.wheel[22].column);

            Assert.AreEqual(0, testTable.wheel[23].color);
            Assert.AreEqual(2, testTable.wheel[23].column);

            Assert.AreEqual(1, testTable.wheel[24].color);
            Assert.AreEqual(3, testTable.wheel[24].column);

            // third dozen test
            for (int i = 25; i <= 36; i++)
            {
                Assert.AreEqual(i, testTable.wheel[i].number);
                Assert.AreEqual(3, testTable.wheel[i].dozen);
                Assert.AreEqual(0, testTable.wheel[i].bid);
            }

            Assert.AreEqual(0, testTable.wheel[25].color);
            Assert.AreEqual(1, testTable.wheel[25].column);

            Assert.AreEqual(1, testTable.wheel[26].color);
            Assert.AreEqual(2, testTable.wheel[26].column);

            Assert.AreEqual(0, testTable.wheel[27].color);
            Assert.AreEqual(3, testTable.wheel[27].column);

            Assert.AreEqual(1, testTable.wheel[28].color);
            Assert.AreEqual(1, testTable.wheel[28].column);

            Assert.AreEqual(1, testTable.wheel[29].color);
            Assert.AreEqual(2, testTable.wheel[29].column);

            Assert.AreEqual(0, testTable.wheel[30].color);
            Assert.AreEqual(3, testTable.wheel[30].column);

            Assert.AreEqual(1, testTable.wheel[31].color);
            Assert.AreEqual(1, testTable.wheel[31].column);

            Assert.AreEqual(0, testTable.wheel[32].color);
            Assert.AreEqual(2, testTable.wheel[32].column);

            Assert.AreEqual(1, testTable.wheel[33].color);
            Assert.AreEqual(3, testTable.wheel[33].column);

            Assert.AreEqual(0, testTable.wheel[34].color);
            Assert.AreEqual(1, testTable.wheel[34].column);

            Assert.AreEqual(1, testTable.wheel[35].color);
            Assert.AreEqual(2, testTable.wheel[35].column);

            Assert.AreEqual(0, testTable.wheel[36].color);
            Assert.AreEqual(3, testTable.wheel[36].column);

        }

        [TestMethod]
        public void BotTest()
        {
            Bots dalamberBotTest = new Bots();
            dalamberBotTest.setBalance(5000);
            dalamberBotTest.DalamberBot(400, 2);

            Bots martingaleBotTest = new Bots();
            martingaleBotTest.setBalance(5000);
            martingaleBotTest.MartingaleBot(400, 2);
            
            
            
        }
    }
}
