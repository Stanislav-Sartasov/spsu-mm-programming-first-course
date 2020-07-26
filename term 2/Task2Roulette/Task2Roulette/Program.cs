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

            while (player.GetBalance() > 0)
            {
                Console.WriteLine("Press esc if you want stop. Press any key if you want play.");
                cki = Console.ReadKey(true);
                if (cki.Key != ConsoleKey.Escape)
                {
                    
                    player.Bet();
                    GameProcess.TypeOfBets choice = (GameProcess.TypeOfBets)player.GetChoice();
                    bool result = gameProcess.IsWin(choice, player.GetCell());
                    player.RoundResult(result, gameProcess.GetCoefficient(choice));
                    Console.WriteLine(player.GetBalance());
                }
                else break;
            }
            

        }
    }
}
