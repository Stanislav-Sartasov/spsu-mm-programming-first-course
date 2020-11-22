using System;

namespace CasinoIncidentRoulette.Roulette
{
    public enum Color
    {
        Green,
        Red,
        Black
    }

    public enum Parity
    {
        Even,
        Odd
    }

    public enum Dozen
    {
        Green,
        FirstDozen,
        SecondDozen,
        ThirdDozen
    }

    public struct Cell
    {
        public int Number;
        public Color Color;
        public Parity Parity;
        public Dozen Dozen;
    }

    public class Table
    {

        private Random rnd = new Random();
        private Cell[] roulette = new Cell[37];

        public void CreateTable()
        {
            roulette[0].Number = 0; roulette[0].Color = Color.Green; roulette[0].Parity = Parity.Even; roulette[0].Dozen = Dozen.Green;
            for (int i = 1; i < 37; i++)
            {
                roulette[i].Number = i;
                roulette[i].Color = (Color)(i % 2 + 1);
                roulette[i].Parity = (Parity)(i % 2);
                roulette[i].Dozen = (Dozen)((i - 1) / 12 + 1);
            }
        }

        public Cell Roll()
        {
            return roulette[rnd.Next(0, 37)];
        }

        public Cell GetCell(int indexCell)
        {
            return roulette[indexCell];
        }

    }
}
