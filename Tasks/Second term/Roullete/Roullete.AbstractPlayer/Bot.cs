using System;
using System.Collections.Generic;
using System.Text;

namespace Roullete.Players
{
    public class Bot : AbstractPlayer
    {
        private int previousCashBet;
        private byte logicLevel;
        private static Random brain = new Random();
        private Random humanFactor = new Random();
        private List<byte> previousGamesResult; // 0 - lose, 1  - win, 2 - first game
        private string previousBet;

        public Bot(byte botLevel)
        {
            switch (botLevel)
            {
                case 1:
                    {
                        playerName = "Martingejl";
                        playerCash = 10_000;
                        logicLevel = 30;
                        previousGamesResult = new List<byte> { 2 };
                        break;
                    }
                case 2:
                    {
                        playerName = "D'Alamber";
                        playerCash = 10_000;
                        logicLevel = 20;
                        previousGamesResult = new List<byte> { 2 };
                        break;
                    }
                default:
                    {
                        playerName = "Rich beginner";
                        playerCash = 1_000_000;
                        logicLevel = 10;
                        previousGamesResult = new List<byte> { 2 };
                        break;
                    }
            }
        }

        public override void SetBet(int maxBet)
        {
            switch (logicLevel / 10)
            {
                case 1:
                    {
                        currentCashBet = Math.Min(brain.Next(1, PlayerCash / logicLevel + humanFactor.Next(0, Math.Abs((previousGamesResult[0] % 2) * humanFactor.Next(0, PlayerCash / 100 - logicLevel)))), PlayerCash);
                        ChooseBigField();
                        break;
                    }
                case 2:
                    {
                        currentCashBet = DalamberBet(Math.Min(maxBet, PlayerCash));
                        ChooseBigField();
                        break;
                    }
                case 3:
                    {
                        currentCashBet = MartingejlBet(Math.Min(maxBet, PlayerCash));
                        ChooseBigField();
                        break;
                    }
            }
        }

        public override void UpdateInformation(List<string> info, int deltaMoney)
        {
            base.UpdateInformation(info, deltaMoney);
            if (CurrentBet.Equals(info[0]) ||
                CurrentBet.Equals(info[1]) ||
                CurrentBet.Equals(info[2]) ||
                CurrentBet.Equals(info[3]))
            {
                previousGamesResult.Add(1);
                previousGamesResult.RemoveAt(0);
            }
            else
            {
                previousGamesResult.Add(0);
                previousGamesResult.RemoveAt(0);
            }
        }

        private int DalamberBet(int maxBet)
        {
            int calculateBet = 0;
            if (previousGamesResult[0] == 0)
            {
                calculateBet = Math.Min(previousCashBet + 10, maxBet);
                previousCashBet = calculateBet;
            }
            else if (previousGamesResult[0] == 1)
            {
                calculateBet = Math.Min(previousCashBet - 10, maxBet);
                previousCashBet = calculateBet;
            }
            else
            {
                calculateBet = Math.Min(maxBet, PlayerCash / 20);
                previousCashBet = calculateBet;
            }
            return calculateBet;
        }

        private int MartingejlBet(int maxBet)
        {
            int calculateBet = 0;
            if (previousGamesResult[0] == 0)
            {
                calculateBet = Math.Min(previousCashBet * 2, maxBet);
                previousCashBet = calculateBet;
            }
            else
            {
                calculateBet = Math.Min(maxBet, PlayerCash / 1000);
                previousCashBet = calculateBet;
            }
            return calculateBet;
        }

        private void ChooseBigField()
        {
            if (previousGamesResult[0] == 2)
            {
                switch (humanFactor.Next(1, 3))
                {
                    case 1:
                        {
                            switch (humanFactor.Next(1, 3))
                            {
                                case 1:
                                    {
                                        currentBet = "Even";
                                        previousBet = CurrentBet;
                                        break;
                                    }
                                case 2:
                                    {
                                        currentBet = "Odd";
                                        previousBet = CurrentBet;
                                        break;
                                    }
                            }
                            break;
                        }
                    case 2:
                        {
                            switch (humanFactor.Next(1, 3))
                            {
                                case 1:
                                    {
                                        currentBet = "Red";
                                        previousBet = CurrentBet;
                                        break;
                                    }
                                case 2:
                                    {
                                        currentBet = "Black";
                                        previousBet = CurrentBet;
                                        break;
                                    }
                            }
                            break;
                        }
                }
            }
            else
                currentBet = previousBet;
        }
    }
}
