using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PuntoBanco;
using Moq;
using System.Runtime.CompilerServices;

namespace UnitTestPuntoBanco
{
	[TestClass]
	public class UnitTest
	{
		[TestMethod]
		public void TestBots()
		{
			SomeBet bet;
			bet.man = 0;
			bet.money = 0;
			bet.target = 1;
			int Rounds = 401;

			int callsBet = 0;
			int callsInput = 0;
			int callsReady = 0;
			var mock = new Mock<IInteraction>();
			mock.Setup(x => x.Ready()).Returns(() =>
			{
				callsReady++;
				if (callsReady != Rounds + 1)
				{
					Console.WriteLine("test 1");
					return true;
				}
				Console.WriteLine("test 0");
				return false;
			});
			mock.Setup(x => x.GetInt()).Returns(() =>
			{
				callsInput++;
				if (callsInput == 1)
				{
					Console.WriteLine("test 25");
					return 25;
				}
				Console.WriteLine("test 1");
				return 1;
			});
			//mock.Setup(x => x.doBet(ref It.Ref<SomeBet>.IsAny, It.IsAny<int>())).Callback((ref SomeBet bet, int money) => Console.WriteLine("s"));
			//Problem: ref bet - must change value in do.Bet(ref SomeBet, int)
			mock.Setup(x => x.DoBet(It.IsAny<SomeBet>(), It.IsAny<int>())).Returns(() =>
			{
				callsBet++;
				Console.WriteLine($"test: money{bet.money}");
				return bet;
			});
			//
			UserInterface user = new UserInterface(mock.Object);
			user.GoGame();
		}

		[TestMethod]
		public void TestGame()
		{
			SomeBet bet;
			bet.man = 0;
			bet.money = 1;
			bet.target = 1;
			int Rounds = 104;

			int callsBet = 0;
			int callsInput = 0;
			int callsReady = 0;
			var mock = new Mock<IInteraction>();
			mock.Setup(x => x.Ready()).Returns(() =>
			{
				callsReady++;
				if (callsReady != Rounds + 1)
				{
					Console.WriteLine("test 1");
					return true;
				}
				Console.WriteLine("test 0");
				return false;
			});
			mock.Setup(x => x.GetInt()).Returns(() => 
			{
				callsInput++;
				if (callsInput == 1)
				{
					Console.WriteLine("test 25");
					return 25;
				}
				Console.WriteLine("test 1");
				return 1; 
			});
			//mock.Setup(x => x.doBet(ref It.Ref<SomeBet>.IsAny, It.IsAny<int>())).Callback((ref SomeBet bet, int money) => Console.WriteLine("s"));
			//Problem: ref bet - must change value in do.Bet(ref SomeBet, int)
			mock.Setup(x => x.DoBet(It.IsAny<SomeBet>(), It.IsAny<int>())).Returns(() => 
			{
				callsBet++;
				Console.WriteLine($"test: money: {bet.money}$, target: {bet.target}");
				return bet;
			});
			//
			UserInterface user = new UserInterface(mock.Object);
			user.GoGame();

			Console.WriteLine($"\n end-----\ncalls of Input: {callsInput}\nMade bets: {callsBet}\nRounds: {callsReady - 1}");
		}

	}
}