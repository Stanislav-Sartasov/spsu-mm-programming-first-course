using System;
using System.Collections.Generic;
using System.Text;

namespace Casino_Case
{
    public class Table
    {


        public Sector[] wheel = new Sector[37];
        int k = 1, i;
        private void initFirstDozen()
        {
            for (i = 1; i <= 10; i++)
            {
                wheel[i].number = i;
                wheel[i].dozen = 1;
                wheel[i].column = k;
                wheel[i].bid = 0;
                k++;

                if (i % 2 == 0)
                    wheel[i].color = 1;  // черный -
                else
                    wheel[i].color = 0; // красный 

                if (k == 4)
                    k = 1;
            }

            wheel[11].color = 1;
            wheel[11].bid = 0;
            wheel[12].color = 0;
            wheel[12].bid = 0;
            for (i = 11; i <= 12; i++)
            {
                wheel[i].number = i;
                wheel[i].dozen = 1;
                wheel[i].column = k;
                k++;
                if (k == 4)
                    k = 1;
            }

        }

        private void initSecondDozen()
        {
            for (i = 13; i <= 18; i++) 
            {
                wheel[i].number = i;
                wheel[i].dozen = 2;
                wheel[i].column = k;
                wheel[i].bid = 0;
                k++;

                if (i % 2 == 0)
                    wheel[i].color = 0;  // красный
                else
                    wheel[i].color = 1; // черный

                if (k == 4)
                    k = 1;
            }

            for (i = 19; i <= 24; i++)
            {
                wheel[i].number = i;
                wheel[i].dozen = 2;
                wheel[i].column = k;
                wheel[i].bid = 0;
                k++;

                if (i % 2 == 0)
                    wheel[i].color = 1;  // черный
                else
                    wheel[i].color = 0; // красный

                if (k == 4)
                    k = 1;
            }
        }

        private void initThirdDozen()
        {
            for (i = 25; i <= 28; i++)
            {
                wheel[i].number = i;
                wheel[i].dozen = 3;
                wheel[i].column = k;
                wheel[i].bid = 0;
                k++;

                if (i % 2 == 0)
                    wheel[i].color = 1;  // черный
                else
                    wheel[i].color = 0; // красный

                if (k == 4)
                    k = 1;
            }

            for (i = 29; i <= 36; i++)
            {
                wheel[i].number = i;
                wheel[i].dozen = 3;
                wheel[i].column = k;
                wheel[i].bid = 0;
                k++;

                if (i % 2 == 0)
                    wheel[i].color = 0;  // красный
                else
                    wheel[i].color = 1; // черный

                if (k == 4)
                    k = 1;
            }
        }

        public  Table()
        {
            // init zero
            wheel[0].color = -1;
            wheel[0].column = -1;
            wheel[0].dozen = -1;
            wheel[0].number = 0;
            wheel[0].bid = 0;

            initFirstDozen();
            initSecondDozen();
            initThirdDozen();
        }
    }

    public struct Sector
    {
        public int number;
        public int color; // зеленый -1, черный 1, красный 0
        public int dozen; // 1st12 2nd12 3rd12
        public int column;
        public int bid;
       
    }
}
