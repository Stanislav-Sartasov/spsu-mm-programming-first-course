using System;
using System.Collections.Generic;
using System.Text;
using IceCream;

namespace PopularIceCream
{
    class Special : AbstractIceCream
    {
        public Special()
        {
            type = Type.special;
            innings = Innings.briquette;
            count = 500;
        }
    }
}
