using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace ThirdTask.GameDescription
{
	public class Game
	{
		public void Start()
		{
			var player = new Human(); // конструктор
			var dealer = new Dealer();

			//ставка

			int isInGame = 1;

			for (int i = 0; i < 2; i++)
			{
				dealer.Turn(player);
			}

			player.ChangeStatus();

			while (player.GameStatus == 2)
			{
				Console.WriteLine($"First two cards: {player.FirstCard}, {player.SecondCard}"); // пересмотреть

				isInGame = Convert.ToInt32(Console.ReadLine());

				player.ChangeStatusExit(isInGame);

				if (player.GameStatus == 2)
				{
					dealer.Turn(player);
					player.ChangeStatus();
					Console.WriteLine($"Other cards sum: {player.OtherCards}");
				}
			}

			if (player.GameStatus != 0)
			{
				for (int i = 0; i < 2; i++)
				{
					dealer.Turn(dealer);
				}
				dealer.ChangeStatus();

				while (dealer.GameStatus == 3)
				{
					dealer.Turn(dealer);
					dealer.ChangeStatus();
				}

				Console.WriteLine($"First two cards: {dealer.FirstCard}, {dealer.SecondCard}, other cards sum: {dealer.OtherCards}");
			}

			CheckWinner(player, dealer);
		}

		public void CheckWinner(Player player, Dealer dealer)
		{
			//blackjack

			if (player.SumOfAllCards() == dealer.SumOfAllCards())
			{
				Console.WriteLine("Draw");
			}
			else if (player.SumOfAllCards() > dealer.SumOfAllCards())
			{
				Console.WriteLine("Player wins!");
			}
			else
			{
				Console.WriteLine("Dealer wins!");
			}

			//cash processing
		}
	}
}
