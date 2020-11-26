using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace ThirdTask.GameDescription
{
	public class Game
	{
		private string GameIsGoing { get; set; } = "first_game"; // Start position
		public void Start() // игрок / бот // число игр (учесть в выборе функций - подсчет внутри бота + информация)
		{
			var player = new UserPlayer(); // Player
			var dealer = new Dealer();
			
			//player.StartMenu();
			Console.WriteLine($"Welcome to blackjack!\nYour cash: {player.Cash}.\nDo you want to play? (\"Yes\" / \"No\")"); //сюда же правила и оговорки
			Continue(player, dealer); // переписать внутрь класса
			while (GameIsGoing == "game_is_on")
			{
				var pad = new Pad();

				player.MakeBet(); // Bet

				#region CardsInitialization // внести в конструктор дилера и игрока

				dealer.FirstCardsInitialization(pad);
				Console.WriteLine($"Dealer's opened card: {dealer.SecondCard}");

				player.FirstCardsInitialization(pad); 
				Console.WriteLine($"Your first two cards: {player.FirstCard}, {player.SecondCard}");

				#endregion

				#region GameProcess // для удобства написать вывод суммы

				if (player.PersonStatus == "blackjack" && dealer.SecondCard < 10)
				{
					CheckWinner(player, dealer); // BlackJack
				}
				else
				{
					while (player.PersonStatus == "in_game")
					{
						player.Action(pad);

						if (player.PersonStatus != "stand" && player.PersonStatus != "surrender" && player.PersonStatus != "blackjack")
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

						Console.WriteLine($"Dealer's first two cards: {dealer.FirstCard}, {dealer.SecondCard}; dealer's other cards sum: {dealer.OtherCards}");
					}

					CheckWinner(player, dealer);
				}

				#endregion

				Cleaner(player, dealer);
				Continue(player, dealer);
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

			Console.WriteLine($"Your cash: {player.Cash}");
		}

		private void FindWinner(Player player, Dealer dealer, int prm)
		{
			switch (prm)
			{
				case 1:
					Console.WriteLine("Dealer wins!");
					dealer.Cash += player.Bet; ;
					break;
				case 2:
					Console.WriteLine("Player wins!");
					player.Cash += (int)(2.2 * player.Bet); // выигрыш 6:5
					dealer.Cash -= player.Bet;
					break;
				default:
					Console.WriteLine("Draw");
					player.Cash += player.Bet;
					break;
			}
		} // внутрь через флаг??

		private void Continue(Player player, Dealer dealer) // внести внутрь игрока (поскольку у бота другой метод)
		{
			if (GameIsGoing == "first_game")
			{
				while (true)
				{
					var input = Console.ReadLine();
					if (input == "Yes")
					{
						GameIsGoing = "game_is_on";
						break;
					}
					if (input == "No")
					{
						GameIsGoing = "stop_game";
						break;
					}
				}
			}
			else
			{
				if (player.Cash <= 0)
				{
					Console.WriteLine("Your cash is empty, seems you got twisted up in this scene.\n" +
						"From where you're kneeling it must seem like an 18-carat run of bad luck.\n" +
						"Truth is... the game was rigged from the start.");
					GameIsGoing = "stop_game";
				}
				else if (dealer.Cash < 0)
				{
					Console.WriteLine("How? Casino never loses, doesn't it?\n");
					GameIsGoing = "stop_game";
				}
				else
				{
					Console.WriteLine("Continue playing? (\"Yes\" / \"No\")");
					while (true)
					{
						var input = Console.ReadLine();
						if (input == "Yes")
						{
							GameIsGoing = "game_is_on";
							break;
						}
						if (input == "No")
						{
							GameIsGoing = "stop_game";
							break;
						}
					}
				}
			}
		}

		private void Cleaner(Player player, Dealer dealer)
		{
			player.Clear();
			dealer.Clear();
		}
	}
}