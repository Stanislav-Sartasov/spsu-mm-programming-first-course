using NUnit.Framework;
using System;

namespace Roulette.Tests
{
	public class Tests
	{
		[Test]
		public void BotTest()
		{
			int averageCashM, averageCashL;
			averageCashL = averageCashM = 0;
			for (int i = 0; i < 100; i++)
			{
				MartingaleBot martingale = new MartingaleBot();
				LightBot lightBot = new LightBot();
				martingale.Bet();
				lightBot.Bet();
				averageCashM += martingale.GetBalance();
				averageCashL += lightBot.GetBalance();
			}
			averageCashM /= 100;
			averageCashL /= 100;
			Console.WriteLine(averageCashM);
			Console.WriteLine(averageCashL);
		}
			
	}
}