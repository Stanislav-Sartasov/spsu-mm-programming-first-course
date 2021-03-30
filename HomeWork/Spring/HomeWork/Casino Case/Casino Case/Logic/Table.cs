using System;
using System.Collections.Generic;
using System.Text;

namespace Casino_Case
{
    public class Table
    {

        internal Sector[] Wheel = new Sector[37];
        int k = 1;
        private void Init(int start, int end, int dozen, int parity, int k = 1)
        {
       
            for (int i = start; i <= end; i++) 
            {
                Wheel[i].Number = i;
                Wheel[i].Dozen = (Dozens)dozen;
                Wheel[i].Column = k;
                Wheel[i].Bid = 0;
                k++;

                if ((i % 2 == 0 && parity == 2) || (i % 2 == 1 && parity == 1))
                    Wheel[i].Color = Colors.Black;
                else
                    Wheel[i].Color = Colors.Red;

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

            Init(1, 10, 1, 2);
            Init(11, 12, 1, 1, 2);
            Init(13, 18, 2, 1);
            Init(19, 24, 2, 2);
            Init(19, 24, 2, 2);
            Init(25, 28, 3, 2);
            Init(29, 36, 3, 1, 2);
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
