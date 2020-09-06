using System;
using Task3;

namespace Task3
{
    class Program
    {
        static void Main()
        {
            Player[] players = new Player[5];
            int startBudget = 1000000;
            GameMaster game = new GameMaster(startBudget);
            Random rand = new Random();
            int counter = 0;
            for (int i = 1; i < 5; i++)
            {
                if (i < 3)
                    players[i] = new MartingaleBot();
                else
                    players[i] = new GoldenRatioBot();
                players[i].SetRand(rand.Next());
                ((Bot)players[i]).AutoLeaving = true;
            }

            players[0] = new Player() { Name = "You" };
            players[2].Name = "Marat";
            players[4].Name = "Goldy";
            
            game.AddPlayer(players[0]);
            game.AddPlayer(players[1]);
            game.AddPlayer(players[3]);
            game.AddPlayer(players[2]);
            game.AddPlayer(players[4]);

            void EndGame()
            {
                Console.Clear();
                Console.WriteLine($"You left the game with {players[0].Budget}$ at {counter} iteration");

                if (counter == 0)
                    Console.WriteLine("You made a good choise");
                else if (players[0].Budget >= startBudget)
                    Console.WriteLine("Well played");
                else if (players[0].Budget == 0)
                    Console.WriteLine("Congratulations!");
                else
                    Console.WriteLine("At least not empty - handed");

                Console.Write("Press Enter to continue");

                while (Console.KeyAvailable)
                    Console.ReadKey(true);

                while (Console.ReadKey().Key != ConsoleKey.Enter) ;

                return;
            }

            for (; ; )
            {
                bool flag = true;
                for (; ; )
                {
                    Console.Clear();
                    Visualizer.PreDrow(game.PlayerList, flag);
                    flag = false;

                    Console.SetCursorPosition(0, 21);

                    while (Console.KeyAvailable)
                        Console.ReadKey(true);

                    Console.SetCursorPosition(0, 21);

                    try
                    {
                        Console.WriteLine("Common types: <player> <bank> <tie> <leave>");
                        Console.Write("Your bateType: ");
                        Console.CursorVisible = true;
                        int type = 0;
                        switch (Console.ReadLine())
                        {
                            case "player":
                                type = 0;
                                break;
                            case "bank":
                                type = 1;
                                break;
                            case "tie":
                                type = 2;
                                break;
                            case "leave":
                                EndGame();
                                return;
                            default:
                                continue;
                        }
                        Console.Write("Your bate: ");
                        int bate = int.Parse(Console.ReadLine());
                        Console.CursorVisible = false;
                        if (players[0].MakeBate(bate, type) == -1)
                            continue;
                    }
                    catch
                    {
                        continue;
                    }

                    bool b = true;

                    for (int i = 0; i < 5; i++)
                    {
                        if (players[i].Budget == 0)
                            game.CickPlayer(players[i]);
                        else
                        {
                            b = false;
                            players[i].MakeBate();
                        }
                    }

                    Console.SetCursorPosition(0, 21);

                    for (int i = 0; i < 240; i++)
                        Console.Write(' ');

                    if (game.StartGame() != 0)
                        continue;

                    counter++;

                    Visualizer.LogDrow(game.GetLog());

                    Console.SetCursorPosition(0, 21);
                    Console.Write("Press Enter to continue");

                    while (Console.KeyAvailable)
                        Console.ReadKey(true);

                    while (Console.ReadKey().Key != ConsoleKey.Enter);

                    if (players[0].place == null || b)
                    {
                        EndGame();
                        return;
                    }

                    break;
                }
            }
        }
    }
}