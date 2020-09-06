using System;
using System.Threading.Tasks;

namespace Task3
{
    public class GoldenRatioBot : ABot
    {
        const double GoldenRat = 1.6180339887498948482;
        int bateNumber = 0;
        int[] bate;
        Field actType = Field.player;
        int count = 4;
        int playWin = 0;
        int bankWin = 0;
        bool settingChanged = false;

        double GoldenBig(double value)
        {
            return value * GoldenRat / (GoldenRat + 1);
        }
        public bool ChangeCount(int newCount)
        {
            if (newCount <= 0)
                return false;
            if (!CanBeSettingsChanged())
                return false;

            count = newCount;
            settingChanged = true;
            return true;
        }
        public bool CanBeSettingsChanged()
        {
            if (connectedPlayer != null)
                if (!(count == bateNumber || connectedPlayer.lastWinField == actType || bate == null))
                    return false;
            return true;
        }
        public override bool MakeBet()
        {
            if (connectedPlayer != null)
            {
                if (connectedPlayer.betDone)
                    return false;
                if (count == bateNumber || connectedPlayer.lastWinField == actType || settingChanged)
                    bate = null;
                if (bate == null)
                {
                    settingChanged = false;
                    if (startBank == -1)
                        ResetStartBank();
                    if (NeedToLeave())
                    {
                        bateNumber = 0;
                        playWin = 0;
                        bankWin = 0;
                        actType = 0;
                        startBank = -1;
                        connectedPlayer.QuitGame();
                        return false;
                    }

                    int[] newBate = new int[count];
                    newBate[0] = (int)Math.Floor(GoldenBig(bank));
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

                if (connectedPlayer.lastWinField != Field.none)
                {
                    switch (connectedPlayer.lastWinField)
                    {
                        case Field.player:
                            playWin++;
                            break;
                        case Field.bank:
                            bankWin++;
                            break;
                        case Field.draw:
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
                            actType = Field.bank;
                        else
                            actType = 0;
                    else if (up == 'p')
                        actType = Field.bank;
                    else
                        actType = Field.player;

                    if (m != 0)
                        if (Math.Abs(n / m - GoldenRat) < 0.01)
                            actType = Field.draw;
                }
                else
                    actType = (Field)rand.Next(0, 2);

                if (bate[bateNumber] <= 0)
                {
                    bateNumber++;
                    connectedPlayer.MakeBet(1, actType);
                }
                else
                    connectedPlayer.MakeBet(bate[bateNumber++], actType);

                return true;
            }
            else
                return false;
        }
    }
}