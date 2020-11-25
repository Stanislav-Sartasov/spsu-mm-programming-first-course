using System;
using System.Collections.Generic;
using System.Text;

namespace ThirdTask.GameDescription
{
	public class UserPlayer : Player
	{
		public override void MakeBet()
		{
			Console.WriteLine("Make your bet.");
			//уменьшение ставки или выход из игры (через отрицательную ставку)
			Bet = Convert.ToInt32(Console.ReadLine());

			if (Bet <= Cash)
			{
				Cash -= Bet;
				Console.WriteLine("This game is going to be perfect...");
			}
			else
			{
				Console.WriteLine("Not enough money!");
			}
			//
		}

		public override void Action(Pad pad)
		{
			int chs = 0;
			if (SplitAndSurrenderIsAllowed != 0)
			{
				DoubleIsAllowed = 1;
				chs = 2;
			}
			if (DoubleIsAllowed != 0 && chs != 2) 
			{
				chs = 1;
			}

			string action;
			switch (chs) // учесть 2 колоды
			{
				case 1: // readstring с допустимыми действиями??
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
					Console.WriteLine("Actions: \"Hit\", \"Stand\", \"Double\", \"Split\", \"Surrender\".\nChoose your action.");
					while (true)
					{
						action = Console.ReadLine();
						if (action == "Hit" || action == "Stand" || action == "Surrender" || action == "Double" || action == "Split")
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

			base.Action(pad);
			//Hit
			//Stand
			//Double (может быть после split)
			//Split (новый конструктор, только 1 ход)
			//Surrender(только 1 ход)
		}
	}
}