
namespace SomeIceCreams
{
    public class WaffleChoco : AbstractIceCream.AbstractIceCream
    {
        public WaffleChoco()
        {
            type = Type.chocolate;
            innings = Innings.withWaffle;
            count = 200;
        }
    }
}