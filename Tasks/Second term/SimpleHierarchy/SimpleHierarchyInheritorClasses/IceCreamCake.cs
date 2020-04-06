using System;
using System.Collections.Generic;
using System.Text;
using SimpleHierarchyBaseClass;

namespace SimpleHierarchyInheritorClasses
{
    public class IceCreamCake : AbstractIceCream
    {
        public IceCreamCake()
        {
            IceCreamName = "Ice cream cake";
            Type = "Cake";
            Form = "Cake";
            NumberOfBalls = 0;
            Weight = 750;
            WeightOfDarkChocolate = 100;
            WeightOfMilkChocolate = 0;
            Milk = 0;
            Eggs = 4;
            Sugar = 100;
            WhippedCream = 500;
        }
        public override void ShowRecipeAndInfo()
        {
            base.ShowRecipeAndInfo();
            string eggsInfo = $"{((Eggs == 0) ? "without eggs, " : "with " + Eggs.ToString() + " eggs, ")}";
            string sugarInfo = $"{((Sugar == 0) ? "without sugar, " : "with " + Sugar.ToString() + " grams of sugar, ")}";
            string milkInfo = $"{((Milk == 0) ? "without milk, " : "with " + Milk.ToString() + " milliliters of milk, ")}";
            string whippedCreamInfo = $"{"with " + WhippedCream + " grams of whipped cream, "}";
            string darkChocolateInfo = $"{((WeightOfDarkChocolate == 0) ? "without dark chocolate, " : "with " + WeightOfDarkChocolate.ToString() + " grams of dark chocolate, ")}";
            string milkChocolateInfo = $"{((WeightOfMilkChocolate == 0) ? "without milk chocolate. " : "with " + WeightOfMilkChocolate.ToString() + " grams of milk chocolate.")}";
            Console.WriteLine("Recipe:\n" + $"Necessary products - {eggsInfo}{sugarInfo}{milkInfo}{whippedCreamInfo}{darkChocolateInfo}{milkChocolateInfo}");
            Console.WriteLine("*****");
        }
    }
}
