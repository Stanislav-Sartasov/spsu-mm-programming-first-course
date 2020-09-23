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

        public virtual string GetMainInfo()
        {
            return $"\nKind of ice cream: {Kind}\nFlavour: {Flavour}\nWeight: {ServingWeight}";
        }

        public virtual string GetIngredients()
        {
            string info = $"\nMain ingredients of {Kind}:\n";
            foreach (var ingredient in Ingredients)
                info += $"{ingredient.Key}: {ingredient.Value} grams\n";
            return info;
        }

        public virtual string GetFullInfo()
        {
            return GetMainInfo() + GetIngredients();
        }

        public void PrintInfo()
        {
            Console.WriteLine(GetFullInfo());
        }
    }
}
