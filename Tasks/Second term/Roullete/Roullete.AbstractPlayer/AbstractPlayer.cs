using System;
using System.Collections.Generic;

namespace Roullete.Players
{
    public abstract class AbstractPlayer
    {
        protected int playerCash;
        public int PlayerCash => playerCash;

        protected string playerName;
        public string PlayerName => playerName;

        protected int currentCashBet;
        public int CurrentCashBet => currentCashBet;

        protected string currentBet;
        public string CurrentBet => currentBet;

        public abstract void SetBet(int maxBet);

        public virtual void UpdateInformation(List<string> info, int deltaMoney)
        {
            playerCash = deltaMoney;
        }

        public void DisplayInfo()
        {
            Console.WriteLine($"^^^^^^^^^\nName - {PlayerName}\n" + $"Amount of money - {PlayerCash}\n^^^^^^^^^^");
        }
    }
}
