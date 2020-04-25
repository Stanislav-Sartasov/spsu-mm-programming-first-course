using System;
using System.Collections.Generic;
using System.Text;
using Task3;

namespace Task3
{
    public class MartingaleBot : Bot
    {
        class BateType
        {
            double probabilityIn;
            int bateCountIn;
            double reserveCoeffIn;

            internal double Probability
            {
                get
                {
                    return probabilityIn;
                }
                set
                {
                    if (value < 0)
                        probabilityIn = 0;
                    else
                        probabilityIn = value;
                }
            }
            internal int BateCount
            {
                get
                {
                    return bateCountIn;
                }
                set
                {
                    if (value > 0)
                        bateCountIn = value;
                }
            }
            internal double ReserveCoeff
            {
                get
                {
                    return reserveCoeffIn;
                }
                set
                {
                    if (value >= 0 && value <= 1)
                        reserveCoeffIn = value;
                    if (value > 1)
                        reserveCoeffIn = 1;
                    if (value < 0)
                        ReserveCoeff = 0;
                }
            }
        }
        public MartingaleBot()
        {
            Name = "Martin";

            types[0] = new BateType();             //the best params after some tests
            types[0].Probability = 0.46;
            types[0].BateCount = 4;
            types[0].ReserveCoeff = 0.04;

            types[1] = new BateType();
            types[1].Probability = 0.45;
            types[1].BateCount = 4;
            types[1].ReserveCoeff = 0.04;

            types[2] = new BateType();
            types[2].Probability = 0.09;
            types[2].BateCount = 12;
            types[2].ReserveCoeff = 0.66;
        }

        int actType = 0;
        int bateNumber = 0;
        int bate = -1;
        double[] coefficients = null;
        BateType[] types = new BateType[3];
        bool settingChanged = false;

        public int ChangeTypeSettings(int type, double probability, int bateCount, double reserveCoeff)
        {
            if (place != null)
                if (type < 0 || type > 2 || bateNumber != types[actType].BateCount)
                    return -1;
            types[type].Probability = probability;
            types[type].BateCount = bateCount;
            types[type].ReserveCoeff = reserveCoeff;
            settingChanged = true;
            return 0;
        }
        public bool CanBeSettingsChanged()
        {
            if (place != null)
                if (bateNumber != types[actType].BateCount && place.lastWinType != actType && bate != -1)
                    return false;
            return true;
        }
        public override int MakeBate()
        {
            if (place != null)
            {
                if (place.AreBated)
                    return -1;
                if (coefficients == null)
                    coefficients = (double[])place.coefficients.Clone();

                if (bateNumber >= types[actType].BateCount || place.lastWinType == actType || bate == -1 || settingChanged)
                {
                    settingChanged = false;

                    if (startBudget == -1)
                        ResetStartBudget();

                    if (NeedToLeave())
                    {
                        actType = 0;
                        bateNumber = 0;
                        bate = -1;
                        startBudget = -1;
                        LeaveActiveGameSession();
                        return -1;
                    }

                    double probabilitySum = 0;
                    for (int i = 0; i < 3; i++)
                        probabilitySum += types[i].Probability;

                    double newRand = rand.NextDouble() * probabilitySum;
                    if (newRand < types[0].Probability)
                        actType = 0;
                    else if (newRand < types[0].Probability + types[1].Probability)
                        actType = 1;
                    else
                        actType = 2;

                    double denominator = 0;
                    for (int i = 0; i < types[actType].BateCount; i++)
                        denominator += Math.Pow(coefficients[actType], i) / Math.Pow(coefficients[actType] - 1, i);

                    bate = (int)Math.Floor(Budget * (1 - types[actType].ReserveCoeff) / denominator);
                    if (bate == 0)
                        bate = 1;

                    bateNumber = 0;
                }

                int nowBate = (int)Math.Floor(bate * Math.Pow(coefficients[actType], bateNumber) / Math.Pow(coefficients[actType] - 1, bateNumber));
                bateNumber++;
                if (nowBate > Budget)
                    nowBate = Budget;
                place.MakeBate(nowBate, actType);
                return 0;
            }
            else
                return -1;
        }
    }
}
