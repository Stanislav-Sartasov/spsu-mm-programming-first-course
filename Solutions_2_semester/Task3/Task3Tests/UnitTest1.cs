using NUnit.Framework;
using Task3;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System;
using NUnit.Framework.Internal;

namespace Task3Tests
{
    public class Tests
    {
        const int GameTestsCount = 10000;

        [Test]
        public void CardTest()
        {
            Card card = new Card(Card.MinValue - 1, Card.Suits.clubs);
            Assert.AreEqual(0, card.value);
            Assert.AreEqual(Card.Suits.clubs, card.suit);
            Assert.AreEqual(0, card.cost);

            card = new Card(Card.MaxValue + 1, Card.Suits.clubs);
            Assert.AreEqual(0, card.value);
            Assert.AreEqual(Card.Suits.clubs, card.suit);
            Assert.AreEqual(0, card.cost);

            for (int i = Card.MinValue; i <= Card.MaxValue; i++)
                for (int j = 0; j < Card.SuitsCount; j++)
                {
                    card = new Card(i, (Card.Suits)j);
                    Assert.AreEqual(i, card.value);
                    if (i < 10)
                        Assert.AreEqual(i, card.cost);
                    else
                        Assert.AreEqual(0, card.cost);
                    Assert.AreEqual(j, (int)card.suit);
                }
        }
        [Test]
        public void DeckTest()
        {
            Deck deck; ;
            List<Card> cardList;

            for (int g = -1; g <= 10; g++)
            {
                deck = new Deck(g);
                cardList = new List<Card>();

                while (deck.actualLength > 0)
                    cardList.Add(deck.Next());

                for (int i = Card.MinValue; i<= Card.MaxValue; i++)
                    for (int j = 0; j < Card.SuitsCount; j++)
                        for (int c = 0; c < Math.Max(g, 1); c++)
                        {
                            int index = cardList.FindIndex((x) => { return x.value == i && x.suit == (Card.Suits)j; });
                            Assert.AreNotEqual(-1, index);
                            if (index > -1)
                                cardList.RemoveAt(index);
                        }

                Assert.AreEqual(0, cardList.Count);
            }
        }

        [Test]
        public void AddAndKickTest()
        {
            GameManager game = new GameManager(5000, 0, null, 0);

            Player firstPlayer = game.AddPlayer("firstPlayer");
            Assert.AreNotEqual(null, firstPlayer);
            Assert.AreEqual("firstPlayer", firstPlayer.playerName);
            Assert.AreEqual(5000, firstPlayer.bank);
            Assert.AreEqual(true, firstPlayer.active);

            Player secondPlayer = game.AddPlayer("secondPlayer");
            Assert.AreNotEqual(null, secondPlayer);
            Assert.AreEqual("secondPlayer", secondPlayer.playerName);
            Assert.AreEqual(5000, secondPlayer.bank);
            Assert.AreEqual(true, secondPlayer.active);

            Assert.AreEqual(null, game.AddPlayer("firstPlayer"));

            Assert.AreEqual(true, game.KickPlayer(firstPlayer));
            Assert.AreEqual(false, firstPlayer.active);

            firstPlayer = game.AddPlayer("firstPlayer");
            Assert.AreNotEqual(null, firstPlayer);

            secondPlayer.MakeBet(500, Field.player);

            Assert.AreEqual(true, game.KickPlayer(secondPlayer));
            Assert.AreEqual(5000, secondPlayer.bank);
            Assert.AreEqual(false, secondPlayer.active);
        }

        [Test]
        public void MakeBateTest()
        {
            GameManager game = new GameManager(5000, 0, null, 0);
            Player player = game.AddPlayer("player");

            Assert.AreEqual(5000, player.bank);

            Assert.AreEqual(true, player.MakeBet(1000, Field.player));
            Assert.AreEqual(4000, player.bank);
            Assert.AreEqual(false, player.MakeBet(1000, Field.player));
            Assert.AreEqual(4000, player.bank);

            game = new GameManager(GameTestsCount * 10 + 1, 0, null, 0);
            player = game.AddPlayer("player");

            for (int i = 0; i < GameTestsCount; i++)
            {
                int b = player.bank;
                player.MakeBet(10, (Field)(i % 3));

                Assert.AreEqual(b - 10, player.bank);

                GameLog log = game.ProduceGame();

                Assert.AreEqual(10, log.playerBetWas[0]);

                if ((int)log.winField == i % 3)
                    Assert.IsTrue(player.bank > b);
                else
                    Assert.IsTrue(player.bank == b - 10);

                Assert.AreEqual(i % 3, (int)log.playerBetFieldWas[0]);
            }
        }

