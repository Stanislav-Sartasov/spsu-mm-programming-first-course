using System;
using System.Collections.Generic;

namespace AbstractPanzer
{
    public abstract class Panzer
    {
        public string Model { get; private set; }
        public string ManufacturerCountry { get; private set; }
        public int Weight { get; private set; }

        public Dictionary<string, int> Armament { get; private set; }

        public Panzer(string model, string manufacturercountry, int weight, Dictionary<string, int> armament)
        {
            Model = model;
            ManufacturerCountry = manufacturercountry;
            Weight = weight;
            Armament = armament;
        }

        public virtual string GetMainInfo()
        {
            return $"\nModel : {Model}\nCountry: {ManufacturerCountry}\nWeight: {Weight} tons";
        }

        public virtual string GetIngredients()
        {
            string info = $"\nTank Config {Model}:\n";
            foreach (var Parameter in Armament)
                info += $"{Parameter.Key}: {Parameter.Value}\n";
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