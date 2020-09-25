using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuntoBanco
{
    public interface IDraw
    {
        //private int[] puntos;
        //private int[] bancos;
        string Color(int card);
        string Cost(int card);
        void PreRound(Gamer[] g);
        void Winner(int who);
        void BotsBets(SomeBet[] first);
        void AllBets(SomeBet[] first);
        void Natural();
        void Enough(int who);
        void ShowFirst(SomeBet[] first, int pf, int ps, int bf, int bs);
        void ShowSecond(SomeBet[] first, int who, int card);
        void InitDeck();
        void AskMoneyToGo();
        void ShowMyMoney(Gamer g);
        void RedyToGo();
        void DoBet(int moneyMoment);
        void IntError();

    }
}
