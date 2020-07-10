using System;
using System.Collections.Generic;
using System.Text;

namespace Hierarchy
{
    public abstract class Tank
    {
        public string Color { get; private set; }
        public string CountryOfManufacture { get; private set; }
        public int MaxSpeed { get; private set; }
        public int Armor { get; private set; }
        public int CabinCrew { get; private set; }

        public Tank(string color, string countryOfManufacture, int maxSpeed, int armor, int cabinCrew)
        {
            Color = color;
            CountryOfManufacture = countryOfManufacture;
            MaxSpeed = maxSpeed;
            Armor = armor;
            CabinCrew = cabinCrew;
        }
        public void Print(string choice = "0")
        {
            if (choice == "0")
            {
                Console.WriteLine("Color is " + Color);
                Console.WriteLine("Country Of Manufacture is " + CountryOfManufacture);
                Console.WriteLine("Maximum Speed is " + MaxSpeed);
                Console.WriteLine("Armor is " + Armor);
                Console.WriteLine("Amount of Crew is " + CabinCrew);
            }
            else if (choice == "Color")
                Console.WriteLine("Color is " + Color);
            else if (choice == "Country Of Manufacture")
                Console.WriteLine("Country Of Manufacture is " + CountryOfManufacture);
            else if (choice == "Maximum Speed")
                Console.WriteLine("Maximum Speed is " + MaxSpeed);
            else if (choice == "Armor")
                Console.WriteLine("Armor is " + Armor);
            else if (choice == "Cabin Crew")
                Console.WriteLine("Amount of Crew is " + CabinCrew);
            else
                Console.WriteLine("Enterd wrong parametrs! Try again");
            Console.WriteLine("\n");
        }


    }
}