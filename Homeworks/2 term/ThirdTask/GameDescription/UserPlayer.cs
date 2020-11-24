using System;
using System.Collections.Generic;
using System.Text;

namespace ThirdTask.GameDescription
{
	public class UserPlayer : Player
	{
		public void ChangeStatusExit()
		{
			Console.WriteLine("Continue ? (1 / 0)");
			int isInGame = Convert.ToInt32(Console.ReadLine());
			if (isInGame == 0) // не в игре
			{
				GameStatus = 3;
			}
		}

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
			Console.WriteLine("Choose your action.");
			string action;

			while (true)
			{
				action = Console.ReadLine();
				if (action == "Hit" || action == "Stand" || action == "Surrender" || action == "Double" || action == "Split") // условия прописать здесь
				{
					ApplyTurn(action, pad);
					break;
				}
			}
			//Hit
			//Stand
			//Double (может быть после split)
			//Split (новый конструктор, только 1 ход)
			//Surrender(только 1 ход)
		}
	}
}