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
        public void Print(Choice choice)
        {


            if (choice == Choice.All)
            {
                Console.WriteLine("Color is " + Color);
                Console.WriteLine("Country Of Manufacture is " + CountryOfManufacture);
                Console.WriteLine("Maximum Speed is " + MaxSpeed);
                Console.WriteLine("Armor is " + Armor);
                Console.WriteLine("Amount of Crew is " + CabinCrew);
            }
            else if (choice == Choice.Color)
                Console.WriteLine("Color is " + Color);
            else if (choice == Choice.CountryOfManufacture)
                Console.WriteLine("Country Of Manufacture is " + CountryOfManufacture);
            else if (choice == Choice.MaximumSpeed)
                Console.WriteLine("Maximum Speed is " + MaxSpeed);
            else if (choice == Choice.Armor)
                Console.WriteLine("Armor is " + Armor);
            else if (choice == Choice.CabinCrew)
                Console.WriteLine("Amount of Crew is " + CabinCrew);
            else
                Console.WriteLine("Enterd wrong parametrs! Try again");
            Console.WriteLine("\n");
        }


        

    }

    [Flags]
    public enum Choice
    {
        All = 0,
        Color = 1,
        CountryOfManufacture = 2,
        MaximumSpeed = 4,
        Armor = 8,
        CabinCrew = 16
    }

}