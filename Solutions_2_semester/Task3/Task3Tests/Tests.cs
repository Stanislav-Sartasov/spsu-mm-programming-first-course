using NUnit.Framework;
using Task3;
using System.Collections.Generic;
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
			Card card = new Card(Card.MinValue - 1, Card.Suits.Clubs);
			Assert.AreEqual(0, card.Value);
			Assert.AreEqual(Card.Suits.Clubs, card.Suit);
			Assert.AreEqual(0, card.Cost);

			card = new Card(Card.MaxValue + 1, Card.Suits.Clubs);
			Assert.AreEqual(0, card.Value);
			Assert.AreEqual(Card.Suits.Clubs, card.Suit);
			Assert.AreEqual(0, card.Cost);

			for (int i = Card.MinValue; i <= Card.MaxValue; i++)
				for (int j = 0; j < Card.SuitsCount; j++)
				{
					card = new Card(i, (Card.Suits)j);
					Assert.AreEqual(i, card.Value);
					if (i < 10)
						Assert.AreEqual(i, card.Cost);
					else
						Assert.AreEqual(0, card.Cost);
					Assert.AreEqual(j, (int)card.Suit);
				}
		}
		[Test]
		public void DeckTest()
		{
			Deck deck;
			List<Card> cardList;

			for (int g = -1; g <= 10; g++)
			{
				deck = new Deck(g);
				cardList = new List<Card>();

				while (deck.ActualLength > 0)
					cardList.Add(deck.Next());

				for (int i = Card.MinValue; i<= Card.MaxValue; i++)
					for (int j = 0; j < Card.SuitsCount; j++)
						for (int c = 0; c < Math.Max(g, 1); c++)
						{
							int index = cardList.FindIndex((x) => { return x.Value == i && x.Suit == (Card.Suits)j; });
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
			Assert.AreEqual("firstPlayer", firstPlayer.PlayerName);
			Assert.AreEqual(5000, firstPlayer.Bank);
			Assert.AreEqual(true, firstPlayer.Active);

			Player secondPlayer = game.AddPlayer("secondPlayer");
			Assert.AreNotEqual(null, secondPlayer);
			Assert.AreEqual("secondPlayer", secondPlayer.PlayerName);
			Assert.AreEqual(5000, secondPlayer.Bank);
			Assert.AreEqual(true, secondPlayer.Active);

			Assert.AreEqual(null, game.AddPlayer("firstPlayer"));

			Assert.AreEqual(true, game.KickPlayer(firstPlayer));
			Assert.AreEqual(false, firstPlayer.Active);

			firstPlayer = game.AddPlayer("firstPlayer");
			Assert.AreNotEqual(null, firstPlayer);

			secondPlayer.MakeBet(500, Field.Player);

			Assert.AreEqual(true, game.KickPlayer(secondPlayer));
			Assert.AreEqual(5000, secondPlayer.Bank);
			Assert.AreEqual(false, secondPlayer.Active);
		}

		[Test]
		public void MakeBetTest()
		{
			GameManager game = new GameManager(5000, 0, null, 0);
			Player player = game.AddPlayer("player");

			Assert.AreEqual(5000, player.Bank);

			Assert.AreEqual(true, player.MakeBet(1000, Field.Player));
			Assert.AreEqual(4000, player.Bank);
			Assert.AreEqual(false, player.MakeBet(1000, Field.Player));
			Assert.AreEqual(4000, player.Bank);

			game = new GameManager(GameTestsCount * 10 + 1, 0, null, 0);
			player = game.AddPlayer("player");

			for (int i = 0; i < GameTestsCount; i++)
			{
				int b = player.Bank;
				player.MakeBet(10, (Field)(i % 3));

				Assert.AreEqual(b - 10, player.Bank);

				GameLog log = game.ProduceGame();

				Assert.AreEqual(10, log.PlayerBetWas[0]);

				if ((int)log.WinField == i % 3)
					Assert.IsTrue(player.Bank > b);
				else
					Assert.IsTrue(player.Bank == b - 10);

				Assert.AreEqual(i % 3, (int)log.PlayerBetFieldWas[0]);
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

				firstPlayer.MakeBet(1000, Field.Player);
				secondPlayer.MakeBet(1000, Field.Bank);
				thirdPlayer.MakeBet(1000, Field.Draw);

				GameLog log = game.ProduceGame();

				Assert.AreEqual(3, log.Count);

				switch (log.WinField)
				{
					case Field.Player:
						Assert.IsTrue(log.BankScore < log.PlayerScore);
						player = true;
						break;

					case Field.Bank:
						Assert.IsTrue(log.BankScore > log.PlayerScore);
						bank = true;
						break;

					case Field.Draw:
						Assert.IsTrue(log.BankScore == log.PlayerScore);
						draw = true;
						break;
				}				

				Assert.AreEqual(log.WinField == Field.Player, firstPlayer.Bank > 5000);
				Assert.AreEqual(log.WinField != Field.Player, firstPlayer.Bank < 5000);
				Assert.AreEqual(log.WinField == Field.Bank, secondPlayer.Bank > 5000);
				Assert.AreEqual(log.WinField != Field.Bank, secondPlayer.Bank < 5000);
				Assert.AreEqual(log.WinField == Field.Draw, thirdPlayer.Bank > 5000);
				Assert.AreEqual(log.WinField != Field.Draw, thirdPlayer.Bank < 5000);
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

					if (log.WinField == log.PlayerBetFieldWas[0])
					{
						martinFinish = true;
						Assert.IsTrue(martin.CanBeSettingsChanged(), "5");
					}

					if (log.WinField == log.PlayerBetFieldWas[1])
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

					for (int g = 0; g < 400 && bot.ConnectedPlayer.Active; g++)
					{
						bot.MakeBet();
						game.ProduceGame();

						if (j % 2 == 0)
						{
							if (peakSC < bot.Bank)
								peakSC = bot.Bank;
						}
						else
						{
							if (peak < bot.Bank)
								peak = bot.Bank;
						}
					}

					if (j % 2 == 0)
						middleSC += (double)bot.Bank / GameTestsCount;
					else
						middle += (double)bot.Bank / GameTestsCount;
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

					for (int g = 0; g < 400 && bot.ConnectedPlayer.Active; g++)
					{
						bot.MakeBet();
						game.ProduceGame();

						if (j % 2 == 0)
						{
							if (peakSC < bot.Bank)
								peakSC = bot.Bank;
						}
						else
						{
							if (peak < bot.Bank)
								peak = bot.Bank;
						}
					}

					if (j % 2 == 0)
						middleSC += (double)bot.Bank / GameTestsCount;
					else
						middle += (double)bot.Bank / GameTestsCount;
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

				for (int g = 0; g < 400 && bot.ConnectedPlayer.Active; g++)
				{
					bot.MakeBet();
					game.ProduceGame();

					if (peak < bot.Bank)
						peak = bot.Bank;
				}

				middle += (double)bot.Bank / GameTestsCount;
			}

			Console.WriteLine($"{middle}$ left at 400 iteration with start budget {startBudget}$");
			Console.WriteLine($"peak value during the game was {peak}$\n");
			Console.WriteLine($"average result from {GameTestsCount} games");
		}
	}
}