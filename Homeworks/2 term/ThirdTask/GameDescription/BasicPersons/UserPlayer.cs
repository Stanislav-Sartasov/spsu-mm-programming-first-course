using System;
using System.Collections.Generic;
using System.Text;

namespace ThirdTask.GameDescription
{
	public class UserPlayer : Player
	{
		public override void MakeBet()
		{
			Console.WriteLine("Your bet: ");
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
		}

		public override void Action(Pad pad)
		{
			Console.WriteLine("Actions: \"Hit\", \"Stand\", \"Double\", \"Split\", \"Surrender\".\nChoose your action.");
			while (true)
			{
				var action = Console.ReadLine();
				if (action == "Hit" || action == "Stand" || action == "Surrender" || action == "Double" || action == "Split") // условия прописать здесь - переписать ввод
				{
					InputForAction = action;
					break;
				}
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