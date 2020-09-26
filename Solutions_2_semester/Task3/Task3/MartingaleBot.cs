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
            types[0] = new BetType();             //the best params after some tests
            types[0].Probability = 0.46;
            types[0].BetCount = 4;
            types[0].ReserveCoeff = 0.04;

            types[1] = new BetType();
            types[1].Probability = 0.45;
            types[1].BetCount = 4;
            types[1].ReserveCoeff = 0.04;

            types[2] = new BetType();
            types[2].Probability = 0.09;
            types[2].BetCount = 12;
            types[2].ReserveCoeff = 0.66;
        }

        Field actField = Field.Player;
        public int betNumber = 0;
        int bet = -1;
        double[] gameСoefficients
        {
            get
            {
                return connectedPlayer.gameСoefficients;
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
            if (connectedPlayer != null)
                if (betNumber != types[(int)actField].BetCount && connectedPlayer.lastWinField != actField && bet != -1)
                    return false;
            return true;
        }
        public override bool MakeBet()
        {
            if (connectedPlayer != null)
            {
                if (connectedPlayer.betDone)
                    return false;

                if (betNumber >= types[(int)actField].BetCount || connectedPlayer.lastWinField == actField || bet == -1 || settingChanged)
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
                        connectedPlayer.QuitGame();
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
                        denominator += Math.Pow(gameСoefficients[(int)actField], i) / Math.Pow(gameСoefficients[(int)actField] - 1, i);

                    bet = (int)Math.Floor(bank * (1 - types[(int)actField].ReserveCoeff) / denominator);
                    if (bet == 0)
                        bet = 1;

                    betNumber = 0;
                }

                int nowBet = (int)Math.Floor(bet * Math.Pow(gameСoefficients[(int)actField], betNumber) / Math.Pow(gameСoefficients[(int)actField] - 1, betNumber));
                betNumber++;
                if (nowBet > bank)
                    nowBet = bank;
                connectedPlayer.MakeBet(nowBet, actField);
                return true;
            }
            else
                return false;
        }
    }
}