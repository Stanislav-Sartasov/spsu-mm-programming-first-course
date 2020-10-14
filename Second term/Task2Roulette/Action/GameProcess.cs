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
        
        public TypeOfBets GetColor(int cell)
        {
            if ((((cell > 0 && cell < 10)) || ((cell > 18 && cell < 29) && (cell % 2 == 1)))
                || (((cell > 10 && cell < 19) || (cell > 29)) && (cell % 2 == 0)))
                return TypeOfBets.Red;
            else
                return TypeOfBets.Black;
        }
        
        public bool LaysIn(int cell, int min, int max)
        {
            if (cell >= min && cell <= max)
                return true;
            else
                return false;
        }

        public bool IsWin(TypeOfBets choice, int cell)
        {
            int winningCell = SpinWheel();
            if (choice == TypeOfBets.SpecificNumber)
            {
                if (cell == winningCell)
                    return true;
                else
                    return false;
            }
            else
            {
                if (choice < TypeOfBets.Odd)
                {
                    if (GetColor(winningCell) == choice)
                        return true;
                    else
                        return false;
                }
                else if (choice < TypeOfBets.FirstDozen)
                {
                    if (((int)choice % 2 == winningCell % 2))
                        return true;
                    else
                        return false;
                }
                else if (choice == TypeOfBets.FirstDozen)
                {
                    return LaysIn(winningCell, 1, 12);
                }
                else if (choice == TypeOfBets.SecondDozen)
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
        
        public int GetCoefficient(TypeOfBets choice)
        {
            int coefficient = 1;
            if (choice < TypeOfBets.FirstDozen)
                coefficient = 1;
            else if (choice < TypeOfBets.SpecificNumber)
                coefficient = 2;
            else
                coefficient = 35;
            return coefficient;
        }
    }
}
