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

        public abstract void SetBet(int maxBet);
        public void DisplayInfo()
        {
            Console.WriteLine($"^^^^^^^^^\nName - {PlayerName}\n" + $"Amount of money - {PlayerCash}\n^^^^^^^^^^");
        }
    }
}
