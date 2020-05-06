using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// punto = 0; banco = 1; tie = 2;
// person = 0;

namespace PuntoBanco
{
    public struct SomeBet
    {
        public int money;
        public int man;
        public int target;
    }
    class PuntoBanco
    {
        public int[] cards;
        private int score;
        public PuntoBanco()
        {
            cards = new int[3];
            score = 0;
            for (int i = 0; i < cards.Length; ++i)
                cards[i] = 0;
        }
        public void getCard(int first, int second)
        {
            cards[0] = first;
            cards[1] = second;
            int firstScore = first % 100 < 10 ? first % 100 : 0;
            int secScore = second % 100 < 10 ? second % 100 : 0;
            score = (firstScore + secScore) % 10;
        }
        public void getCard(int first)
        {
            cards[2] = first;
            int firstScore = first % 100 < 10 ? first % 100 : 0;
            score = (firstScore + score) % 10;
        }
        public bool isNatural()
        {
            if (cards[2] == 0)
                return score > 7 ? true : false;
            return false;
        }
        public bool CanTake()
        {
            if (cards[2] == 0)
                return score < 6 ? true : false;
            return false;
        }

        public static bool operator >(PuntoBanco p, PuntoBanco b)
        {
            return p.score > b.score;
        }
        public static bool operator <(PuntoBanco p, PuntoBanco b)
        {
            return p.score < b.score;
        }

    }
    public class UserInterface
    {
        private int Bots = 2;
        private int [] deck;
        private int index = 0;
        private gamer [] gamers;
        private void indexIncr()
        {
            if (index + 1 == deck.Length)
            {
                initDeck(416);
                index = 0;
            }
            else
                index++;
        }
        private void initDeck(int count)
        {
            Random rnd = new Random();
            int randFirst = 0, randSec = 0;
            int eight = 1;
            int color = 0;
            int val = 1;
            deck = new int[count];
            for (int i = 0; i < count; ++i) 
            {
                color++;
                if (color > 4)
                {
                    color = 1;
                    val++;
                    if (val > 13)
                    {
                        val = 2;
                        eight++;
                    }
                }
                deck[i] = eight * 1000 + color * 100 + val;
            }
            for (int i = 0; i < count; ++i)
            {
                randFirst = rnd.Next(0, count);
                randSec = rnd.Next(0, count);
                int temp = deck[randFirst];
                deck[randFirst] = deck[randSec];
                deck[randSec] = temp;
            }
            Draw tabel = new Draw();
            tabel.initDeck();
        }
        private void getPlayers()
        {
            initDeck(416);
            gamers = new gamer[3];
            gamers[0] = new person();
            gamers[1] = new gamer1(20);
            gamers[2] = new gamer2(40);
        }
        private void Round()
        {
            Draw table = new Draw();
            table.preRound(gamers);
            SomeBet[] bets = new SomeBet[3];

            for (int i = 1; i <= Bots; ++i)
                bets[i] = gamers[i].makeBet();

            table.BotsBets(bets);

            bets[0] = gamers[0].makeBet();
            
            table.allBets(bets);

            PuntoBanco punto = new PuntoBanco();
            PuntoBanco banco = new PuntoBanco();

            if (index + 3 == deck.Length)
            {
                initDeck(416);
            }

            punto.getCard(deck[index], deck[index + 2]);
            banco.getCard(deck[index + 1], deck[index + 3]);
            table.showFirst(bets, deck[index], deck[index + 2], deck[index + 1], deck[index + 3]);
            index += 3;
            indexIncr();
            if ((!punto.isNatural()) && (!banco.isNatural()))
            {
                if (punto.CanTake())
                {
                    punto.getCard(deck[index]);
                    table.showSecond(bets, 0, deck[index]);
                    indexIncr();
                }
                else
                    table.enough(0);
                if (banco.CanTake())
                {
                    banco.getCard(deck[index]);
                    table.showSecond(bets, 1, deck[index]);
                    indexIncr();
                }
                else
                    table.enough(1);
            }
            else
            {
                table.Natural();
            }

            if (punto > banco)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (bets[i].target == 0)
                        gamers[bets[i].man].recive(bets[i].money * 2);
                }
                table.winner(0);
            }
            else if (banco > punto)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (bets[i].target == 1)
                        gamers[bets[i].man].recive(bets[i].money * 2);
                }
                table.winner(1);
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    if (bets[i].target == 2)
                        gamers[bets[i].man].recive(bets[i].money * 9);
                }
                table.winner(2);
            }

            table.preRound(gamers);

        }

        public void goGame()
        {
            getPlayers();
            Interaction inter = new Interaction();
            Draw user = new Draw();
            int end = 0;
            do
            {
                user.redyToGo();
                if (inter.ready() == true)
                {
                    Round();
                }
                else
                {
                    user.ShowMyMoney(gamers[0]);
                    end = 1;
                }
            } while (end == 0);
        }
    }
    abstract public class gamer
    {
        protected int startMoney;
        public int moneyMoment;
        public virtual SomeBet makeBet()
        {
            Random rnd = new Random();
            SomeBet betNow;
            betNow.money = rnd.Next(moneyMoment);
            moneyMoment -= betNow.money;
            betNow.man = -1;
            betNow.target = rnd.Next(4);
            return betNow;
        }
        public virtual void recive(int money)
        {
            moneyMoment += money;
        }
    }
    public class gamer1 : gamer
    {
        public gamer1(int money)
        {
            startMoney = money;
            moneyMoment = money;
        }
        public override SomeBet makeBet()
        {
            SomeBet betNow;
            betNow.man = 1;
            betNow.target = 0;
            betNow.money = (int)(0.125 * moneyMoment);
            moneyMoment -= betNow.money;
            return betNow;
        }
    }
    public class gamer2 : gamer
    {
        public gamer2(int money)
        {
            startMoney = money;
            moneyMoment = money;
        }
        public override SomeBet makeBet()
        {
            SomeBet betNow;
            betNow.man = 2;
            betNow.target = 1;
            betNow.money = (int)(0.25 * moneyMoment);
            moneyMoment -= betNow.money;
            return betNow;
        }
    }
    public class person : gamer
    {
        public person()
        {
            Interaction inter = new Interaction();
            Draw user = new Draw();
            user.askMoneyToGo();
            startMoney = inter.getInt();
            moneyMoment = startMoney;
        }
        public override SomeBet makeBet()
        {
            Interaction inter = new Interaction();
            Draw user = new Draw();
            user.doBet(moneyMoment);
            SomeBet betNow;
            betNow.man = 0;
            betNow.target = 0;
            betNow.money = 0;
            inter.doBet(ref betNow, moneyMoment);
            betNow.man = 0;
            moneyMoment -= betNow.money;
            return betNow;
        }
    }
    class Draw
    {
        private int[] puntos;
        private int[] bancos;
        private string color(int card)
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
        private string cost(int card)
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

        public void preRound(gamer []g)
        {
            int f = 0;
            foreach(var i in g)
            {
                Console.WriteLine($"index: {f}, money: {i.moneyMoment}");
                f++;
            }
        }
        public void winner(int who)
        {
            Console.Write("The winner is ");
            if (who == 0)
                Console.WriteLine("Punto");
            if (who == 1)
                Console.WriteLine("Banco");
            if (who == 2)
                Console.WriteLine("No one (tie)");
        }
        public void BotsBets(SomeBet []first)
        {

        }
        public void allBets(SomeBet []first)
        {

        }
        public void Natural()
        {

        }
        public void enough(int who)
        {
            if (who == 0)
                Console.WriteLine("Punto is enough");
            if (who == 1)
                Console.WriteLine("Banco is enough");
        }
        public void showFirst(SomeBet []first, int pf, int ps, int bf, int bs)
        {
            puntos = new int[2] { pf, ps };
            bancos = new int[2] { bf, bs };
            Console.WriteLine($"Punto's cards {color(pf)}{cost(pf)}-{pf}, {color(ps)}{cost(ps)}-{ps}");
            Console.WriteLine($"Banco's cards {color(bf)}{cost(bf)}-{bf}, {color(bs)}{cost(bs)}-{bs}");
        }
        public void showSecond(SomeBet[] first, int who, int card)
        {
            if (who == 0)
                Console.Write("Punto's");
            if (who == 1)
                Console.Write("Banco's");
            Console.WriteLine($" next card is {color(card)}{cost(card)}-{card}");
        }
        public void initDeck()
        {
            Console.WriteLine("new deck rotation");
        }

        public void askMoneyToGo()
        {
            Console.Write("how mutch money you will give to start the game: ");
        }

        internal void ShowMyMoney(gamer g)
        {
            Console.WriteLine($"Now you have {g.moneyMoment} $");
        }

        public void redyToGo()
        {
            Console.Write("If you want to play 1 Round type '1' else type any other integer(0): ");
        }

        public void doBet(int moneyMoment)
        {
            Console.WriteLine($"Now yo have {moneyMoment}$, type first target of your bet(0 = punto, 1 = banco; 2 = tie) press enter after target\nThen type sum of money\n stock target is 0, stock sum is {moneyMoment}\n");
        }
    }
    class Interaction
    {
        public int getInt()
        {
            return Int32.Parse(Console.ReadLine());
        }

        public bool ready()
        {
            if (Int32.Parse(Console.ReadLine()) == 1)
                return true;
            else
                return false;
        }

        public void doBet(ref SomeBet betNow, int moneyMoment)
        {
            int target = Int32.Parse(Console.ReadLine());
            if (target != 0)
                if (target != 1)
                    if (target != 2)
                        target = 0;
            betNow.target = target;
            int money = Int32.Parse(Console.ReadLine());
            if (money > moneyMoment)
                money = moneyMoment;
            betNow.money = money;
        }
    }
}
