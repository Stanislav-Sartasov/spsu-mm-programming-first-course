using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Task3
{
    public class MartingaleBot : ABot
    {
        internal class BetType
        {
            double probability;
            int betCount;
            double reserveCoeff;

            internal double Probability
            {
                get
                {
                    return probability;
                }
                set
                {
                    if (value < 0)
                        probability = 0;
                    else
                        probability = value;
                }
            }
            internal int BetCount
            {
                get
                {
                    return betCount;
                }
                set
                {
                    if (value > 0)
                        betCount = value;
                }
            }
            internal double ReserveCoeff
            {
                get
                {
                    return reserveCoeff;
                }
                set
                {
                    if (value >= 0 && value <= 1)
                        reserveCoeff = value;
                    if (value > 1)
                        reserveCoeff = 1;
                    if (value < 0)
                        ReserveCoeff = 0;
                }
            }
        }
        public MartingaleBot()
        {
            types[0] = new BetType
            {
                Probability = 0.46,
                BetCount = 4,
                ReserveCoeff = 0.04
            };                          //the best params after some tests

            types[1] = new BetType
            {
                Probability = 0.45,
                BetCount = 4,
                ReserveCoeff = 0.04
            };

            types[2] = new BetType
            {
                Probability = 0.09,
                BetCount = 12,
                ReserveCoeff = 0.66
            };
        }

        Field actField = Field.Player;
        public int betNumber = 0;
        int bet = -1;
        double[] GameСoefficients
        {
            get
            {
                return ConnectedPlayer.GameСoefficients;
            }
        }
        BetType[] types = new BetType[3];
        bool settingChanged = false;

        public bool GetType(int type, out double probability, out int betCount, out double reserveCoeff)
        {
            if (type < 0 || type > 2)
            {
                probability = default;
                betCount = default;
                reserveCoeff = default;
                return false;
            }

            probability = types[type].Probability;
            betCount = types[type].BetCount;
            reserveCoeff = types[type].ReserveCoeff;
            return true;
        }
        public bool ChangeTypeSettings(int type, double probability, int betCount, double reserveCoeff)
        {
            if (type < 0 || type > 2)
                return false;

            if (!CanBeSettingsChanged())
                return false;

            types[type].Probability = probability;
            types[type].BetCount = betCount;
            types[type].ReserveCoeff = reserveCoeff;
            settingChanged = true;
            return true;
        }
        public bool CanBeSettingsChanged()
        {
            if (ConnectedPlayer != null)
                if (betNumber != types[(int)actField].BetCount && ConnectedPlayer.LastWinField != actField && bet != -1)
                    return false;
            return true;
        }
        public override bool MakeBet()
        {
            if (ConnectedPlayer != null)
            {
                if (ConnectedPlayer.BetDone)
                    return false;

                if (betNumber >= types[(int)actField].BetCount || ConnectedPlayer.LastWinField == actField || bet == -1 || settingChanged)
                {
                    settingChanged = false;

                    if (startBank == -1)
                        ResetStartBank();

                    if (NeedToLeave())
                    {
                        actField = 0;
                        betNumber = 0;
                        bet = -1;
                        startBank = -1;
                        ConnectedPlayer.QuitGame();
                        return false;
                    }

                    double probabilitySum = 0;
                    for (int i = 0; i < 3; i++)
                        probabilitySum += types[i].Probability;

                    double newRand = rand.NextDouble() * probabilitySum;
                    if (newRand < types[0].Probability)
                        actField = Field.Player;
                    else if (newRand < types[0].Probability + types[1].Probability)
                        actField = Field.Bank;
                    else
                        actField = Field.Draw;

                    double denominator = 0;
                    for (int i = 0; i < types[(int)actField].BetCount; i++)
                        denominator += Math.Pow(GameСoefficients[(int)actField], i) / Math.Pow(GameСoefficients[(int)actField] - 1, i);

                    bet = (int)Math.Floor(Bank * (1 - types[(int)actField].ReserveCoeff) / denominator);
                    if (bet == 0)
                        bet = 1;

                    betNumber = 0;
                }

                int nowBet = (int)Math.Floor(bet * Math.Pow(GameСoefficients[(int)actField], betNumber) / Math.Pow(GameСoefficients[(int)actField] - 1, betNumber));
                betNumber++;
                if (nowBet > Bank)
                    nowBet = Bank;
                ConnectedPlayer.MakeBet(nowBet, actField);
                return true;
            }
            else
                return false;
        }
    }
}