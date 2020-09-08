using System;

namespace TankDescription
{
    public class HeavyTank : Tank
    {
        public int FrontalHullArmor { get; set; }
        public int FrontalTurretArmor { get; set; }
        public HeavyTank(string name, string country, int year, int caliber, int hullArmor, int turretArmor) : base(name, country, year, caliber)
        {
            FrontalHullArmor = hullArmor;
            FrontalTurretArmor = turretArmor;
        }
        public override string GetData()
        {
            string output = base.GetData() + $"type: heavy tank;\nSpecific characteristics: frontal hull armor: {FrontalHullArmor} mm, frontal turret armor: {FrontalTurretArmor} mm.\n";
            Console.WriteLine($"type: heavy tank;\nSpecific characteristics: frontal hull armor: {FrontalHullArmor} mm, frontal turret armor: {FrontalTurretArmor} mm.\n");
            return output;
        }
    }
}
