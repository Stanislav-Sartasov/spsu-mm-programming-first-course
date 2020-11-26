using System;
using CasinoIncidentRoulette.Roulette;
using CasinoIncidentRoulette.Bots;
using CasinoIncidentRoulette.Player;

namespace CasinoIncidentRoulette
{
    class Program
    {
        static void Main(string[] args)
        {
            Table table = new Table();

            MartingaleBot martingaleBot = new MartingaleBot();
            MakarovBot makarovBot = new MakarovBot();

            for (int i = 0; i < 400; i++)
            {
                if (martingaleBot.CanIBet())
                    martingaleBot.PlayerBet = martingaleBot.Bet(table);
                if (makarovBot.CanIBet())
                    makarovBot.PlayerBet = makarovBot.Bet(table);

                table.Roll();

                if (martingaleBot.CanIBet())
                    martingaleBot.CheckResult(martingaleBot.PlayerBet, table.LastExodus);
                if (makarovBot.CanIBet())
                    makarovBot.CheckResult(makarovBot.PlayerBet, table.LastExodus);
            }

            if (martingaleBot.CanIBet())
                Console.WriteLine($"Martingale bot didn't lost. Money: {martingaleBot.GetMoney()}");
            else
                Console.WriteLine($"Martingale bot lost. Money: {martingaleBot.GetMoney()}");

            if (makarovBot.CanIBet())
                Console.WriteLine($"Makarov bot didn't lost. Money: {makarovBot.GetMoney()}");
            else
                Console.WriteLine($"Makarov bot lost. Money: {makarovBot.GetMoney()}");
        }
    }
}
