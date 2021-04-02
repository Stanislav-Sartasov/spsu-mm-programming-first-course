using System;


namespace Roulette
{
	class Program
	{

		static void Main(string[] args)
		{
			Console.WriteLine("Welcome to the roulette table!");
			Player player = new Player();
			Game game = new Game();
			Console.WriteLine("Press esc if you want to exit. Press any other key to continue");
			while (player.GetBalance() >= 100)
			{
				if (Console.ReadKey().Key != ConsoleKey.Escape)
				{
					player.Bet();
					bool result = game.CheckOfWin((Game.TypeOfBet)player.GetTypeOfBet(), player.GetCell());
					player.Winnings(game.GetCoefficient((Game.TypeOfBet)player.GetTypeOfBet()), result);
					Console.WriteLine($"Your balance: {player.GetBalance()}");
				}
				else break;
			}
				Console.WriteLine("The minimum bet is one hundred.Insufficient funds on the account.");
		}
	}
}
