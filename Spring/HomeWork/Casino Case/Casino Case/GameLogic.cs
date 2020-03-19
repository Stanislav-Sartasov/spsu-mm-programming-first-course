using System;
using System.Collections.Generic;
using System.Text;
namespace Casino_Case
{
    public class Game
    {

        public Player player = new Player();
        private Table table = new Table();
        public void setBalance(int m)
        {
            player.balance = m;
        }



        public void numberBet(int number, int bid)
        {
            if (number > 36 || number < 0)
                throw new NotImplementedException("Entered wrong number");
            if (bid > player.balance)
                throw new NotImplementedException("You dont have enough money");

            player.amountOfBets++;
            Random rnd = new Random();
            int value = rnd.Next(0, 36);
            Console.WriteLine("Winning number " + value);
            if (value == number)
            {
                //bid *= 35;
                Console.WriteLine("You've just won " + bid * 35);
                player.balance += (bid) * 35;
                player.profit += (bid) * 35;
            }
            else
            {
                Console.WriteLine("Didn't win :(");
                player.balance -= bid;
                player.profit -= bid;
            }
        }

        public void colorBet(int color, int bid)
        {
            if (color < 0 || color > 1)
                throw new NotImplementedException("Entered wrong color");
            if (bid > player.balance)
                throw new NotImplementedException("You dont have enough money");
            if (bid <= 0)
                throw new NotImplementedException("Bid can't be less than 1");
            player.amountOfBets++;
            Random rnd = new Random();
            int value = rnd.Next(0, 36);
            Console.WriteLine("Winning number " + value);
            if (table.wheel[value].color == color)
            {
                Console.WriteLine("You've just won " + bid);
                player.balance += bid;
                player.profit += bid;
            }
            else
            {
                Console.WriteLine("Didn't win :(");
                player.balance -= bid;
                player.profit -= bid;
            }
            Console.WriteLine();
        }

        public void parityBet(int Parity, int bid)
        {
            if (Parity < 0 || Parity > 1)
                throw new NotImplementedException("Entered wrong parity");
            if (bid > player.balance)
                throw new NotImplementedException("You dont have enough money");

            player.amountOfBets++;
            Random rnd = new Random();
            int value = rnd.Next(0, 36);
            Console.WriteLine("Winning number " + value);
            if (value % 2 == Parity)
            {
                //bid *= 35;
                Console.WriteLine("You've just won " + bid);
                player.balance += bid;
                player.profit += bid;
            }
            else
            {
                Console.WriteLine("Didn't win :(");
                player.balance -= bid;
                player.profit -= bid;
            }
        }

        public void dozonBet(int dozen, int bid)
        {
            if (dozen < 1 || dozen > 3)
                throw new NotImplementedException("Entered wrong dozen");
            if (bid > player.balance)
                throw new NotImplementedException("You dont have enough money");
            player.amountOfBets++;
            Random rnd = new Random();
            int value = rnd.Next(0, 36);
            Console.WriteLine("Winning number " + value);
            if (table.wheel[value].dozen == dozen)
            {
                Console.WriteLine("You've just won " + bid * 2);
                player.balance += (bid) * 2;
                player.profit += (bid) * 2;
            }
            else
            {
                Console.WriteLine("Didn't win :(");
                player.balance -= bid;
                player.profit -= bid;
            }
        }

        public void columnBet(int column, int bid)
        {
            if (column < 1 || column > 3)
                throw new NotImplementedException("Entered wrong column");
            if (bid > player.balance)
                throw new NotImplementedException("You dont have enough money");
            player.amountOfBets++;
            Random rnd = new Random();
            int value = rnd.Next(0, 36);
            Console.WriteLine("Winning number " + value);
            if (table.wheel[value].column == column)
            {
                Console.WriteLine("You've just won " + bid * 2);
                player.balance += (bid) * 2;
                player.profit += (bid) * 2;
            }
            else
            {
                Console.WriteLine("Didn't win :(");
                player.balance -= bid;
                player.profit -= bid;
            }
        }


    }
}

