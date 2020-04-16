using System;
using System.Collections.Generic;

namespace AbstractIceCream
{
    public abstract class IceCream
    {
        public string Kind { get; private set; }
        public string Flavour { get; private set; }
        public int ServingWeight { get; private set; }

        public Dictionary<string, int> Ingredients { get; private set; }

        public IceCream(string kind, string flavour, int weight, Dictionary<string, int> ingredients)
        {
            Kind = kind;
            Flavour = flavour;
            ServingWeight = weight;
            Ingredients = ingredients;
        }

        public virtual void GetMainInfo()
        {
            Console.WriteLine($"\nKind of ice cream: {Kind}\nFlavour: {Flavour}\nWeight: {ServingWeight}");
        }

        public virtual void GetIngredients()
        {
            Console.WriteLine($"\nMain ingredients of {Kind}: ");
            foreach (var ingredient in Ingredients)
                Console.WriteLine($"{ingredient.Key}: {ingredient.Value} grams");
        }

        public virtual void GetFullInfo()
        {
            GetMainInfo();
            GetIngredients();
        }
    }
}
