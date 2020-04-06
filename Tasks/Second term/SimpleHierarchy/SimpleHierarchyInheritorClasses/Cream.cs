using System;
using SimpleHierarchyBaseClass;

namespace SimpleHierarchyInheritorClasses
{
    public class Cream : AbstractIceCream
    {
        public uint Vanilin;
        public Cream()
        {
            IceCreamName = "Ice cream in a waffle cup";
            Type = "Creamy";
            Form = "In a waffle cup";
            NumberOfBalls = 0;
            Weight = 500;
            WeightOfDarkChocolate = 0;
            WeightOfMilkChocolate = 0;
            Milk = 200;
            Eggs = 4;
            Sugar = 70;
            WhippedCream = 200;
            Vanilin = 7;
        }
        public override void ShowRecipeAndInfo()
        {
            base.ShowRecipeAndInfo();
            string eggsInfo = $"{((Eggs == 0) ? "without eggs, " : "with " + Eggs.ToString() + " eggs, ")}";
            string sugarInfo = $"{((Sugar == 0) ? "without sugar, " : "with " + Sugar.ToString() + " grams of sugar, ")}";
            string milkInfo = $"{((Milk == 0) ? "without milk, " : "with " + Milk.ToString() + " milliliters of milk, ")}";
            string whippedCreamInfo = $"{"with " + WhippedCream + " grams of whipped cream, "}";
            string vanilinInfo = $"{("with " + Vanilin + " grams of vanilin, ")}";
            string darkChocolateInfo = $"{((WeightOfDarkChocolate == 0) ? "without dark chocolate, " : "with " + WeightOfDarkChocolate.ToString() + " grams of dark chocolate, ")}";
            string milkChocolateInfo = $"{((WeightOfMilkChocolate == 0) ? "without milk chocolate. " : "with " + WeightOfMilkChocolate.ToString() + " grams of milk chocolate.")}";
            Console.WriteLine("Recipe:\n" + $"Necessary products - {eggsInfo}{sugarInfo}{milkInfo}{whippedCreamInfo}{vanilinInfo}{darkChocolateInfo}{milkChocolateInfo}");
            Console.WriteLine("*****");
        }
    }
}
