using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace ThirdTask.GameDescription
{
	public class Game
	{
		private string GameIsGoing { get; set; } = "first_game"; // Start position
		public void Start() // игрок / бот // число игр
		{
			var player = new UserPlayer(); // Player
			var dealer = new Dealer();
			
			Console.WriteLine($"Welcome to blackjack!\nYour cash: {player.Cash}"); //сюда же правила и оговорки

			while (Continue(player, dealer) == "game_is_on")
			{
				var pad = new Pad();

				player.MakeBet(); // Bet

				player.FirstCardsInitialization(pad); // внести в конструктор дилера и игрока
				Console.WriteLine($"Your first two cards: {player.FirstCard}, {player.SecondCard}"); // открыть карту дилера - инциализировать его раньше при необходимости

				if (player.GameStatus == 1)
				{
					//blackjack у игрока
				}
				else
				{
					while (player.GameStatus == 2)
					{
						player.Action(pad); // зацикливается при Stand

						//Console.WriteLine(player.GameStatus);

						if (player.GameStatus != 3)
						{
							Console.WriteLine($"Your first two cards: {player.FirstCard}, {player.SecondCard}; your other cards sum: {player.OtherCards}");
						}

					}
				}

				if (player.GameStatus != 0)
				{
					dealer.FirstCardsInitialization(pad);

					while (dealer.GameStatus == 2)
					{
						dealer.Action(pad);
						//Console.WriteLine(dealer.GameStatus);
					}

					Console.WriteLine($"Dealer's first two cards: {dealer.FirstCard}, {dealer.SecondCard}; dealer's other cards sum: {dealer.OtherCards}");
				}

				CheckWinner(player, dealer);
				Cleaner(player, dealer);
			}
		}

		public void CheckWinner(Player player, Dealer dealer)
		{
			if (player.GameStatus == 1 || dealer.GameStatus == 1) // Blackjack
			{
				// переписать if для перебора blackjack
			}
			else
			{
				if (player.SumOfAllCards() == dealer.SumOfAllCards() || (player.SumOfAllCards() > 21 && dealer.SumOfAllCards() > 21))
					// рассмотреть случай перебора у обоих, изменить проигрыш дилера при проигрыше игрока
				{
					Console.WriteLine("Draw");
					player.Cash += player.Bet;
				}
				else if ((player.SumOfAllCards() > dealer.SumOfAllCards() || dealer.SumOfAllCards() > 21) && (player.SumOfAllCards() < 22))
				{
					Console.WriteLine("Player wins!");
					player.Cash += 2 * player.Bet; // уточнить выигрыш
					dealer.Cash -= player.Bet;
				}
				else // if (dealer.SumOfAllCards() < 22) // учесть surrender
				{
					Console.WriteLine("Dealer wins!");
					dealer.Cash += player.Bet;
				}
				//исправить - сумма у игрока 21, но он проиграл/не реагирует
			}

			Console.WriteLine($"Your cash: {player.Cash}");
		}

		public string Continue(Player player, Dealer dealer)
		{
			if (GameIsGoing == "first_game")
			{
				GameIsGoing = "game_is_on";
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
					Console.WriteLine("Continue playing ? (\"Yes\" / \"No\")");
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