using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace ThirdTask.GameDescription
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
		private string GameIsGoing { get; set; } = "first_game"; // Start position
		private int GamesLeft { get; set; } = -1; // Negative if player is user
		public void Start(Player player, int numOfGames = 0)
		{
			var dealer = new Dealer();

			if (numOfGames != 0)
			{
				GamesLeft = numOfGames;
			}

			if (GamesLeft == -1)
			{
				Console.WriteLine($"Welcome to blackjack!\nYour cash: {player.Cash}.\nDo you want to play? (\"Yes\" / \"No\")");
			}
			ContinueGame(player, dealer);

			while (GameIsGoing == "game_is_on")
			{
				var pad = new Pad();

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

				if (player.PersonStatus == "blackjack" && dealer.SecondCard < 10)
				{
					CheckWinner(player, dealer); // BlackJack
				}
				else
				{
					while (player.PersonStatus == "in_game")
					{
						player.Action(pad);

						if (player.PersonStatus != "stand" && player.PersonStatus != "surrender" && player.PersonStatus != "blackjack" && GamesLeft == -1)
						{
							Console.WriteLine($"Your first two cards: {player.FirstCard}, {player.SecondCard}; your other cards sum: {player.OtherCards}");
						}
					}

					if (player.PersonStatus != "lose" && player.PersonStatus != "surrender")
					{
						while (dealer.PersonStatus == "in_game")
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

				Cleaner(player, dealer);
				ContinueGame(player, dealer);
			}
		}

		private void CheckWinner(Player player, Dealer dealer)
		{
			if (player.PersonStatus == "blackjack") // Blackjack
			{
				if (dealer.PersonStatus == "blackjack")
				{
					FindWinner(player, dealer, 0);
				}
				else
				{
					FindWinner(player, dealer, 2);
				}
			}
			else if (player.PersonStatus == "lose" || player.PersonStatus == "surrender")
			{
				FindWinner(player, dealer, 1);
			}
			else
			{
				if (dealer.PersonStatus == "lose" || (player.SumOfAllCards() > dealer.SumOfAllCards()))
				{
					FindWinner(player, dealer, 2);
				}
				else if (player.SumOfAllCards() == dealer.SumOfAllCards())
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
					player.Cash += (int)(2.2 * player.Bet); // выигрыш 6:5
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

		private void ContinueGame(Player player, Dealer dealer)
		{
			if (GamesLeft == -1 || GamesLeft > 0)
			{
				if (GameIsGoing == "first_game")
				{
					if (player.IsContinue() == "Yes")
					{
						GameIsGoing = "game_is_on";
						if (GamesLeft != -1)
						{
							GamesLeft--;
						}
					}
					else
					{
						GameIsGoing = "stop_game";
					}
				}
				else
				{
					if (player.Cash <= 0)
					{
						if (GamesLeft == -1)
						{
							Console.WriteLine("Your cash is empty, seems you got twisted up in this scene.\n" +
								"From where you're kneeling it must seem like an 18-carat run of bad luck.\n" +
								"Truth is... the game was rigged from the start.");
						}

						GameIsGoing = "stop_game";
					}
					else if (dealer.Cash < 0)
					{
						if (GamesLeft == -1)
						{
							Console.WriteLine("How? Casino never loses, doesn't it?\n");
						}

						GameIsGoing = "stop_game";
					}
					else
					{
						if (GamesLeft == -1)
						{
							Console.WriteLine("Continue playing? (\"Yes\" / \"No\")");
						}

						if (player.IsContinue() == "Yes")
						{
							GameIsGoing = "game_is_on";
							if (GamesLeft != -1)
							{
								GamesLeft--;
							}
						}
						else
						{
							GameIsGoing = "stop_game";
						}
					}
				}
			}
			else
			{
				GameIsGoing = "stop_game";
			}
		}

		private void Cleaner(Player player, Dealer dealer)
		{
			player.Clear();
			dealer.Clear();
		}
	}
}