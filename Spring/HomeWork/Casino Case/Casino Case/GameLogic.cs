using System;
using System.Collections.Generic;
using System.Text;
namespace Casino_Case
{
    public class Game
    {

        protected internal int balance;
        protected internal int profit = 0;
        protected internal int amountOfBets = 0;

        private Table table = new Table();
        public void SetBalance(int m)
        {
            this.balance = m;
        }



        public void NumberBet(int number, int bid)
        {
            if (number > 36 || number < 0)
                throw new NotImplementedException("Entered wrong number");
            InputCheking(bid, this.balance);

            this.amountOfBets++;
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
                throw new NotImplementedException("Entered wrong color");
            InputCheking(bid, this.balance);

            this.amountOfBets++;
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
                throw new NotImplementedException("Entered wrong parity");
            InputCheking(bid, this.balance);

            this.amountOfBets++;
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
                throw new NotImplementedException("Entered wrong dozen");
            InputCheking(bid, this.balance);

            this.amountOfBets++;
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
                throw new NotImplementedException("Entered wrong column");
            InputCheking(bid, this.balance);

            this.amountOfBets++;
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


       private void InputCheking(int t_Bid, int t_Balance)
        {
            if (t_Bid > t_Balance)
                throw new NotImplementedException("You dont have enough money");
            if (t_Bid <= 0)
                throw new NotImplementedException("Bid can't be less than 1");
        }
        private void OutputWin (int t_Bid,int t_Coef)
        {
            Console.WriteLine("You've just won " + t_Bid * t_Coef);
            this.balance += (t_Bid) * t_Coef;
            this.profit += (t_Bid) * t_Coef;
        }
        private void OutputLose(int t_Bid)
        {
            Console.WriteLine("Didn't win :(");
            this.balance -= t_Bid;
            this.profit -= t_Bid;
        }

    }


}

