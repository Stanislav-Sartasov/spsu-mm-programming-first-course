using System;
using System.Collections.Generic;
using System.Text;
using IceCream;

namespace PopularIceCream
{
    class WaffleChoco : AbstractIceCream
    {
        public WaffleChoco()
        {
            type = Type.chocolate;
            innings = Innings.withWaffle;
            count = 200;
        }
    }
}