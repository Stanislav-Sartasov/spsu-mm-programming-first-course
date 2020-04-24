using System;
using System.Collections.Generic;
using System.Text;
using IceCream;

namespace PopularIceCream
{
    class StreetStravberry : AbstractIceCream
    {
        public StreetStravberry()
        {
            type = Type.strawberry;
            innings = Innings.onStick;
            count = 1;
        }
    }
}