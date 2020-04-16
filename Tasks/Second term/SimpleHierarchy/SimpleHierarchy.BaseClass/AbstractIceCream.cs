using System;

namespace SimpleHierarchy.BaseClass
{
    public abstract class AbstractIceCream
    {
        protected string iceCreamName;
        protected uint weight;
        protected string type;
        protected string form;
        protected uint numberOfBalls;
        protected uint weightOfDarkChocolate;
        protected uint weightOfMilkChocolate;
        protected uint milk;
        protected uint eggs;
        protected uint sugar;
        protected uint whippedCream;

        public string IceCreamName => iceCreamName;
        public uint Weight => weight;
        public string Type => type;
        public string Form => form;
        public uint NumberOfBalls => numberOfBalls;
        public uint WeightOfDarkChocolate => weightOfDarkChocolate;
        public uint WeightOfMilkChocolate => weightOfMilkChocolate;
        public uint Milk => milk;
        public uint Eggs => eggs;
        public uint Sugar => sugar;
        public uint WhippedCream => whippedCream;

        public virtual void ShowRecipeAndInfo()
        {
            string ballsInfo = $"{((numberOfBalls == 0) ? "without balls\n" : "with " + numberOfBalls.ToString() + " balls\n")}";
            Console.WriteLine($"Name - {iceCreamName}\n" + $"Type - {type}\n" + $"Form - {form}\n" +
                $"Weight gained - {weight}\n" + $"The presence of balls - {ballsInfo}");

        }
    }
}
