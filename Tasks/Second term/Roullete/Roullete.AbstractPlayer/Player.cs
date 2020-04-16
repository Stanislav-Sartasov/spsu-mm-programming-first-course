using System;
using System.Collections.Generic;
using System.Text;

namespace Roullete.Players
{
    public class Player : AbstractPlayer
    {
        public Player()
        {
            Console.WriteLine("Enter your name");
            playerName = Console.ReadLine();
            Console.WriteLine("Enter amount of your money");
            playerCash = int.Parse(Console.ReadLine());
        }

        public override void SetBet(int maxBet)
        {
            Console.WriteLine("Select the amount of money you want to bet");
            currentCashBet = int.Parse(Console.ReadLine());
            while (CurrentCashBet > Math.Min(maxBet, PlayerCash))
            {
                Console.WriteLine("Bet too high, select another");
                currentCashBet = int.Parse(Console.ReadLine());
            }
            Console.WriteLine("Select the field you want to put on: Zero, Tier 1-3, Color (Black or Red), Parity (Even or Odd), Number 1-36 (just a number)");
            currentBet = Console.ReadLine();
        }
        
    }
}
