using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace GameDescription
{
	public class Game
	{
		#region Rules
		/* 
		Surrender is available only after distribution of the first two cards. 
		Double can be also available after surrender. 
		All pictures and 10 are equal. 
		*/
		#endregion
		private int GamesLeft { get; set; } = -1; // Negative if player is user (using for output)
		public void Start(Player player, int numOfGames = 0)
		{
			if (numOfGames != 0)
			{
				GamesLeft = numOfGames;
			}

			if (GamesLeft == -1)
			{
				Console.WriteLine($"Welcome to blackjack!\nYour cash: {player.Cash}.\nDo you want to play? (\"Yes\" / \"No\")");
			}

			var dealer = new Dealer();
			var pad = new Pad();

			if (GamesLeft == -1 || GamesLeft > 0)
			{
				if (player.IsContinue(GamesLeft) == true && dealer.Cash > 0)
				{
					if (GamesLeft != -1)
					{
						GamesLeft--;
					}
				}
				else
				{
					if (dealer.Cash <= 0 && GamesLeft == -1)
					{
						Console.WriteLine("How? Casino never loses, doesn't it?\n");
					}
					Environment.Exit(0);
				}
			}

			while(true)
			{
				player.MakeBet(); // Bet

				#region CardsInitialization

				dealer.FirstCardsInitialization(pad);
				player.FirstCardsInitialization(pad);
				if (GamesLeft == -1)
				{
					Console.WriteLine($"Dealer's opened card: {dealer.SecondCard}.\nYour first two cards: {player.FirstCard}, {player.SecondCard}.");
				}

				#endregion

				#region GameProcess

				if (player.SumOfAllCards == 21 && GamesLeft == -1) // Player's blackjack on cards distribution
				{
					Console.WriteLine($"Sum of all your cards is {player.SumOfAllCards}!");
				}

				if (player.SumOfAllCards == 21 && dealer.SecondCard < 10)
				{
					
					CheckWinner(player, dealer); // Player's win
				}
				else
				{
					while (player.SumOfAllCards < 21 && player.StandFlag != 1 && player.SumOfAllCards != 50)
					{
						player.Action(pad);

						if (GamesLeft == -1 && player.StandFlag != 1 && player.SumOfAllCards != 50)
						{
							Console.WriteLine($"Your first two cards: {player.FirstCard}, {player.SecondCard}; your other cards sum: {player.OtherCards}.");

							if (player.SumOfAllCards > 21)
							{
								Console.WriteLine("Overshoot!");
							}
							
							if (player.SumOfAllCards == 21)
							{
								Console.WriteLine($"Sum of all your cards is {player.SumOfAllCards}!");
							}
						}
					}

					if (player.SumOfAllCards <= 21 && player.SumOfAllCards != 50)
					{;
						while (dealer.SumOfAllCards < 21 && dealer.StandFlag != 1)
						{
							dealer.Action(pad);
						}

						if (GamesLeft == -1)
						{
							Console.WriteLine($"Dealer's first two cards: {dealer.FirstCard}, {dealer.SecondCard}; dealer's other cards sum: {dealer.OtherCards}");
						}
					}

					CheckWinner(player, dealer);
				}

				#endregion

				player.Clear();
				dealer.Clear();

				if (GamesLeft == -1 || GamesLeft > 0)
				{
					if (GamesLeft == -1)
					{
						Console.WriteLine("Continue playing? (\"Yes\" / \"No\")");
					}

					if (player.IsContinue(GamesLeft) == true && dealer.Cash > 0)
					{
						if (GamesLeft != -1)
						{
							if (GamesLeft > 0)
							{
								GamesLeft--;
								if (GamesLeft == 0)
								{
									break;
								}
							}
						}
					}
					else
					{
						if (dealer.Cash <= 0 && GamesLeft == -1)
						{
							Console.WriteLine("How? Casino never loses, doesn't it?\n");
						}
						break;
					}
				}
			}
		}

		private void CheckWinner(Player player, Dealer dealer)
		{
			if (player.SumOfAllCards == 21) // Blackjack
			{
				if (dealer.SumOfAllCards == 21)
				{
					FindWinner(player, dealer, 0);
				}
				else
				{
					FindWinner(player, dealer, 2);
				}
			}
			else if (player.SumOfAllCards > 21 || player.SumOfAllCards == 50)
			{
				FindWinner(player, dealer, 1);
			}
			else
			{
				if (dealer.SumOfAllCards > 21 || (player.SumOfAllCards > dealer.SumOfAllCards))
				{
					FindWinner(player, dealer, 2);
				}
				else if (player.SumOfAllCards == dealer.SumOfAllCards)
				{
					FindWinner(player, dealer, 0);
				}
				else
				{
					FindWinner(player, dealer, 1);
				}
			}

			if (GamesLeft == -1)
			{
				Console.WriteLine($"Your cash: {player.Cash}");
			}
		}

		private void FindWinner(Player player, Dealer dealer, int prm)
		{
			switch (prm)
			{
				case 1:
					dealer.Cash += player.Bet;
					break;
				case 2:
					player.Cash += (int)(2.2 * player.Bet); // 6:5
					dealer.Cash -= player.Bet;
					break;
				default:
					player.Cash += player.Bet;
					break;
			}
			if (GamesLeft == -1)
			{
				switch (prm)
				{
					case 1:
						Console.WriteLine("Dealer wins!");
						break;
					case 2:
						Console.WriteLine("Player wins!");
						break;
					default:
						Console.WriteLine("Draw");
						break;
				}
			}
		}
	}
}