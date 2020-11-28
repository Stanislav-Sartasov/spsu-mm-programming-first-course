using System;
using System.Collections.Generic;
using System.Text;

namespace GameDescription
{
	public class UserPlayer : Player
	{
		public override void MakeBet()
		{
			Console.WriteLine("Make your bet.");
			while (Bet <= 0)
			{
				try 
				{
					Bet = Convert.ToInt32(Console.ReadLine());
					if (Bet <= 0)
					{
						throw new Exception();
					}
				}
				catch
				{
					Console.WriteLine("Please, input positive int number.");
				}
			}

			if (Bet <= Cash)
			{
				Cash -= Bet;
				Console.WriteLine("This game is going to be perfect...");
			}
			else
			{
				Console.WriteLine("Not enough money!");
			}
		}

		public override void Action(Pad pad)
		{
			if (SumOfAllCards() == 21)
			{
				Console.WriteLine($"Sum of all cards is {SumOfAllCards()}");
			}
			else
			{
				int chs = 0;
				if (SurrenderIsAllowed != 0)
				{
					DoubleIsAllowed = 1;
					chs = 2;
				}
				if (DoubleIsAllowed != 0 && chs != 2)
				{
					chs = 1;
				}

				string action;
				switch (chs)
				{
					case 1:
						Console.WriteLine("Actions: \"Hit\", \"Stand\", \"Double\".\nChoose your action.");
						while (true)
						{
							action = Console.ReadLine();
							if (action == "Hit" || action == "Stand" || action == "Double")
							{
								InputForAction = action;
								break;
							}
						}
						break;
					case 2:
						Console.WriteLine("Actions: \"Hit\", \"Stand\", \"Double\", \"Surrender\".\nChoose your action.");
						while (true)
						{
							action = Console.ReadLine();
							if (action == "Hit" || action == "Stand" || action == "Surrender" || action == "Double")
							{
								InputForAction = action;
								break;
							}
						}
						break;
					default:
						Console.WriteLine("Actions: \"Hit\", \"Stand\".\nChoose your action.");
						while (true)
						{
							action = Console.ReadLine();
							if (action == "Hit" || action == "Stand")
							{
								InputForAction = action;
								break;
							}
						}
						break;
				}
			}

			base.Action(pad);
		}

		public override string IsContinue()
		{
			string input = "";
			while (input != "Yes" && input != "No")
			{
				input = Console.ReadLine();
			}

			return input;
		}
	}
}