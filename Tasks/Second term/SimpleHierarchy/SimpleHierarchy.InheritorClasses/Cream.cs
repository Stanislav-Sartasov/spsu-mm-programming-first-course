using System;
using SimpleHierarchy.BaseClass;

namespace SimpleHierarchy.InheritorClasses
{
    public class Cream : AbstractIceCream
    {
        private uint vanilin;
        public uint Vanilin => vanilin;
        public Cream()
        {
            iceCreamName = "Ice cream in a waffle cup";
            type = "Creamy";
            form = "In a waffle cup";
            numberOfBalls = 0;
            weight = 500;
            weightOfDarkChocolate = 0;
            weightOfMilkChocolate = 0;
            milk = 200;
            eggs = 4;
            sugar = 70;
            whippedCream = 200;
            vanilin = 7;
        }
        public override string ShowRecipeAndInfo()
        {
            string temp = base.ShowRecipeAndInfo();
            string eggsInfo = $"{((eggs == 0) ? "without eggs, " : "with " + eggs.ToString() + " eggs, ")}";
            string sugarInfo = $"{((sugar == 0) ? "without sugar, " : "with " + sugar.ToString() + " grams of sugar, ")}";
            string milkInfo = $"{((milk == 0) ? "without milk, " : "with " + milk.ToString() + " milliliters of milk, ")}";
            string whippedCreamInfo = $"{"with " + whippedCream + " grams of whipped cream, "}";
            string vanilinInfo = $"{("with " + vanilin + " grams of vanilin, ")}";
            string darkChocolateInfo = $"{((weightOfDarkChocolate == 0) ? "without dark chocolate, " : "with " + weightOfDarkChocolate.ToString() + " grams of dark chocolate, ")}";
            string milkChocolateInfo = $"{((weightOfMilkChocolate == 0) ? "without milk chocolate." : "with " + weightOfMilkChocolate.ToString() + " grams of milk chocolate.")}";
            return temp + "Recipe:\n" + $"Necessary products - {eggsInfo}{sugarInfo}{milkInfo}{whippedCreamInfo}{vanilinInfo}{darkChocolateInfo}{milkChocolateInfo}" + "\n*****\n";
        }
    }
}
