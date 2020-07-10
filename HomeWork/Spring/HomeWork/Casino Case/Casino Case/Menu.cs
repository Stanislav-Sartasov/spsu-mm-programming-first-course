using System;
using System.Collections.Generic;
using System.Text;

namespace Casino_Case
{
    public class Menu
    {
        Game Gaming = new Game();
        Bots NewBots = new Bots();
        int choice = 0;
        int choice2;
        bool exit = false;
        int betAmount;
        int betOn;
        int botChoice;
        int minBid;
        int balance = 0;
        public void mainMenu()
        {
            for (; ; )
            {
                if (exit == true)
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
                        bettingMenu();

                    }

                    else if (choice == 2)
                    {
                        //Console.Clear();
                        bool right = false;
                        while (right == false)
                        {
                            Console.Clear();
                            Console.WriteLine("What bot do you wanna play?\n 1 - Dalamber \n 2 - Martingale ");
                            botChoice = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("Enter bot balance, minimum bid and amount of bets");
                            balance = Convert.ToInt32(Console.ReadLine());
                            minBid = Convert.ToInt32(Console.ReadLine());
                            betAmount = Convert.ToInt32(Console.ReadLine());
                            if ((botChoice == 1 || botChoice == 2) && balance > 0 && minBid > 0 && betAmount > 0)
                                right = true;
                        }
                        NewBots.SetBalance(balance);
                        if (botChoice == 1)
                            NewBots.DalamberBot(betAmount, minBid);
                        else
                            NewBots.MartingaleBot(betAmount, minBid);

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

        public void bettingMenu()
        {
            for (; ; )
            {

                Console.WriteLine("1 - " + "Stright Bet" + "                Info:");
                Console.WriteLine("2 - " + "Red or Black Bet" + "           Balance: " + Gaming.balance);
                Console.WriteLine("3 - " + "Even or Odd Bet" + "            Amount of bets: " + Gaming.amountOfBets);
                Console.WriteLine("4 - " + "Dozen Bet" + "                  Total Profit: " + Gaming.profit);
                Console.WriteLine("5 - " + "Column Bet");
                Console.WriteLine("6 - " + "Exit");
                choice2 = Convert.ToInt32(Console.ReadLine());
                if (choice2 == 1)
                {
                    Console.Clear();
                    Console.WriteLine("Enter the numbers you want to bet on and bet amount");
                    betOn = Convert.ToInt32(Console.ReadLine());
                    betAmount = Convert.ToInt32(Console.ReadLine());
                    Console.Clear();
                    Gaming.NumberBet(betOn, betAmount);
                }

                else if (choice2 == 2)
                {
                    Console.Clear();
                    Console.WriteLine("Enter the color you want to bet(0 is red, 1 is red) on and bet amount");
                    betOn = Convert.ToInt32(Console.ReadLine());
                    betAmount = Convert.ToInt32(Console.ReadLine());
                    Console.Clear();
                    Gaming.ColorBet(betOn, betAmount);
                }
                else if (choice2 == 3)
                {
                    Console.Clear();
                    Console.WriteLine("Enter the parity you want to bet(0 - even, 1 - odd) on and bet amount");
                    betOn = Convert.ToInt32(Console.ReadLine());
                    betAmount = Convert.ToInt32(Console.ReadLine());
                    Console.Clear();
                    Gaming.ParityBet(betOn, betAmount);
                }
                else if (choice2 == 4)
                {
                    Console.Clear();
                    Console.WriteLine("Enter the dozen you want to bet on and bet amount");
                    betOn = Convert.ToInt32(Console.ReadLine());
                    betAmount = Convert.ToInt32(Console.ReadLine());
                    Console.Clear();
                    Gaming.DozonBet(betOn, betAmount);
                }

                else if (choice2 == 5)
                {
                    Console.Clear();
                    Console.WriteLine("Enter the column you want to bet on and bet amount");
                    betOn = Convert.ToInt32(Console.ReadLine());
                    betAmount = Convert.ToInt32(Console.ReadLine());
                    Console.Clear();
                    Gaming.ColumnBet(betOn, betAmount);
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
