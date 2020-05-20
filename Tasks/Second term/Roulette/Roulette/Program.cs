using Roulette.Casino;
using Roulette.People;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Roullete
{
    class Program
    {
        static void Main(string[] args)
        {
            Table table = new Table();
            List<AbstractPlayer> bots = new List<AbstractPlayer>() { new Bot(0), new Bot(1), new Bot(2) };
            
            for (int i = 0; i < 500; i++)
            {
                List<AbstractPlayer> delList = new List<AbstractPlayer>();
                foreach (AbstractPlayer bot in bots)
                    bot.SetBet(table.ShowAmountOfMoney());
                table.Iteration(bots);
                Console.WriteLine(table.ShowStatus());
                foreach (AbstractPlayer bot in bots)
                {
                    int x = bot.ViewAmountOfMoney();
                    Console.WriteLine(bot.ShowStatus());
                    if (x < 1)
                    {
                        Console.WriteLine("Player are bankrupt");
                        delList.Add(bot);
                    }
                }
                foreach (AbstractPlayer delBot in delList)
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