        [Test]
        public void GameTest()
        {
            bool player = false;
            bool bank = false;
            bool draw = false;

            for (int j = 0; j < GameTestsCount || !player || !bank || !draw; j++)
            {
                GameManager game = new GameManager(5000, 0, null, 0);
                Player firstPlayer = game.AddPlayer("first");
                Player secondPlayer = game.AddPlayer("second");
                Player thirdPlayer = game.AddPlayer("third");

                firstPlayer.MakeBet(1000, Field.player);
                secondPlayer.MakeBet(1000, Field.bank);
                thirdPlayer.MakeBet(1000, Field.draw);

                GameLog log = game.ProduceGame();

                Assert.AreEqual(3, log.count);

                switch (log.winField)
                {
                    case Field.player:
                        Assert.IsTrue(log.bankScore < log.playerScore);
                        player = true;
                        break;

                    case Field.bank:
                        Assert.IsTrue(log.bankScore > log.playerScore);
                        bank = true;
                        break;

                    case Field.draw:
                        Assert.IsTrue(log.bankScore == log.playerScore);
                        draw = true;
                        break;
                }                

                Assert.AreEqual(log.winField == Field.player, firstPlayer.bank > 5000);
                Assert.AreEqual(log.winField != Field.player, firstPlayer.bank < 5000);
                Assert.AreEqual(log.winField == Field.bank, secondPlayer.bank > 5000);
                Assert.AreEqual(log.winField != Field.bank, secondPlayer.bank < 5000);
                Assert.AreEqual(log.winField == Field.draw, thirdPlayer.bank > 5000);
                Assert.AreEqual(log.winField != Field.draw, thirdPlayer.bank < 5000);
            }
        }

        [Test]
        public void SettingChangeTest()
        {
            for (int j = 0; j < GameTestsCount; j++)
            {
                GameManager game = new GameManager(5000, 0, null, 0);

                MartingaleBot martin = new MartingaleBot();
                martin.Connect(game.AddPlayer("Martin"));
                martin.ChangeTypeSettings(0, 1, 3, 0.9);
                martin.ChangeTypeSettings(1, 0, 0, 0);
                martin.ChangeTypeSettings(2, 0, 0, 0);

                GoldenRatioBot golden = new GoldenRatioBot();
                golden.Connect(game.AddPlayer("goldy"));
                golden.ChangeCount(3);

                Assert.IsTrue(martin.CanBeSettingsChanged(), "1");
                Assert.IsTrue(golden.CanBeSettingsChanged(), "2");

                martin.MakeBet();
                golden.MakeBet();

                Assert.IsFalse(martin.CanBeSettingsChanged(), "3");
                Assert.IsFalse(golden.CanBeSettingsChanged(), "4");

                bool martinFinish = false;
                bool goldenFinish = false;

                for (int i = 0; i < 3; i++)
                {
                    martin.MakeBet();
                    golden.MakeBet();
                    GameLog log = game.ProduceGame();

                    if (log.winField == log.playerBetFieldWas[0])
                    {
                        martinFinish = true;
                        Assert.IsTrue(martin.CanBeSettingsChanged(), "5");
                    }

                    if (log.winField == log.playerBetFieldWas[1])
                    {
                        goldenFinish = true;
                        Assert.IsTrue(golden.CanBeSettingsChanged(), "6");
                    }
                }

                if (!martinFinish)
                    Assert.IsTrue(martin.CanBeSettingsChanged(), "7");
                if (!goldenFinish)
                    Assert.IsTrue(golden.CanBeSettingsChanged(), "8");
            }
        }

