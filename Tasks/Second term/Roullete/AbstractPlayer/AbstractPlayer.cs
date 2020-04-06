using System;
using System.Collections.Generic;

namespace Players
{
    public abstract class AbstractPlayer
    {
        public int PlayerCash;
        public string PlayerName;
        public int CurrentCashBet;
        public string CurrentBet;
        public string PreviousBet;
        public byte CurrentGameResult;
        public List<byte> PreviousGamesResult; // 0 - lose, 1  - win, 2 - first game

        public void DisplayInfo()
        {
            Console.WriteLine($"^^^^^^^^^\nName - {PlayerName}\n" + $"Amount of money - {PlayerCash}\n^^^^^^^^^^");
        }

        //public virtual void SetBet()
        //{
        //    CurrentCashBet = ChooseCashBet();
        //    CurrentBet = ChooseFieldBet();
        //}
        //public virtual int ChooseCashBet()
        //{
        //    return 0;
        //}
        //public virtual string ChooseFieldBet()
        //{
        //    return "None";
        //}
    }
}
