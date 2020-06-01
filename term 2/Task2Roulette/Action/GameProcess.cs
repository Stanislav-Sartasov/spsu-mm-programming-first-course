using System;

namespace Action
{
    public class GameProcess
    {
        public enum TypeOfBets
        {
            Red = 1,
            Black,
            Odd,
            Even,
            FirstDozen,
            SecondDozen,
            ThirdDozen,
            SpecificNumber
        }
        
        public int GetColor(int cell)
        {
            if ((((cell > 0 && cell < 10)) || ((cell > 18 && cell < 29) && (cell % 2 == 1)))
                || (((cell > 10 && cell < 19) || (cell > 29)) && (cell % 2 == 0)))
                return (int)TypeOfBets.Red;
            else
                return (int)TypeOfBets.Black;
        }
        
        public bool LaysIn(int cell, int min, int max)
        {
            if (cell >= min && cell <= max)
                return true;
            else
                return false;
        }

        public bool IsWin(int choice, int cell)
        {
            int winningCell = SpinWheel();
            if (choice == (int)TypeOfBets.SpecificNumber)
            {
                if (cell == winningCell)
                    return true;
                else
                    return false;
            }
            else
            {
                if (choice < (int)TypeOfBets.Odd)
                {
                    if (GetColor(winningCell) == choice)
                        return true;
                    else
                        return false;
                }
                else if (choice < (int)TypeOfBets.FirstDozen)
                {
                    if ((choice % 2 == winningCell % 2))
                        return true;
                    else
                        return false;
                }
                else if (choice == (int)TypeOfBets.FirstDozen)
                {
                    return LaysIn(winningCell, 1, 12);
                }
                else if (choice == (int)TypeOfBets.SecondDozen)
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
            if (choice < (int)GameProcess.TypeOfBets.FirstDozen)
                coefficient = 1;
            else if (choice < (int)GameProcess.TypeOfBets.SpecificNumber)
                coefficient = 2;
            else
                coefficient = 35;
            return coefficient;
        }
    }
}
