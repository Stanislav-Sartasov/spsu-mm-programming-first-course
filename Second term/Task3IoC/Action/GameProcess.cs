using System;

namespace Action
{
    public class GameProcess
    {
        
        
        public int GetColor(int cell)
        {
            if ((((cell > 0 && cell < 10)) || ((cell > 18 && cell < 29) && (cell % 2 == 1)))
                || (((cell > 10 && cell < 19) || (cell > 29)) && (cell % 2 == 0)))
                return 1; // Red
            else
                return 2; // Black
        }
        
        public bool LaysIn(int cell, int min, int max)
        {
            if (cell >= min && cell <= max)
                return true;
            else
                return false;
        }

        public bool Result(int choice, int cell)
        {
            int winningCell = SpinWheel();
            if (choice == 8)
            {
                if (cell == winningCell)
                    return true;
                else
                    return false;
            }
            else
            {
                if (choice < 3)
                {
                    if (GetColor(winningCell) == choice)
                        return true;
                    else
                        return false;
                }
                else if (choice < 5)
                {
                    if ((choice % 2 == winningCell % 2))
                        return true;
                    else
                        return false;
                }
                else if (choice == 5)
                {
                    return LaysIn(winningCell, 1, 12);
                }
                else if (choice == 6)
                {
                    return LaysIn(winningCell, 13, 24);
                }
                else
                {
                    return LaysIn(winningCell, 25, 36);
                }

            }
        }
        public int SpinWheel()
        {
            Random rnd = new Random();
            return rnd.Next(37);
        }
        
        public int GetCoefficient(int choice)
        {
            int coefficient = 1;
            if (choice < 5)
                coefficient = 1;
            else if (choice < 8)
                coefficient = 2;
            else
                coefficient = 35;
            return coefficient;
        }
    }
}
