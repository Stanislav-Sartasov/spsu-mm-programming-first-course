using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using Unity;
using Unity.Injection;
using GameDescription;

namespace SeventhTask
{
	[TestClass]
	public class UnityTest
	{
		[TestMethod]
		public void UnityTestMethod()
		{
			IUnityContainer unity = new UnityContainer();
			unity.RegisterType<Game>();
			unity.RegisterType<Player, FirstBotPlayer>();
			unity.RegisterType<Player, SecondBotPlayer>();

			var game = unity.Resolve<Game>();

			var firstBot = unity.Resolve<FirstBotPlayer>();
			game.Start(firstBot, 100);
			PrintCash(firstBot); // First bot cash

			var secondBot = unity.Resolve<SecondBotPlayer>();
			game.Start(secondBot, 100);
			PrintCash(secondBot); // Second bot cash
		}

		public void PrintCash(Player player)
		{
			Assert.IsNotNull(player.Cash);
			if (player.Cash > 0)
			{
				Debug.WriteLine($"Cash is {player.Cash}");
			}
			else
			{
				Debug.WriteLine("Lose all money!");
			}
		}
	}
}
