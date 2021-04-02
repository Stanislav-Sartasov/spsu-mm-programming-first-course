using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Roulette
{
	public class Player:APlayer
	{
		public Player()
		{
			Console.WriteLine("Enter your name: ");
			name = Console.ReadLine();
			Console.WriteLine("Enter your starting capital: ");
			cash = InputCheck.IntCheck(cash);
		}
		public override void Bet()
		{
			if (cash < 100)
			{
				Console.WriteLine("The minimum bet is one hundred. You don't have enough money to bet.");
				return;
			}
			typeOfBet = 0;
			int fcell, scell;
			fcell = scell = 0;
			while (typeOfBet < 1 || typeOfBet > 11)
			{
				Console.WriteLine("Enter the number of the type of your bet: \n1.Single\n2.Split" +
					"\n3.Basket\n4.Red\n5.Black\n6.Even\n7.Odd\n8.First Dozen" +
					"\n9.Second Dozen\n10.Third Dozen\n11.Snake Bet");
				typeOfBet = InputCheck.IntCheck(typeOfBet);
				if (typeOfBet < 1 || typeOfBet > 11)
				{
					Console.WriteLine("Invalid input. Check the list again.");
				}
			}
			switch (typeOfBet)
			{
				case 1:
					Console.WriteLine("Enter slot number:");
					cells = InputCheck.GetIntoSlot(cells);
					break;
				case 2:
					while (Math.Abs(fcell - scell) != 1)
					{
						Console.WriteLine("Enter two consecutive numbers using the enter key:");
						fcell = InputCheck.GetIntoSlot(fcell);
						scell = InputCheck.GetIntoSlot(scell);
						cells = Math.Min(fcell, scell);
					}
					break;
				case 3:
					while (Math.Abs(fcell - scell) != 1)
					{
						Console.WriteLine("Your bet will be on 2 consecutive numbers and zero.\nEnter two consecutive numbers using the enter key:");
						fcell = InputCheck.GetIntoSlot(fcell);
						scell = InputCheck.GetIntoSlot(scell);
						cells = Math.Min(fcell, scell);
					}
					break;
				case 11:
					Console.WriteLine("A bet on the following combination of red cells: 1-5-9-12-14-16-19-23-27-30-32-34");
					break;
				default:
					break;
			}
			do
			{
				Console.WriteLine("Enter bet amount:");
				currentBet = InputCheck.IntCheck(currentBet);
				if (currentBet < 100)
					Console.WriteLine("The minimum bet is one hundred.");
				else if (currentBet > cash)
					Console.WriteLine("Insufficient funds on the account.");
			} while (currentBet < 100 || currentBet > cash);
		}
	}
}
