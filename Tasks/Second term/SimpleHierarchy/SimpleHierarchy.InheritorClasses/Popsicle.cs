using System;
using System.Collections.Generic;
using System.Text;
using SimpleHierarchy.BaseClass;

namespace SimpleHierarchy.InheritorClasses
{
    public class Popsicle : AbstractIceCream
    {
        private uint butter;
        public uint Butter => butter;
        public Popsicle()
        {
            iceCreamName = "Popsicle";
            type = "Ice cream in a chocolate glaze";
            form = "On a wooden stick";
            numberOfBalls = 0;
            weight = 800;
            weightOfDarkChocolate = 150;
            weightOfMilkChocolate = 0;
            milk = 250;
            eggs = 4;
            sugar = 100;
            whippedCream = 250;
            butter = 100;
        }
        public override string ShowRecipeAndInfo()
        {
            string temp = base.ShowRecipeAndInfo();
            string eggsInfo = $"{((eggs == 0) ? "without eggs, " : "with " + eggs.ToString() + " eggs, ")}";
            string sugarInfo = $"{((sugar == 0) ? "without sugar, " : "with " + sugar.ToString() + " grams of sugar, ")}";
            string milkInfo = $"{((milk == 0) ? "without milk, " : "with " + milk.ToString() + " milliliters of milk, ")}";
            string whippedCreamInfo = $"{"with " + whippedCream + " grams of whipped cream, "}";
            string butterInfo = $"{("with " + Butter + " grams of butter, ")}";
            string darkChocolateInfo = $"{((weightOfDarkChocolate == 0) ? "without dark chocolate, " : "with " + weightOfDarkChocolate.ToString() + " grams of dark chocolate, ")}";
            string milkChocolateInfo = $"{((weightOfMilkChocolate == 0) ? "without milk chocolate." : "with " + weightOfMilkChocolate.ToString() + " grams of milk chocolate.")}";
            return temp + "Recipe:\n" + $"Necessary products - {eggsInfo}{sugarInfo}{milkInfo}{whippedCreamInfo}{butterInfo}{darkChocolateInfo}{milkChocolateInfo}" + "\n*****\n";
        }
    }
}
