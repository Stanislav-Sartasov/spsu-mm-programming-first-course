using System;
using System.Collections.Generic;
using System.Text;


namespace AbstractPlayer
{
    public class Player : AbstractPlayer
    {
        public Player()
        {
            Console.WriteLine("Enter your nickname: ");
            name = Console.ReadLine();
            Console.WriteLine("Enter your balance: ");
            wallet = int.Parse(Console.ReadLine());
            
        }
        public override void Bet()
        {
            
            Console.WriteLine("Choose bet name or enter cell number:\n" +
                "Red\nBlack\nEven\nOdd\nFirst Dozen\nSecond Dozen\nThird Dozen");
            betName = Console.ReadLine();
            
            Console.WriteLine("Enter your current bet: ");
            currentBet = int.Parse(Console.ReadLine());
            while (currentBet > wallet)
            {
                Console.WriteLine("You don't have enough money. Enter your current bet: ");
                currentBet = int.Parse(Console.ReadLine());
            }
        }

    }
}
