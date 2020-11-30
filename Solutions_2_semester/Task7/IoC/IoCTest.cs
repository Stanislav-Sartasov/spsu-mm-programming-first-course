using NUnit.Framework;
using Unity;
using Unity.Injection;
using Task3;
using System.Collections.Generic;
using System;

namespace IoC
{
	public class Tests
	{
		[Test]
		public void LoCTestGame()
		{
			IUnityContainer container = new UnityContainer();
			container.RegisterType(typeof(GameManager), new InjectionConstructor(10000, 4, null, 0));
			container.RegisterType(typeof(MartingaleBot), new InjectionConstructor());
			container.RegisterType(typeof(RandomBot), new InjectionConstructor());
			container.RegisterType(typeof(GoldenRatioBot), new InjectionConstructor());

			GameManager game = (GameManager)container.Resolve(typeof(GameManager));
			List<ABot> bots = new List<ABot>()
			{
				(ABot)container.Resolve(typeof(MartingaleBot)),
				(ABot)container.Resolve(typeof(RandomBot)),
				(ABot)container.Resolve(typeof(GoldenRatioBot))
			};
			List<GameLog> logs = new List<GameLog>();

			for (int i = 0; i < bots.Count; i++)
			{
				bots[i].Connect(game.AddPlayer("Bot_" + i.ToString()));
				bots[i].AutoLeaving = true;
				Assert.AreEqual(10000, bots[i].Bank);
			}

			bool end;
			do
			{
				end = true;
				foreach (ABot bot in bots)
				{
					if (!bot.ConnectedPlayer.Active)
						continue;
					bot.MakeBet();
					end = false;
				}
				logs.Add(game.ProduceGame());
			}
			while (!end);

			foreach (GameLog log in logs)
			{
				if (log == null)
					continue;

				for (int i = 0; i < 20; i++)
					Console.Write("_");
				Console.Write("\nactive players: ");
				for (int i = 0; i < log.Count; i++)
					Console.Write($"{log.PlayerName[i]} ");
				Console.WriteLine();
				for (int i = 0; i < log.Count; i++)
				{
					Console.WriteLine($"{log.PlayerName[i]} bank: {log.PlayerBankWas[i] + log.PlayerBetWas[i]}");
					Console.WriteLine($"{log.PlayerName[i]} bet:  {log.PlayerBetWas[i]} on {log.PlayerBetFieldWas[i]}");
				}
				Console.Write("card pull: Player< ");
				for (int i = 0; i < 3; i++)
					if (log.CardPull[i] != default)
						Console.Write($"{log.CardPull[i].Suit}_{log.CardPull[i].Value} ");
				Console.Write("> Bank< ");
				for (int i = 3; i < 6; i++)
					if (log.CardPull[i] != default)
						Console.Write($"{log.CardPull[i].Suit}_{log.CardPull[i].Value} ");
				Console.WriteLine($">\nwin field: {log.WinField}");
				for (int i = 0; i < log.Count; i++)
					Console.WriteLine($"{log.PlayerName[i]} bank become: {log.PlayerBankBecome[i]}");
				Console.Write("\n\n\n");
			}
			Assert.Pass();
		}
	}
}