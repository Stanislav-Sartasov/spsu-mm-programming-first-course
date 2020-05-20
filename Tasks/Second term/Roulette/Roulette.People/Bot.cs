using System;
using System.Collections.Generic;
using System.Text;

namespace Roulette.People
{
    public class Bot : AbstractPlayer
    {
        private int numberOfStrategy;
        private byte previousGamesResult;
        private int previousCashBet;
        private int firstBet;
        private static Random brain = new Random();
        private char[] previousBet = new char[] { 'f', 'f' };
        public Bot(int level)
        {
            amountMoney = 50_000;
            switch (level)
            {
                case 1:
                    {
                        playerName = "Martingejl";
                        numberOfStrategy = 1;
                        previousGamesResult = 2;
                        break;
                    }
                case 2:
                    {
                        playerName = "D'Alamber";
                        numberOfStrategy = 2;
                        previousGamesResult = 2;
                        break;
                    }
                default:
                    {
                        playerName = "Rich beginner";
                        numberOfStrategy = 3;
                        break;
                    }
            }
        }

        public override void SetBet(int maxBet)
        {
            switch (numberOfStrategy)
            {
                case 1:
                    {
                        MartingejlBet(maxBet);
                        break;
                    }
                case 2:
                    {
                        DalamberBet(maxBet);
                        break;
                    }
                case 3:
                    {
                        RandomBet(maxBet);
                        break;
                    }
            }
        }

        public override void MoneyRecount(bool roundResult, int multiplier, int maxSum)
        {
            base.MoneyRecount(roundResult, multiplier, maxSum);
            if (roundResult)
                previousGamesResult = 1;
            else
                previousGamesResult = 0;
        }

        private void RandomBet(int maxBet)
        {
            sumBet = Math.Min(brain.Next(1, amountMoney + 1), maxBet);
            switch (brain.Next(1, 5))
            {
                case 1: // Number, zero
                    {
                        int x = brain.Next(0, 37);
                        bet = (x / 10 == 0 ? new char[] { '0', x.ToString()[0] } : x.ToString().ToCharArray());
                        break;
                    }
                case 2: // color
                    {
                        switch (brain.Next(1, 3))
                        {
                            case 1:
                                {
                                    bet = new char[] { 'R', 'E' };
                                    break;
                                }
                            case 2:
                                {
                                    bet = new char[] { 'B', 'L' };
                                    break;
                                }
                        }
                        break;
                    }
                case 3: // parity
                    {
                        switch (brain.Next(1, 3))
                        {
                            case 1:
                                {
                                    bet = new char[] { 'O', 'D' };
                                    break;
                                }
                            case 2:
                                {
                                    bet = new char[] { 'E', 'V' };
                                    break;
                                }
                        }
                        break;
                    }
                case 4: // tier
                    {
                        int x = brain.Next(0, 37);
                        bet = new char[] { 'T', ((x) / 12 + 1).ToString()[0] };
                        break;
                    }
            }
        }

        private void DalamberBet(int maxBet)
        {
            if (previousGamesResult == 0)
            {
                sumBet = Math.Min(previousCashBet + 10, maxBet);
                previousCashBet = sumBet;
            }
            else if (previousGamesResult == 1)
            {
                sumBet = Math.Min(Math.Max(firstBet, previousCashBet - 10), maxBet);
                previousCashBet = sumBet;
            }
            else
            {
                sumBet = Math.Min(maxBet, amountMoney / 20);
                previousCashBet = sumBet;
                firstBet = sumBet;
            }
            if (previousGamesResult == 2)
            {
                switch (brain.Next(1, 5))
                {
                    case 1:
                        {
                            bet = new char[] { 'R', 'E' };
                            break;
                        }
                    case 2:
                        {
                            bet = new char[] { 'B', 'L' };
                            break;
                        }
                    case 3:
                        {
                            bet = new char[] { 'O', 'D' };
                            break;
                        }
                    case 4:
                        {
                            bet = new char[] { 'E', 'V' };
                            break;
                        }
                }
                bet.CopyTo(previousBet, 0);
            }
            else
                previousBet.CopyTo(bet, 0);
        }

        private void MartingejlBet(int maxBet)
        {
            if (previousGamesResult == 0)
            {
                sumBet = Math.Min(previousCashBet * 2, maxBet);
                previousCashBet = sumBet;
            }
            else if (previousGamesResult == 1)
            {
                sumBet = Math.Min(firstBet, maxBet);
                previousCashBet = sumBet;
            }
            else
            {
                sumBet = Math.Min(maxBet, amountMoney / 1000);
                previousCashBet = sumBet;
                firstBet = sumBet;
            }
            switch (brain.Next(1, 5))
            {
                case 1:
                    {
                        bet = new char[] { 'R', 'E' };
                        break;
                    }
                case 2:
                    {
                        bet = new char[] { 'B', 'L' };
                        break;
                    }
                case 3:
                    {
                        bet = new char[] { 'O', 'D' };
                        break;
                    }
                case 4:
                    {
                        bet = new char[] { 'E', 'V' };
                        break;
                    }
            }
        }
    }
}
