using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using Task3;

namespace Task3Tests
{
    public class MainTests
    {
        const int CountOfGames = 1000;
        [Test]
        public void AddAndCickTest()
        {
            Player firstPlayer = new Player();
            Player secondPlayer = new Player();
            GameMaster game = new GameMaster(5000);

            Assert.AreEqual(0, game.AddPlayer(firstPlayer), "1");
            Assert.AreEqual(0, game.AddPlayer(secondPlayer), "2");
            Assert.AreEqual(-1, game.AddPlayer(firstPlayer), "3");

            Assert.AreEqual(0, game.CickPlayer(firstPlayer), "4");
            Assert.AreEqual(0, game.AddPlayer(firstPlayer), "5");

            secondPlayer.MakeBate();

            Assert.AreEqual(-1, game.CickPlayer(secondPlayer), "6");
            Assert.AreEqual(0, game.CickPlayer(firstPlayer), "7");
        }
        [Test]
        public void MakeBateTest()
        {
            Player firstPlayer = new Player();
            GameMaster game = new GameMaster(5000);

            Assert.AreEqual(0, firstPlayer.Budget, "1");
            Assert.AreEqual(-1, firstPlayer.MakeBate(1000, 0), "2");

            game.AddPlayer(firstPlayer);

            Assert.AreEqual(5000, firstPlayer.Budget, "3");
            Assert.AreEqual(0, firstPlayer.MakeBate(1000, 0), "4");
            Assert.AreEqual(4000, firstPlayer.Budget, "5");
            firstPlayer.MakeBate(1000, 0);
            Assert.AreEqual(4000, firstPlayer.Budget, "6");      //already budgeted => can't make new budget

            firstPlayer = new Player();
            game = new GameMaster(100000);
            game.AddPlayer(firstPlayer);
            int b;

            for (int i = 0; i < CountOfGames; i++)
            {
                b = firstPlayer.Budget;
                firstPlayer.MakeBate(10, i % 3);

                Assert.AreEqual(b - 10, firstPlayer.Budget);

                game.StartGame();

                Assert.AreEqual(10, game.GetLog().bate[0], "7");
                Assert.AreEqual(i % 3, game.GetLog().bateType[0], "8");
            }
        }
        [Test]
        public void GameTest()
        {
            for (int j = 0; j < CountOfGames; j++)
            {
                Player firstPlayer = new Player();
                Player secondPlayer = new Player();
                Player thirdPlayer = new Player();
                GameMaster game = new GameMaster(5000);

                game.AddPlayer(firstPlayer);
                game.AddPlayer(secondPlayer);
                game.AddPlayer(thirdPlayer);

                Assert.AreEqual(-1, game.StartGame(), "1");

                firstPlayer.MakeBate(1000, 0);
                secondPlayer.MakeBate(1000, 1);
                thirdPlayer.MakeBate(1000, 2);

                Assert.AreEqual(0, game.StartGame(), "2");

                GameMaster.Log gameLog = game.GetLog();

                Assert.AreEqual(3, gameLog.playerCount, "3");

                Assert.AreEqual(gameLog.winType == 0, gameLog.bankScoreAnc < gameLog.playScoreAnc, "4");
                Assert.AreEqual(gameLog.winType == 1, gameLog.bankScoreAnc > gameLog.playScoreAnc, "5");
                Assert.AreEqual(gameLog.winType == 2, gameLog.bankScoreAnc == gameLog.playScoreAnc, "6");

                Assert.AreEqual(gameLog.winType == 0, firstPlayer.Budget > 5000, "7");
                Assert.AreEqual(gameLog.winType == 1, secondPlayer.Budget > 5000, "8");
                Assert.AreEqual(gameLog.winType == 2, thirdPlayer.Budget > 5000, "9");
            }
        }
        [Test]
        public void SettingChangeTest()
        {
            for (int j = 0; j < CountOfGames; j++)
            {
                MartingaleBot martin = new MartingaleBot();
                Assert.AreEqual(true, martin.CanBeSettingsChanged(), "1");
                martin.ChangeTypeSettings(0, 1, 3, 0.9);
                martin.ChangeTypeSettings(1, 0, 0, 0);
                martin.ChangeTypeSettings(2, 0, 0, 0);

                GoldenRatioBot golden = new GoldenRatioBot();
                Assert.AreEqual(true, golden.CanBeSettingsChanged(), "2");
                golden.ChangeCount(3);

                GameMaster game = new GameMaster(5000);
                game.AddPlayer(martin);
                game.AddPlayer(golden);

                Assert.AreEqual(true, martin.CanBeSettingsChanged(), "3");
                Assert.AreEqual(true, golden.CanBeSettingsChanged(), "4");

                martin.MakeBate();
                golden.MakeBate();

                Assert.AreEqual(false, martin.CanBeSettingsChanged(), "5");
                Assert.AreEqual(false, golden.CanBeSettingsChanged(), "6");

                bool martinFinish = false;
                bool goldenFinish = false;

                for (int i = 0; i < 3; i++)
                {
                    martin.MakeBate();
                    golden.MakeBate();
                    game.StartGame();

                    if (game.GetLog().winType == game.GetLog().bateType[0])
                    {
                        martinFinish = true;
                        Assert.AreEqual(true, martin.CanBeSettingsChanged(), "8");
                    }

                    if (game.GetLog().winType == game.GetLog().bateType[1])
                    {
                        goldenFinish = true;
                        Assert.AreEqual(true, golden.CanBeSettingsChanged(), "7");
                    }
                }

                if (!martinFinish)
                    Assert.AreEqual(true, martin.CanBeSettingsChanged(), "9");
                if (!goldenFinish)
                    Assert.AreEqual(true, golden.CanBeSettingsChanged(), "10");
            }
        }
        [Test]
        public void MartingaleTest()
        {
            double middle = 0;
            int startBudget = 100000;

            for (int i = 0; i < CountOfGames; i++)
            {
                MartingaleBot bot = new MartingaleBot();        //standard settings for highest average peak value
                bot.ChangeTypeSettings(0, 0.5, 8, 0.1);       //change them to increase the duration of the game
                bot.ChangeTypeSettings(1, 0.5, 8, 0.1);       //but not exaggerate
                bot.ChangeTypeSettings(2, -1, 0, 0);

                GameMaster game = new GameMaster(startBudget);
                game.AddPlayer(bot);

                for (int j = 0; j < 400 && bot.Budget > 0; j++)
                {
                    bot.MakeBate();
                    game.StartGame();
                }

                middle += (double)bot.Budget / CountOfGames;
            }

            Console.WriteLine($"{middle}$ left at 400 iteration with start budget {startBudget}$");
        }
        [Test]
        public void GoldenRatioTest()
        {
            double middle = 0;
            int startBudget = 100000;

            for (int i = 0; i < CountOfGames; i++)
            {
                GoldenRatioBot bot = new GoldenRatioBot();
                bot.ChangeCount(8);

                GameMaster game = new GameMaster(startBudget);
                game.AddPlayer(bot);

                for (int j = 0; j < 400 && bot.Budget > 0; j++)
                {
                    bot.MakeBate();
                    game.StartGame();
                }

                middle += (double)bot.Budget / CountOfGames;
            }

            Console.WriteLine($"{middle}$ left at 400 iteration with start budget {startBudget}$");
        }
    }
}