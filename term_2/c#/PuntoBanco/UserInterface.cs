using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuntoBanco
{
    public class UserInterface
    {
        private int Bots = 2;
        private int[] deck;
        private int index = 0;
        private Gamer[] gamers;
        private IInteraction inter;
        public UserInterface(IInteraction YourInter)
        {
            index = 0;
            Bots = 2;
            inter = YourInter;
        }
        public UserInterface()
        {
            index = 0;
            Bots = 2;
            inter = new Interaction(new Draw());
        }
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
            tabel.InitDeck();
        }
        private void getPlayers()
        {
            initDeck(416);
            gamers = new Gamer[3];
            gamers[0] = new Person(inter);
            gamers[1] = new GamerFirst(20);
            gamers[2] = new GamerSecond(40);
        }
        private void Round()
        {
            Draw table = new Draw();
            table.PreRound(gamers);
            SomeBet[] bets = new SomeBet[3];

            for (int i = 1; i <= Bots; ++i)
                bets[i] = gamers[i].MakeBet();

            table.BotsBets(bets);

            bets[0] = gamers[0].MakeBet();

            table.AllBets(bets);

            PuntoBanco punto = new PuntoBanco();
            PuntoBanco banco = new PuntoBanco();
            int card1, card2, card3, card4;
            card1 = deck[index];
            indexIncr();
            card2 = deck[index];
            indexIncr();
            card3 = deck[index];
            indexIncr();
            card4 = deck[index];
            indexIncr();


            punto.getCard(card1, card3);
            banco.getCard(card2, card4);
            table.ShowFirst(bets, card1, card3, card2, card4);

            if ((!punto.isNatural()) && (!banco.isNatural()))
            {
                if (punto.CanTake())
                {
                    punto.getCard(deck[index]);
                    table.ShowSecond(bets, 0, deck[index]);
                    indexIncr();
                }
                else
                    table.Enough(0);
                if (banco.CanTake())
                {
                    banco.getCard(deck[index]);
                    table.ShowSecond(bets, 1, deck[index]);
                    indexIncr();
                }
                else
                    table.Enough(1);
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
                        gamers[bets[i].man].Recive(bets[i].money * 2);
                }
                table.Winner(0);
            }
            else if (banco > punto)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (bets[i].target == 1)
                        gamers[bets[i].man].Recive(bets[i].money * 2);
                }
                table.Winner(1);
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    if (bets[i].target == 2)
                        gamers[bets[i].man].Recive(bets[i].money * 9);
                }
                table.Winner(2);
            }

            table.PreRound(gamers);

        }

        public void goGame()
        {
            getPlayers();
            Draw user = new Draw();
            int end = 0;
            do
            {
                user.RedyToGo();
                if (inter.Ready() == true)
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

}
