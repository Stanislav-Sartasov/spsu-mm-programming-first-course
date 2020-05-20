
namespace SomeIceCreams
{
    public class SpecialInTheHorn : AbstractIceCream.AbstractIceCream
    {
        public SpecialInTheHorn()
        {
            type = Type.special;
            innings = Innings.inTheHorn;
            count = 0xf0;
        }
    }
}
