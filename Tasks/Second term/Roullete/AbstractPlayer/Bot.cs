using System;
using System.Collections.Generic;
using System.Text;

namespace Players
{
    public class Bot : AbstractBot
    {
        public Bot(byte botLevel)
        {
            switch (botLevel)
            {
                case 1:
                    {
                        PlayerName = "Martingejl";
                        PlayerCash = 10_000;
                        LogicLevel = 30;
                        PreviousGamesResult = new List<byte> { 2 };
                        break;
                    }
                case 2:
                    {
                        PlayerName = "D'Alamber";
                        PlayerCash = 10_000;
                        LogicLevel = 20;
                        PreviousGamesResult = new List<byte> { 2 };
                        break;
                    }
                default:
                    {
                        PlayerName = "Rich beginner";
                        PlayerCash = 1_000_000;
                        LogicLevel = 10;
                        PreviousGamesResult = new List<byte> { 2 };
                        break;
                    }
            }
        }

        public void SetBet(int maxBet)
        {
            switch (LogicLevel / 10)
            {
                case 1:
                    {
                        CurrentCashBet = Math.Min(Brain.Next(1, PlayerCash / LogicLevel + Humanfactor.Next(0, Math.Abs((PreviousGamesResult[0] % 2) * Humanfactor.Next(0, PlayerCash / 100 - LogicLevel)))), PlayerCash);
                        ChooseBigField();
                        break;
                    }
                case 2:
                    {
                        CurrentCashBet = DalamberBet(Math.Min(maxBet, PlayerCash));
                        ChooseBigField();
                        break;
                    }
                case 3:
                    {
                        CurrentCashBet = MartingejlBet(Math.Min(maxBet, PlayerCash));
                        ChooseBigField();
                        break;
                    }
            }
        }

        private int DalamberBet(int maxBet)
        {
            int calculateBet = 0;
            if (PreviousGamesResult[0] == 0)
            {
                calculateBet = Math.Min(PreviousCashBet + 10, maxBet);
                PreviousCashBet = calculateBet;
            }
            else if (PreviousGamesResult[0] == 1)
            {
                calculateBet = Math.Min(PreviousCashBet - 10, maxBet);
                PreviousCashBet = calculateBet;
            }
            else
            {
                calculateBet = Math.Min(maxBet, PlayerCash / 20);
            }
            return calculateBet;
        }

        private int MartingejlBet(int maxBet)
        {
            int calculateBet = 0;
            if (PreviousGamesResult[0] == 0)
            {
                calculateBet = Math.Min(PreviousCashBet * 2, maxBet);
                PreviousCashBet = calculateBet;
            }
            else
            {
                calculateBet = Math.Min(maxBet, PlayerCash / 1000);
                PreviousCashBet = calculateBet;
            }
            return calculateBet;
        }

        private void ChooseBigField()
        {
            if (PreviousGamesResult[0] == 2)
            {
                switch (Humanfactor.Next(1, 2))
                {
                    case 1:
                        {
                            switch (Humanfactor.Next(1, 2))
                            {
                                case 1:
                                    {
                                        CurrentBet = "Even";
                                        PreviousBet = CurrentBet;
                                        break;
                                    }
                                case 2:
                                    {
                                        CurrentBet = "Odd";
                                        PreviousBet = CurrentBet;
                                        break;
                                    }
                            }
                            break;
                        }
                    case 2:
                        {
                            switch (Humanfactor.Next(1, 2))
                            {
                                case 1:
                                    {
                                        CurrentBet = "Red";
                                        PreviousBet = CurrentBet;
                                        break;
                                    }
                                case 2:
                                    {
                                        CurrentBet = "Black";
                                        PreviousBet = CurrentBet;
                                        break;
                                    }
                            }
                            break;
                        }
                }
            }
            else
                CurrentBet = PreviousBet;
        }
    }
}
