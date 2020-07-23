using System;
using System.Collections.Generic;
using System.Text;

namespace Tanks
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
        public void Print(Choose choice)
        {


            if (choice == Choose.All)
            {
                Console.WriteLine("Color is " + Color);
                Console.WriteLine("Country Of Manufacture is " + CountryOfManufacture);
                Console.WriteLine("Maximum Speed is " + MaxSpeed);
                Console.WriteLine("Armor is " + Armor);
                Console.WriteLine("Amount of Crew is " + CabinCrew);
            }
            else if (choice == Choose.Color)
                Console.WriteLine("Color is " + Color);
            else if (choice == Choose.CountryOfManufacture)
                Console.WriteLine("Country Of Manufacture is " + CountryOfManufacture);
            else if (choice == Choose.MaximumSpeed)
                Console.WriteLine("Maximum Speed is " + MaxSpeed);
            else if (choice == Choose.Armor)
                Console.WriteLine("Armor is " + Armor);
            else if (choice == Choose.CabinCrew)
                Console.WriteLine("Amount of Crew is " + CabinCrew);
            else
                Console.WriteLine("Enterd wrong parametrs! Try again");
            Console.WriteLine("\n");
        }


        

    }

    public enum Choose : int
    {
        All = 0,
        Color = 1,
        CountryOfManufacture = 2,
        MaximumSpeed = 3,
        Armor = 4,
        CabinCrew = 5
    }

}