using System;

namespace CasinoIncidentRoulette.Roulette
{
    public struct Cell
    {
        public int Number;
        public int Color; //0 - green, 1 - red, 2 - black
        public bool Parity; //false - odd, true - even
        public int Dozen; //0 - for green, 1 - 1st dozen, 2 - 2nd dozen, 3 - 3rd dozen
    }

    public static class Table
    {

        private static Random rnd = new Random();
        private static Cell[] roulette = new Cell[37];

        public static void CreateTable()
        {
            roulette[0].Number = 0; roulette[0].Color = 0; roulette[0].Parity = true; roulette[0].Dozen = 0;
            for (int i = 1; i < 37; i++)
            {
                roulette[i].Number = i;
                roulette[i].Color = i % 2 + 1;
                roulette[i].Parity = i % 2 == 0;
                roulette[i].Dozen = (i - 1) % 12 + 1;
            }
        }

        public static Cell Roll()
        {
            return roulette[rnd.Next(0, 37)];
        }

        public static Cell GetCell(int indexCell)
        {
            return roulette[indexCell];
        }

    }
}
