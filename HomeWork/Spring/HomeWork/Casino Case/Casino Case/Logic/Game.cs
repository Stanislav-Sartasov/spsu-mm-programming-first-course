using System;
using System.Collections.Generic;
using System.Text;
namespace Casino_Case
{
    public class Game
    {

        protected internal int balance;
        protected internal int profit = 0;
        protected internal int AmountOfBets = 0;

        private Table table = new Table();
        public void SetBalance(int m)
        {
            this.balance = m;
        }



        public void NumberBet(int number, int bid)
        {
            if (number > 36 || number < 0)
                throw new ArgumentException("Entered wrong number");
            InputChecking(bid, this.balance);

            this.AmountOfBets++;
            Random rnd = new Random();
            int value = rnd.Next(0, 36);
            Console.WriteLine("Winning number " + value);
            if (value == number)
            {
                OutputWin(bid, 35);
            }
            else
            {
                OutputLose(bid);
            }
        }

        public void ColorBet(int color, int bid)
        {
            if (color < 0 || color > 1)
                throw new ArgumentException("Entered wrong color");
            InputChecking(bid, this.balance);

            this.AmountOfBets++;
            Random rnd = new Random();
            int value = rnd.Next(0, 36);
            Console.WriteLine("Winning number " + value);
            if (table.Wheel[value].Color == (Table.Colors) Convert.ToInt16(color))
            {
                OutputWin(bid, 1);
            }
            else
            {
                OutputLose(bid);
            }
            Console.WriteLine();
        }

        public void ParityBet(int parity, int bid)
        {
            if (parity < 0 || parity > 1)    // чет нечет
                throw new ArgumentException("Entered wrong parity");
            InputChecking(bid, this.balance);

            this.AmountOfBets++;
            Random rnd = new Random();
            int value = rnd.Next(0, 36);
            Console.WriteLine("Winning number " + value);
            if (value % 2 == parity)
            {
                OutputWin(bid, 1);
            }
            else
            {
                OutputLose(bid);
            }
        }

        public void DozonBet(int dozen, int bid)
        {
            if (dozen < 1 || dozen > 3)
                throw new ArgumentException("Entered wrong dozen");
            InputChecking(bid, this.balance);

            this.AmountOfBets++;
            Random rnd = new Random();
            int value = rnd.Next(0, 36);
            Console.WriteLine("Winning number " + value);
            if (table.Wheel[value].Dozen == (Table.Dozens) Convert.ToInt16(dozen))
            {
                OutputWin(bid, 2);
            }
            else
            {
                OutputLose(bid);
            }
        }

        public void ColumnBet(int column, int bid)
        {
            if (column < 1 || column > 3)
                throw new ArgumentException("Entered wrong column");
            InputChecking(bid, this.balance);

            this.AmountOfBets++;
            Random rnd = new Random();
            int value = rnd.Next(0, 36);
            Console.WriteLine("Winning number " + value);
            if (table.Wheel[value].Column == column)
            {
                OutputWin(bid, 2);
            }
            else
            {
                OutputLose(bid);
            }
        }


       private void InputChecking(int TBid, int TBalance)
        {
            if (TBid > TBalance)
                throw new Exception("You dont have enough money");
            if (TBid <= 0)
                throw new Exception("Bid can't be less than 1");
        }
        private void OutputWin (int TBid,int TCoef)
        {
            Console.WriteLine("You've just won " + TBid * TCoef);
            this.balance += (TBid) * TCoef;
            this.profit += (TBid) * TCoef;
        }
        private void OutputLose(int TBid)
        {
            Console.WriteLine("Didn't win :(");
            this.balance -= TBid;
            this.profit -= TBid;
        }

    }


}

