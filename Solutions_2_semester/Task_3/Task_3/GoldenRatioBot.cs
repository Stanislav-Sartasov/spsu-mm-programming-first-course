using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Task3;

namespace Task3
{
    public class GoldenRatioBot : Bot
    {
        public GoldenRatioBot()
        {
            Name = "Porunarefu";
        }
        double GoldenBig(double value)
        {
            return value * GoldenRat / (GoldenRat + 1);
        }

        const double GoldenRat = 1.6180339887498948482;
        int bateNumber = 0;
        int[] bate;
        int actType = 0;
        int count = 4;
        int playWin = 0;
        int bankWin = 0;
        bool settingChanged = false;

        public int ChangeCount(int newCount)
        {
            if (newCount <= 0)
                return -1;
            if (place != null)
                if (!(count == bateNumber || place.lastWinType == actType))
                    return -1;

            count = newCount;
            settingChanged = true;
            return 0;
        }
        public bool CanBeSettingsChanged()
        {
            if (place != null)
                if (!(count == bateNumber || place.lastWinType == actType || bate == null))
                    return false;
            return true;
        }
        public override int MakeBate()
        {
            if (place != null)
            {
                if (place.AreBated)
                    return -1;
                if (count == bateNumber || place.lastWinType == actType || settingChanged)
                    bate = null;
                if (bate == null)
                {
                    settingChanged = false;
                    if (startBudget == -1)
                        ResetStartBudget();
                    if (NeedToLeave())
                    {
                        bateNumber = 0;
                        playWin = 0;
                        bankWin = 0;
                        actType = 0;
                        startBudget = -1;
                        LeaveActiveGameSession();
                        return -1;
                    }

                    int[] newBate = new int[count];
                    newBate[0] = (int)Math.Floor(GoldenBig(Budget));
                    for (int i = 1; i < count; i++)
                    {
                        newBate[i] = (int)Math.Floor(GoldenBig(newBate[i - 1]));
                        newBate[i - 1] -= newBate[i];
                    }
                    bate = new int[count];
                    bate[0] = newBate[count - 2];
                    bate[1] = newBate[count - 1];
                    for (int i = 0; i < count - 2; i++)
                        bate[i + 2] = newBate[count - 3 - i];

                    bateNumber = 0;
                }

                if (place.lastWinType != -1)
                {
                    switch (place.lastWinType)
                    {
                        case 0:
                            playWin++;
                            break;
                        case 1:
                            bankWin++;
                            break;
                        case 2:
                            if (rand.NextDouble() < 0.5)
                                playWin++;
                            else
                                bankWin++;
                            break;
                    }
                    double a;
                    double b;
                    if (bankWin != 0 && playWin != 0)
                    {
                        a = (double)playWin / bankWin;       //n / m -> Golden ratio
                        b = (double)bankWin / playWin;       //select what's better playWin / bankWin or bankWin / playWin
                    }
                    else
                    {
                        a = 0;
                        b = 0;
                    }
                    int n;
                    int m;
                    char up;

                    if (Math.Abs(a - GoldenRat) > Math.Abs(b - GoldenRat))
                    {
                        n = bankWin;
                        m = playWin;
                        up = 'b';
                    }
                    else
                    {
                        n = playWin;
                        m = bankWin;
                        up = 'p';
                    }

                    a = GoldenRat * m - n;          //(n + a) / b or n / (b + a) -> Golden ratio
                    b = n / GoldenRat - m;          //select what's smaller a or b

                    if (Math.Abs(a) > Math.Abs(b))
                        if (up == 'p')
                            actType = 1;
                        else
                            actType = 0;
                    else if (up == 'p')
                        actType = 1;
                    else
                        actType = 0;

                    if (m != 0)
                        if (Math.Abs(n / m - GoldenRat) < 0.01)
                            actType = 2;
                }
                else
                    actType = rand.Next(0, 2);

                if (bate[bateNumber] <= 0)
                {
                    bateNumber++;
                    place.MakeBate(1, actType);
                }
                else
                    place.MakeBate(bate[bateNumber++], actType);

                return 0;
            }
            else
                return -1;
        }
    }
}
