using System;
using System.Collections.Generic;
using System.Text;

namespace Players
{
    public class Player : AbstractPlayer
    {
        public void Initialization()
        {
            Console.WriteLine("Enter your name");
            PlayerName = Console.ReadLine();
            Console.WriteLine("Enter amount of your money");
            PlayerCash = int.Parse(Console.ReadLine());
            CurrentGameResult = 0;
            PreviousGamesResult = new List<byte> { 2 };
        }

        // Example of bet:
        // Zero
        // 0 // equivalent to zero
        // Tier 2
        // 24
        // Even
        // Black
        public override void SetBet(int maxBet)
        {
            Console.WriteLine("Select the amount of money you want to bet");
            CurrentCashBet = int.Parse(Console.ReadLine());
            while (CurrentCashBet > Math.Min(maxBet, PlayerCash))
            {
                Console.WriteLine("Bet too high, select another");
                CurrentCashBet = int.Parse(Console.ReadLine());
            }
            Console.WriteLine("Select the field you want to put on: Zero, Tier 1-3, Color (Black or Red), Parity (Even or Odd), Number 1-36 (just a number)");
            CurrentBet = Console.ReadLine();
        }
        
    }
}
