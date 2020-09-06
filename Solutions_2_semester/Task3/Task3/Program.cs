using System;

namespace Task3
{
    class Program
    {
        static void Main()
        {
            ABot[] bot = new ABot[4];
            int startBudget = 1000000;
            GameManager game = new GameManager(startBudget, 4, null, 0);
            Player player = game.AddPlayer("You");
            int counter = 0;
            for (int i = 0; i < 4; i++)
            {
                if (i % 3 == 0)
                {
                    bot[i] = new MartingaleBot();
                    bot[i] = new MartingaleBot();
                    bot[i].Connect(game.AddPlayer("Martin_" + i.ToString()));
                }
                else if (i % 3 == 1)
                {
                    bot[i] = new GoldenRatioBot();
                    bot[i].Connect(game.AddPlayer("Goldy_" + i.ToString()));
                }
                else
                {
                    bot[i] = new RandomBot();
                    bot[i].Connect(game.AddPlayer("Randy_" + i.ToString()));
                }
            }

            void EndGame()
            {
                Console.Clear();
                Console.WriteLine($"You left the game with {player.bank}$ at {counter} iteration");

                if (counter == 0)
                    Console.WriteLine("You made a good choise");
                else if (player.bank >= startBudget)
                    Console.WriteLine("Well played");
                else if (player.bank == 0)
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
                    Visual.PreDraw(game.GetPlayers(), flag);
                    flag = false;

                    Console.SetCursorPosition(0, 21);

                    while (Console.KeyAvailable)
                        Console.ReadKey(true);

                    Console.SetCursorPosition(0, 21);

                    try
                    {
                        Console.WriteLine("Common types: <player> <bank> <draw> <pass> <leave>");
                        Console.Write("Your bateType: ");
                        Console.CursorVisible = true;
                        Field type = Field.none;
                        switch (Console.ReadLine())
                        {
                            case "player":
                            case "1":
                                type = Field.player;
                                break;
                            case "bank":
                            case "2":
                                type = Field.bank;
                                break;
                            case "draw":
                            case "3":
                                type = Field.draw;
                                break;
                            case "pass":
                            case "4":
                                type = Field.none;
                                break;
                            case "leave":
                            case "5":
                                EndGame();
                                return;
                            default:
                                continue;
                        }

                        int bate = 0;
                        if (type != Field.none)
                        {
                            Console.Write("Your bate: ");
                            bate = int.Parse(Console.ReadLine());
                            Console.CursorVisible = false;
                        }
                        if (!player.MakeBet(bate, type))
                            continue;
                    }
                    catch
                    {
                        continue;
                    }

                    for (int i = 0; i < 4; i++)
                    {
                        if (bot[i].bank == 0)
                            game.KickPlayer(bot[i].connectedPlayer);
                        else
                        {
                            bot[i].MakeBet();
                        }
                    }

                    Console.SetCursorPosition(0, 21);

                    for (int i = 0; i < 240; i++)
                        Console.Write(' ');

                    GameLog log = game.ProduceGame();
                    if (log == null)
                        continue;

                    counter++;

                    Visual.LogDraw(log);

                    Console.SetCursorPosition(0, 21);
                    Console.Write("Press Enter to continue");

                    while (Console.KeyAvailable)
                        Console.ReadKey(true);

                    while (Console.ReadKey().Key != ConsoleKey.Enter) ;

                    if (!player.active)
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