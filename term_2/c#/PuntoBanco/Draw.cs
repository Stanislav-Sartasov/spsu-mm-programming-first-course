using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuntoBanco
{
    class Draw : IDraw
    {
        private int[] puntos;
        private int[] bancos;
        public string Color(int card)
        {
            string str = "none";
            card = card % 1000;
            card = (card - (card % 100)) / 100;
            switch (card)
            {
                case 1:
                    str = "Cherva";
                    break;
                case 2:
                    str = "Buba";
                    break;
                case 3:
                    str = "Krest";
                    break;
                case 4:
                    str = "Pika";
                    break;

            }
            return str;
        }
        public string Cost(int card)
        {
            string str = "none";
            card = card % 100;
            if (card < 10)
            {
                if (card == 1)
                {
                    str = "A";
                }
                else
                {
                    str = card.ToString();
                }
            }
            else
            {
                switch (card)
                {
                    case 10:
                        str = "10";
                        break;
                    case 11:
                        str = "J";
                        break;
                    case 12:
                        str = "Q";
                        break;
                    case 13:
                        str = "K";
                        break;
                }
            }
            return str;
        }

        public void PreRound(Gamer[] g)
        {
            int f = 0;
            foreach (var i in g)
            {
                Console.WriteLine($"index: {f}, money: {i.moneyMoment}");
                f++;
            }
        }
        public void Winner(int who)
        {
            Console.Write("The winner is ");
            if (who == 0)
                Console.WriteLine("Punto");
            if (who == 1)
                Console.WriteLine("Banco");
            if (who == 2)
                Console.WriteLine("No one (tie)");
        }
        public void BotsBets(SomeBet[] first)
        {

        }
        public void AllBets(SomeBet[] first)
        {

        }
        public void Natural()
        {

        }
        public void Enough(int who)
        {
            if (who == 0)
                Console.WriteLine("Punto is enough");
            if (who == 1)
                Console.WriteLine("Banco is enough");
        }
        public void ShowFirst(SomeBet[] first, int pf, int ps, int bf, int bs)
        {
            puntos = new int[2] { pf, ps };
            bancos = new int[2] { bf, bs };
            Console.WriteLine($"Punto's cards {Color(pf)}{Cost(pf)}-{pf}, {Color(ps)}{Cost(ps)}-{ps}");
            Console.WriteLine($"Banco's cards {Color(bf)}{Cost(bf)}-{bf}, {Color(bs)}{Cost(bs)}-{bs}");
        }
        public void ShowSecond(SomeBet[] first, int who, int card)
        {
            if (who == 0)
                Console.Write("Punto's");
            if (who == 1)
                Console.Write("Banco's");
            Console.WriteLine($" next card is {Color(card)}{Cost(card)}-{card}");
        }
        public void InitDeck()
        {
            Console.WriteLine("new deck rotation");
        }
        public void AskMoneyToGo()
        {
            Console.Write("how mutch money you will give to start the game: ");
        }
        public void ShowMyMoney(Gamer g)
        {
            Console.WriteLine($"Now you have {g.moneyMoment} $");
        }
        public void RedyToGo()
        {
            Console.Write("If you want to play 1 Round type '1' else type any other integer(0): ");
        }
        public void DoBet(int moneyMoment)
        {
            Console.WriteLine($"Now yo have {moneyMoment}$, type first target of your bet(0 = punto, 1 = banco; 2 = tie) press enter after target\nThen type sum of money\n stock target is 0, stock sum is {moneyMoment}\n");
        }
        public void IntError()
        {
            Console.WriteLine("Y have entered not integer default value = 0");
        }
    }
}
