using Roulette.Casino;
using Roulette.Players;

using System;
using System.Collections.Generic;
using System.Threading;

namespace Roulette
{
    class Program
    {
        static void Main(string[] args)
        {
            Table table = new Table();
            List<IPlayer> bots = new List<IPlayer>() { CreateInstance.SelectBot(0), CreateInstance.SelectBot(1) };

            for (int i = 0; i < 2000; i++)
            {
                List<IPlayer> delList = new List<IPlayer>();
                foreach (IPlayer bot in bots)
                    bot.MakeBet(table.ShowAmountOfMoney());
                table.Iteration(bots);
                Console.WriteLine(table.ShowStatus());
                foreach (IPlayer bot in bots)
                {
                    int x = bot.ShowMoney();
                    Console.WriteLine(bot.ShowStatus());
                    if (x < 1)
                    {
                        Console.WriteLine("Player are bankrupt");
                        delList.Add(bot);
                    }
                }
                foreach (IPlayer delBot in delList)
                    bots.Remove(delBot);
                if (table.ShowAmountOfMoney() < 1)
                {
                    Console.WriteLine("Congratulations! Casino is bankrupt.");
                    break;
                }
                if (bots.Count == 0)
                {
                    Console.WriteLine("Congratulations, all players went bankrupt! The casino won(as expected).");
                    break;
                }
                Console.WriteLine("^^^^");
                //Thread.Sleep(1000);
            }
            Console.WriteLine("End.");
            Console.ReadKey();
        }
    }
}
