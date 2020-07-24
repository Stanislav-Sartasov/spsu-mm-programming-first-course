using Casino_Case.Bot;
using System;
using System.Collections.Generic;
using System.Text;

namespace Casino_Case
{
    public class Menu
    {
        Game Gaming = new Game();
        MartingaleBot Martingale = new MartingaleBot();
        DalamberBot Dalamber = new DalamberBot();
        int choice = 0;
        int choice2;
        bool exit = false;
        int BetAmount;
        int BetOn;
        int BotChoice;
        int MinBid;
        int balance = 0;
        public void MainMenu()
        {
            for (; ; )
            {
                if (exit)
                    break;
                Console.WriteLine("1 - " + "Start");
                Console.WriteLine("2 - " + "Exit");
                choice = Convert.ToInt32(Console.ReadLine());


                if (choice == 1)
                {
                    Console.Clear();
                    Console.WriteLine("1 - " + "Play by yourself");
                    Console.WriteLine("2 - " + "Bots");
                    Console.WriteLine("3 - " + "Exit");
                    choice = Convert.ToInt32(Console.ReadLine());
                    if (choice == 1)
                    {
                        Console.Clear();
                        while (balance <= 0)
                        {
                            Console.Clear();
                            Console.WriteLine("Inter you balance (more than 0)");
                            balance = Convert.ToInt32(Console.ReadLine());
                        }


                        Gaming.SetBalance(balance);
                        Console.Clear();
                        BettingMenu();

                    }

                    else if (choice == 2)
                    {
                        //Console.Clear();
                        bool right = false;
                        while (right == false)
                        {
                            Console.Clear();
                            Console.WriteLine("What bot do you wanna play?\n 1 - Dalamber \n 2 - Martingale ");
                            BotChoice = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("Enter bot balance, minimum bid and amount of bets");
                            balance = Convert.ToInt32(Console.ReadLine());
                            MinBid = Convert.ToInt32(Console.ReadLine());
                            BetAmount = Convert.ToInt32(Console.ReadLine());
                            if ((BotChoice == 1 || BotChoice == 2) && balance > 0 && MinBid > 0 && BetAmount > 0)
                                right = true;
                        }
                        Dalamber.SetBalance(balance);
                        Martingale.SetBalance(balance);
                        if (BotChoice == 1)
                            Dalamber.Action(BetAmount, MinBid);
                        else
                            Martingale.Action(BetAmount, MinBid);

                    }

                    else if (choice == 3)
                    {
                        Console.Clear();
                        break;
                    }
                    else
                        Console.Clear();


                }
                else if (choice == 2)
                {
                    Console.Clear();
                    break;
                }
                else
                    Console.Clear();
            }
        }

        public void BettingMenu()
        {
            for (; ; )
            {

                Console.WriteLine("1 - " + "Stright Bet" + "                Info:");
                Console.WriteLine("2 - " + "Red or Black Bet" + "           Balance: " + Gaming.balance);
                Console.WriteLine("3 - " + "Even or Odd Bet" + "            Amount of bets: " + Gaming.AmountOfBets);
                Console.WriteLine("4 - " + "Dozen Bet" + "                  Total Profit: " + Gaming.profit);
                Console.WriteLine("5 - " + "Column Bet");
                Console.WriteLine("6 - " + "Exit");
                choice2 = Convert.ToInt32(Console.ReadLine());
                if (choice2 == 1)
                {
                    Console.Clear();
                    Console.WriteLine("Enter the numbers you want to bet on and bet amount");
                    BetOn = Convert.ToInt32(Console.ReadLine());
                    BetAmount = Convert.ToInt32(Console.ReadLine());
                    Console.Clear();
                    Gaming.NumberBet(BetOn, BetAmount);
                }

                else if (choice2 == 2)
                {
                    Console.Clear();
                    Console.WriteLine("Enter the color you want to bet(0 is red, 1 is red) on and bet amount");
                    BetOn = Convert.ToInt32(Console.ReadLine());
                    BetAmount = Convert.ToInt32(Console.ReadLine());
                    Console.Clear();
                    Gaming.ColorBet(BetOn, BetAmount);
                }
                else if (choice2 == 3)
                {
                    Console.Clear();
                    Console.WriteLine("Enter the parity you want to bet(0 - even, 1 - odd) on and bet amount");
                    BetOn = Convert.ToInt32(Console.ReadLine());
                    BetAmount = Convert.ToInt32(Console.ReadLine());
                    Console.Clear();
                    Gaming.ParityBet(BetOn, BetAmount);
                }
                else if (choice2 == 4)
                {
                    Console.Clear();
                    Console.WriteLine("Enter the dozen you want to bet on and bet amount");
                    BetOn = Convert.ToInt32(Console.ReadLine());
                    BetAmount = Convert.ToInt32(Console.ReadLine());
                    Console.Clear();
                    Gaming.DozonBet(BetOn, BetAmount);
                }

                else if (choice2 == 5)
                {
                    Console.Clear();
                    Console.WriteLine("Enter the column you want to bet on and bet amount");
                    BetOn = Convert.ToInt32(Console.ReadLine());
                    BetAmount = Convert.ToInt32(Console.ReadLine());
                    Console.Clear();
                    Gaming.ColumnBet(BetOn, BetAmount);
                }


                else if (choice2 == 6)
                {
                    Console.Clear();
                    exit = true;
                    break;
                }
                else
                {
                    Console.Clear();
                }


            }
        }
    }


}
