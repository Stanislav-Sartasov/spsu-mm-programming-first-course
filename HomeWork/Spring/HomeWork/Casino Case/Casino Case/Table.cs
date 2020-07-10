using System;
using System.Collections.Generic;
using System.Text;

namespace Casino_Case
{
    public class Table
    {

        internal Sector[] Wheel = new Sector[37];
        int k = 1, i;
        private void InitFirstDozen()
        {
            for (i = 1; i <= 10; i++)
            {
                Wheel[i].Number = i;
                Wheel[i].Dozen = Dozens.First12;
                Wheel[i].Column = k;
                Wheel[i].Bid = 0;
                k++;

                if (i % 2 == 0)
                    Wheel[i].Color = Colors.Black;
                else
                    Wheel[i].Color = Colors.Red; 

                if (k == 4)
                    k = 1;
            }

            Wheel[11].Color = Colors.Black;
            Wheel[11].Bid = 0;
            Wheel[12].Color = Colors.Red;
            Wheel[12].Bid = 0;
            for (i = 11; i <= 12; i++)
            {
                Wheel[i].Number = i;
                Wheel[i].Dozen = Dozens.First12;
                Wheel[i].Column = k;
                k++;
                if (k == 4)
                    k = 1;
            }

        }

        private void InitSecondDozen()
        {
            for (i = 13; i <= 18; i++) 
            {
                Wheel[i].Number = i;
                Wheel[i].Dozen = Dozens.Second12;
                Wheel[i].Column = k;
                Wheel[i].Bid = 0;
                k++;

                if (i % 2 == 0)
                    Wheel[i].Color = Colors.Red; 
                else
                    Wheel[i].Color = Colors.Black; 

                if (k == 4)
                    k = 1;
            }

            for (i = 19; i <= 24; i++)
            {
                Wheel[i].Number = i;
                Wheel[i].Dozen = Dozens.Second12;
                Wheel[i].Column = k;
                Wheel[i].Bid = 0;
                k++;

                if (i % 2 == 0)
                    Wheel[i].Color = Colors.Black;  
                else
                    Wheel[i].Color = Colors.Red; 

                if (k == 4)
                    k = 1;
            }
        }

        private void InitThirdDozen()
        {
            for (i = 25; i <= 28; i++)
            {
                Wheel[i].Number = i;
                Wheel[i].Dozen = Dozens.Third12;
                Wheel[i].Column = k;
                Wheel[i].Bid = 0;
                k++;

                if (i % 2 == 0)
                    Wheel[i].Color = Colors.Black;  
                else
                    Wheel[i].Color = Colors.Red; 

                if (k == 4)
                    k = 1;
            }

            for (i = 29; i <= 36; i++)
            {
                Wheel[i].Number = i;
                Wheel[i].Dozen = Dozens.Third12;
                Wheel[i].Column = k;
                Wheel[i].Bid = 0;
                k++;

                if (i % 2 == 0)
                    Wheel[i].Color = Colors.Red;  
                else
                    Wheel[i].Color = Colors.Black; 

                if (k == 4)
                    k = 1;
            }
        }

        public  Table()
        {
            // init zero
            Wheel[0].Color = Colors.Green;
            Wheel[0].Column = -1;
            Wheel[0].Dozen = Dozens.Zero;
            Wheel[0].Number = 0;
            Wheel[0].Bid = 0;

            InitFirstDozen();
            InitSecondDozen();
            InitThirdDozen();
        }
          internal struct Sector
          {
            public int Number;
            public Colors Color;
            public Dozens Dozen; 
            public int Column;
            public int Bid;

          }
        public enum Colors : Int16
        {
            Red = 0,
            Black = 1,
            Green = -1,
        }
        public enum Dozens : Int16
        {
            First12 = 1,
            Second12 = 2,
            Third12 = 3,
            Zero = -1,
        }
    }


}
