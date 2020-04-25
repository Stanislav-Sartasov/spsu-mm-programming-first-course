using System;

namespace Action
{
    public class GameProcess
    {
        
        
        public int[] red = { 1, 3, 5, 7, 9, 12, 14, 16, 18, 19, 21, 23, 25, 27, 30, 32, 34, 36 };
        public int[] black = { 2, 4, 6, 8, 10, 11, 13, 15, 17, 20, 22, 24, 26, 28, 29, 31, 33, 35 };

        public int SpinWheel()
        {
            Random rnd = new Random();
            return rnd.Next(37);
        }
        
        public int Payout(int wallet, string choice, int bet)
        {
            

            int cell = SpinWheel();
            
            switch (choice)
            {
                case ("Red"):
                    if (Array.IndexOf(red, cell) != -1)
                    {
                        wallet += bet;
                    }
                    else
                    {
                        wallet -= bet;
                    }
                    break;

                case ("Black"):
                    if (Array.IndexOf(black, cell) != -1)
                    {
                        wallet += bet;
                    }
                    else
                    {
                        wallet -= bet;
                    }
                    break;

                case ("Even"):
                    if (cell % 2 == 0) wallet += bet;
                    else wallet -= bet;
                    break;

                case ("Odd"):
                    if (cell % 2 == 1) wallet += bet;
                    else wallet -= bet;
                    break;

                case ("First Dozen"):
                    if (cell < 13 && cell > 0) wallet += 2 * bet;
                    else wallet -= bet;
                    break;

                case ("Second Dozen"):
                    if (cell < 25 && cell > 12) wallet += 2 * bet;
                    else wallet -= bet;
                    break;

                case ("Third Dozen"):
                    if (cell < 37 && cell > 24) wallet += 2 * bet;
                    else wallet -= bet;
                    break;

                default:
                    int result;
                    if (int.TryParse(choice, out result))
                    {
                        if (result == cell) wallet += 35 * bet;
                        else wallet -= bet;
                        break;
                    }
                    else break;
            }
            
            return wallet;
        }

        public int Result(int wallet, string choice, int bet)
        {
            Console.WriteLine("Bets made, no more bets.");
            int previousWallet = wallet;
            int correntWallet = Payout(wallet, choice, bet);
            if (correntWallet > previousWallet)
                Console.WriteLine($"Congratilations! You won {correntWallet - previousWallet}$! Your balance: {correntWallet}$");
            else Console.WriteLine($"Better luck next time! Your balance: {correntWallet}$");
            return correntWallet;
        }
        
    }
}
