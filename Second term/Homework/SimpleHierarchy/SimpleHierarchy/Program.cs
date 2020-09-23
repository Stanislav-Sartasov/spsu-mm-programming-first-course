using IceCreamRecipes;

namespace SimpleHierarchy
{
    class Program
    {
        static void Main(string[] args)
        {
            FruitIce fruitIce = new FruitIce();
            Sundae sundaeWithTopping = new Sundae("Chocolate");
            Sundae sundaeWithoutTopping = new Sundae();
            ClassicIceCream classicIC = new ClassicIceCream();
            fruitIce.PrintInfo();
            sundaeWithoutTopping.PrintInfo();
            sundaeWithTopping.PrintInfo();
            classicIC.PrintInfo();
        }
    }
}
