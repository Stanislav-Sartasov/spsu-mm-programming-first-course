using System;
using CasinoIncidentRoulette.Roulette;
using CasinoIncidentRoulette.Bots;

namespace CasinoIncidentRoulette
{
    class Program
    {
        static void Main(string[] args)
        {
            Table.CreateTable();
            Table.Cell exodus;

            MartingaleBot martingaleBot = new MartingaleBot();
            Tuple<Table.Cell, int, int> martingaleBet = martingaleBot.Bet();

            MakarovBot makarovBot = new MakarovBot();
            Tuple<Table.Cell, int, int> makarovBet = makarovBot.Bet();

            for (int i = 0; i < 400; i++)
            {
                if (martingaleBot.CanIBet())
                    martingaleBet = martingaleBot.Bet();
                if (makarovBot.CanIBet())
                    makarovBet = makarovBot.Bet();

                exodus = Table.Roll();

                if (martingaleBot.CanIBet())
                    martingaleBot.CheckResult(martingaleBet, exodus);
                if (makarovBot.CanIBet())
                    makarovBot.CheckResult(makarovBet, exodus);
            }

            if (martingaleBot.CanIBet())
                Console.WriteLine($"Martingale bot didn't lost. Money: {martingaleBot.GetMoney()}");
            else
                Console.WriteLine($"Martingale bot lost. Money: {martingaleBot.GetMoney()}");

            if (makarovBot.CanIBet())
                Console.WriteLine($"Martingale bot didn't lost. Money: {makarovBot.GetMoney()}");
            else
                Console.WriteLine($"Martingale bot lost. Money: {makarovBot.GetMoney()}");
        }
    }
}
