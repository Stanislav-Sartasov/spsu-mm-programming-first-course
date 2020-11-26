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
            Table table1 = new Table();
            table1.CreateTable();
            Cell exodus;

            MartingaleBot martingaleBot = new MartingaleBot();
            Tuple<Cell, int, AbstractPlayer.TypeBet> martingaleBet = martingaleBot.Bet(table1);

            MakarovBot makarovBot = new MakarovBot();
            Tuple<Cell, int, AbstractPlayer.TypeBet> makarovBet = makarovBot.Bet(table1);

            for (int i = 0; i < 400; i++)
            {
                if (martingaleBot.CanIBet())
                    martingaleBet = martingaleBot.Bet(table1);
                if (makarovBot.CanIBet())
                    makarovBet = makarovBot.Bet(table1);

                exodus = table1.Roll();

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
                Console.WriteLine($"Makarov bot didn't lost. Money: {makarovBot.GetMoney()}");
            else
                Console.WriteLine($"Makarov bot lost. Money: {makarovBot.GetMoney()}");
        }
    }
}
