using System;
using AbstractPlayer;
using Action;

namespace Task2Roulette
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Casino!");
            ConsoleKeyInfo cki;
            Player player = new Player();
            GameProcess gameProcess = new GameProcess();

            while (player.wallet > 0)
            {
                Console.WriteLine("Press esc if you want stop. Press any key if you want play.");
                cki = Console.ReadKey(true);
                if (cki.Key != ConsoleKey.Escape)
                {
                    player.Bet();
                    player.wallet = gameProcess.Result(player.wallet, player.betName, player.currentBet);
                }
                else break;
            }
            

        }
    }
}
