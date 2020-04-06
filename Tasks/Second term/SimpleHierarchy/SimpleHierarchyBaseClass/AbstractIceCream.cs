using System;

namespace SimpleHierarchyBaseClass
{
    public abstract class AbstractIceCream
    {
        public string IceCreamName;
        public uint Weight;
        public string Type;
        public string Form;
        public uint NumberOfBalls;
        public uint WeightOfDarkChocolate;
        public uint WeightOfMilkChocolate;
        public uint Milk;
        public uint Eggs;
        public uint Sugar;
        public uint WhippedCream;

        public virtual void ShowRecipeAndInfo()
        {
            string ballsInfo = $"{((NumberOfBalls == 0) ? "without balls\n" : "with " + NumberOfBalls.ToString() + " balls\n")}";
            Console.WriteLine($"Name - {IceCreamName}\n" + $"Type - {Type}\n" + $"Form - {Form}\n" +
                $"Weight gained - {Weight}\n" + $"The presence of balls - {ballsInfo}");

        }
    }
}
