using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace ThirdTask.GameDescription
{
	public class Game
	{
		private int GameIsGoing { get; set; } = 2;
		public void Start() // игрок / бот
		{
			var player = new UserPlayer(); // Player
			var dealer = new Dealer();
			
			Console.WriteLine("Welcome to blackjack!"); //правила


			while (Continue(player, dealer) == 1)
			{
				var pad = new Pad();

				player.MakeBet(); // bet

				for (int i = 0; i < 2; i++)
				{
					player.GetCard(pad);
				}
				player.ChangeStatus();
				Console.WriteLine($"First two cards: {player.FirstCard}, {player.SecondCard}");
				////

				while (player.GameStatus == 2)
				{ 
					//player.Action();
					player.ChangeStatusExit(); // переделать на Action

					if (player.GameStatus == 2)
					{
						player.GetCard(pad);
						player.ChangeStatus();
						Console.WriteLine($"First two cards: {player.FirstCard}, {player.SecondCard}\nOther cards sum: {player.OtherCards}");
					}
				}

				if (player.GameStatus != 0)
				{
					/*
					for (int i = 0; i < 2; i++)
					{
						dealer.GetCard(pad);
					}
					dealer.ChangeStatus();

					while (dealer.GameStatus == 3)
					{
						dealer.GetCard(pad);
						dealer.ChangeStatus();
					}
					*/
					dealer.Action(pad);

					Console.WriteLine($"First two cards: {dealer.FirstCard}, {dealer.SecondCard}; other cards sum: {dealer.OtherCards}");
				}

				CheckWinner(player, dealer);
				//Cleaner;
			}
		}

		public void CheckWinner(Player player, Dealer dealer)
		{
			//blackjack

			if (player.SumOfAllCards() == dealer.SumOfAllCards()) // переписывать if для перебора blackjack
			{
				Console.WriteLine("Draw");
			}
			else if ((player.SumOfAllCards() > dealer.SumOfAllCards()) && (player.SumOfAllCards() < 22))
			{
				Console.WriteLine("Player wins!");
				player.Cash += player.Bet;
				dealer.Cash -= player.Bet;
			}
			else
			{
				Console.WriteLine("Dealer wins!");
				//player.Cash -= player.Bet;
				dealer.Cash += player.Bet;
			}

			Console.WriteLine($"Your cash: {player.Cash}");
			//cash processing
		}

		public int Continue(Player player, Dealer dealer)
		{
			if (GameIsGoing == 2)
			{
				GameIsGoing = 1;
			}
			else
			{
				if (player.Cash <= 0)
				{
					Console.WriteLine("Your cash is empty, seems you got twisted up in this scene.\n" +
						"From where you're kneeling it must seem like an 18-carat run of bad luck.\n" +
						"Truth is... the game was rigged from the start.");
					GameIsGoing = 0;
				}
				else if (dealer.Cash < 0)
				{
					Console.WriteLine("How? Casino never loses, doesn't it?\n");
					GameIsGoing = 0;
				}
				else
				{
					Console.WriteLine("Continue playing ? (1 / 0)");
					GameIsGoing = Convert.ToInt32(Console.ReadLine());
				}
			}
			return GameIsGoing;
		}

		public void Cleaner(Player player, Dealer dealer)
		{
			player.Clear();
			dealer.Clear();
		}
	}
}

//