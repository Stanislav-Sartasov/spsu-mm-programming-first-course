using System;
using System.Collections.Generic;
using System.Text;

namespace Roulette
{
	public abstract class APlayer
	{
		protected int cash;
		protected string name;
		protected int currentBet;
		protected int typeOfBet;
		protected int cells;

		public int GetBet()
		{
			return currentBet;
		}

		public int GetBalance()
		{
			return cash;
		}
		public int GetTypeOfBet()
		{
			return typeOfBet;
		}
		public int GetCell()
		{
			return cells;
		}
		public string GetName()
		{
			return name;
		}
		public void Winnings(int coefficient, bool result)
		{
			if (result)
			{
				cash += coefficient * currentBet;
				Console.WriteLine($"{GetName()} won {coefficient * currentBet}");
			}
			else
			{
				Console.WriteLine($"{GetName()} lose");
				cash -= currentBet;
			}
		}

		public abstract void Bet();
	}
}