        [Test]
        public void MartingaleBotTest()
        {
            double middle = 0;
            double middleSC = 0;
            int peak = 0;
            int peakSC = 0;
            int startBudget = 10000;

            for (int i = 0; i < GameTestsCount; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    MartingaleBot bot = new MartingaleBot();

                    if (j % 2 == 0)
                    {
                        bot.ChangeTypeSettings(0, 0.5, 8, 0.1);
                        bot.ChangeTypeSettings(1, 0.5, 8, 0.1);
                        bot.ChangeTypeSettings(2, -1, 0, 0);
                    }

                    GameManager game = new GameManager(startBudget, 0, null, 0);
                    bot.Connect(game.AddPlayer("Martin"));

                    for (int g = 0; g < 400 && bot.connectedPlayer.active; g++)
                    {
                        bot.MakeBet();
                        game.ProduceGame();

                        if (j % 2 == 0)
                        {
                            if (peakSC < bot.bank)
                                peakSC = bot.bank;
                        }
                        else
                        {
                            if (peak < bot.bank)
                                peak = bot.bank;
                        }
                    }

                    if (j % 2 == 0)
                        middleSC += (double)bot.bank / GameTestsCount;
                    else
                        middle += (double)bot.bank / GameTestsCount;
                }
            }

            Console.WriteLine($"{middle}$ left at 400 iteration with start budget {startBudget}$ using standard settings");
            Console.WriteLine($"peak value during the game was {peak}$\n");
            Console.WriteLine($"{middleSC}$ left at 400 iteration with start budget {startBudget}$ using longplay settings");
            Console.WriteLine($"peak value during the game was {peakSC}$\n");
            Console.WriteLine($"average result from {GameTestsCount} games");
        }

        [Test]
        public void GoldenRatioBotTest()
        {
            double middle = 0;
            double middleSC = 0;
            int peak = 0;
            int peakSC = 0;
            int startBudget = 10000;

            for (int i = 0; i < GameTestsCount; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    GoldenRatioBot bot = new GoldenRatioBot();

                    if (j % 2 == 0)
                        bot.ChangeCount(8);

                    GameManager game = new GameManager(startBudget, 0, null, 0);
                    bot.Connect(game.AddPlayer("Martin"));

                    for (int g = 0; g < 400 && bot.connectedPlayer.active; g++)
                    {
                        bot.MakeBet();
                        game.ProduceGame();

                        if (j % 2 == 0)
                        {
                            if (peakSC < bot.bank)
                                peakSC = bot.bank;
                        }
                        else
                        {
                            if (peak < bot.bank)
                                peak = bot.bank;
                        }
                    }

                    if (j % 2 == 0)
                        middleSC += (double)bot.bank / GameTestsCount;
                    else
                        middle += (double)bot.bank / GameTestsCount;
                }
            }

            Console.WriteLine($"{middle}$ left at 400 iteration with start budget {startBudget}$ using standard settings");
            Console.WriteLine($"peak value during the game was {peak}$\n");
            Console.WriteLine($"{middleSC}$ left at 400 iteration with start budget {startBudget}$ using longplay settings");
            Console.WriteLine($"peak value during the game was {peakSC}$\n");
            Console.WriteLine($"average result from {GameTestsCount} games");
        }

        [Test]
        public void RandomBotTest()
        {
            double middle = 0;
            int peak = 0;
            int startBudget = 10000;

            for (int i = 0; i < GameTestsCount; i++)
            {
                RandomBot bot = new RandomBot();

                GameManager game = new GameManager(startBudget, 0, null, 0);
                bot.Connect(game.AddPlayer("Martin"));

                for (int g = 0; g < 400 && bot.connectedPlayer.active; g++)
                {
                    bot.MakeBet();
                    game.ProduceGame();

                    if (peak < bot.bank)
                        peak = bot.bank;
                }

                middle += (double)bot.bank / GameTestsCount;
            }

            Console.WriteLine($"{middle}$ left at 400 iteration with start budget {startBudget}$");
            Console.WriteLine($"peak value during the game was {peak}$\n");
            Console.WriteLine($"average result from {GameTestsCount} games");
        }
    }
}