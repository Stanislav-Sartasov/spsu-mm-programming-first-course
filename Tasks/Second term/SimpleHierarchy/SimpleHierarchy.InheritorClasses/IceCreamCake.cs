using System;
using System.Collections.Generic;
using System.Text;
using SimpleHierarchy.BaseClass;

namespace SimpleHierarchy.InheritorClasses
{
    public class IceCreamCake : AbstractIceCream
    {
        public IceCreamCake()
        {
            iceCreamName = "Ice cream cake";
            type = "Cake";
            form = "Cake";
            numberOfBalls = 0;
            weight = 750;
            weightOfDarkChocolate = 100;
            weightOfMilkChocolate = 0;
            milk = 0;
            eggs = 4;
            sugar = 100;
            whippedCream = 500;
        }
        public override void ShowRecipeAndInfo()
        {
            base.ShowRecipeAndInfo();
            string eggsInfo = $"{((eggs == 0) ? "without eggs, " : "with " + eggs.ToString() + " eggs, ")}";
            string sugarInfo = $"{((sugar == 0) ? "without sugar, " : "with " + sugar.ToString() + " grams of sugar, ")}";
            string milkInfo = $"{((milk == 0) ? "without milk, " : "with " + milk.ToString() + " milliliters of milk, ")}";
            string whippedCreamInfo = $"{"with " + whippedCream + " grams of whipped cream, "}";
            string darkChocolateInfo = $"{((weightOfDarkChocolate == 0) ? "without dark chocolate, " : "with " + weightOfDarkChocolate.ToString() + " grams of dark chocolate, ")}";
            string milkChocolateInfo = $"{((weightOfMilkChocolate == 0) ? "without milk chocolate. " : "with " + weightOfMilkChocolate.ToString() + " grams of milk chocolate.")}";
            Console.WriteLine("Recipe:\n" + $"Necessary products - {eggsInfo}{sugarInfo}{milkInfo}{whippedCreamInfo}{darkChocolateInfo}{milkChocolateInfo}");
            Console.WriteLine("*****");
        }
    }
}
