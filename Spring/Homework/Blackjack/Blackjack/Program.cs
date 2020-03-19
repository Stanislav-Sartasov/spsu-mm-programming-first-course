using System.Collections.Generic;


namespace Blackjack
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game;
            game = new Game(new List<Player> { new RandomBot(1000, "Random_Bot", PlayerStatus.Bot), new BasicStrategyBot(1000, "BSBot", PlayerStatus.Bot), new Player(1000, "RealPlayer", PlayerStatus.RealPlayer) },
            new Deck(), new Croupier(PlayerStatus.Bot), 10, 400);
            game.StartGame();
        }
    }
}
 