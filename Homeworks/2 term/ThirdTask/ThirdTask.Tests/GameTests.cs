using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using GameDescription;

namespace ThirdTask.Tests
{
	[TestClass]
	public class GameTests
	{
		[TestMethod]
		public void FirstBotTestMethod()
		{
			var bot = new FirstBotPlayer();
			var game = new Game();

			game.Start(bot, 400);

			ShowCash(bot);
		}

		[TestMethod]
		public void SecondBotTestMethod()
		{
			var bot = new SecondBotPlayer();
			var game = new Game();

			game.Start(bot, 400);

			ShowCash(bot);
		}

		public static void ShowCash(Player player)
		{
			Assert.IsNotNull(player.Cash);
			if (player.Cash > 0)
			{
				Assert.IsTrue(player.Cash > 0);
				Console.WriteLine($"Cash is {player.Cash}.");
			}
			else
			{
				Assert.IsFalse(player.Cash > 0);
				Console.WriteLine("Lost all money!");
			}
			
		}
	}
}
