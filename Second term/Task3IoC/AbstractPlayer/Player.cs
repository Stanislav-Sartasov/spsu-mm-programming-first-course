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
            wallet = CorrectInput.CorrectInput.IntInput(wallet);

        }
        public override void Bet()
        {
            choice = 0;
            while (!(choice > 0 && choice < 9))
            {
                Console.WriteLine("\nMenu:\n1.Red\n2.Black\n3.Odd\n4.Even\n5.1-12\n6.13-24\n7.25-36\n8.Straight up\n" +
                    "\nChoose bet number");

                choice = CorrectInput.CorrectInput.IntInput(choice);
                if (!(choice > 0 && choice < 9))
                    Console.WriteLine("Incorrect input!");
            }
            
            if (choice == 8)
            {
                Console.WriteLine("Enter cell number");
                cell = CorrectInput.CorrectInput.IntInput(cell);
            }
            Console.WriteLine("Enter your current bet: ");

            currentBet = CorrectInput.CorrectInput.IntInput(currentBet);

            while (currentBet > wallet)
            {
                Console.WriteLine("You don't have enough money. Enter your current bet: ");
                currentBet = CorrectInput.CorrectInput.IntInput(currentBet);
            }
        }

    }
}
