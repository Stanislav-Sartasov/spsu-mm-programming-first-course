using System;
using System.Collections.Generic;
using System.Text;
using SimpleHierarchyBaseClass;

namespace SimpleHierarchyInheritorClasses
{
    public class Popsicle : AbstractIceCream
    {
        public uint Butter;
        public Popsicle()
        {
            IceCreamName = "Popsicle";
            Type = "Ice cream in a chocolate glaze";
            Form = "On a wooden stick";
            NumberOfBalls = 0;
            Weight = 800;
            WeightOfDarkChocolate = 150;
            WeightOfMilkChocolate = 0;
            Milk = 250;
            Eggs = 4;
            Sugar = 100;
            WhippedCream = 250;
            Butter = 100;
        }
        public override void ShowRecipeAndInfo()
        {
            base.ShowRecipeAndInfo();
            string eggsInfo = $"{((Eggs == 0) ? "without eggs, " : "with " + Eggs.ToString() + " eggs, ")}";
            string sugarInfo = $"{((Sugar == 0) ? "without sugar, " : "with " + Sugar.ToString() + " grams of sugar, ")}";
            string milkInfo = $"{((Milk == 0) ? "without milk, " : "with " + Milk.ToString() + " milliliters of milk, ")}";
            string whippedCreamInfo = $"{"with " + WhippedCream + " grams of whipped cream, "}";
            string butterInfo = $"{("with " + Butter + " grams of butter, ")}";
            string darkChocolateInfo = $"{((WeightOfDarkChocolate == 0) ? "without dark chocolate, " : "with " + WeightOfDarkChocolate.ToString() + " grams of dark chocolate, ")}";
            string milkChocolateInfo = $"{((WeightOfMilkChocolate == 0) ? "without milk chocolate. " : "with " + WeightOfMilkChocolate.ToString() + " grams of milk chocolate.")}";
            Console.WriteLine("Recipe:\n" + $"Necessary products - {eggsInfo}{sugarInfo}{milkInfo}{whippedCreamInfo}{butterInfo}{darkChocolateInfo}{milkChocolateInfo}");
            Console.WriteLine("*****");
        }
    }
}
