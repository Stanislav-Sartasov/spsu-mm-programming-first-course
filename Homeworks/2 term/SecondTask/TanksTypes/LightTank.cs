using System;

namespace TankDescription
{
    public class LightTank : Tank
    {
        public int MaxSpeed { get; set; }
        public double SpecificPower { get; set; }
        public LightTank(string name, string country, int year, int caliber, int speed, double power) : base(name, country, year, caliber)
        {
            MaxSpeed = speed;
            SpecificPower = power;
        }
        public override string GetData()
        {
            string output = base.GetData() + $"type: light tank;\nSpecific characteristics: max speed: {MaxSpeed} km/h, specific power: {SpecificPower} hp/t.\n";
            Console.WriteLine($"type: light tank;\nSpecific characteristics: max speed: {MaxSpeed} km/h, specific power: {SpecificPower} hp/t.\n");
            return output;
        }
    }
}
