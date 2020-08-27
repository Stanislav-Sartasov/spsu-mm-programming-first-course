using System;

namespace TankCommon
{
    public abstract class Tank
    {
        public string Name { get; set; }
        public string Country { get; set; }
        public int ProductionYear { get; set; }
        public int GunCaliber { get; set; }
        public Tank(string name, string country, int year, int caliber)
        {
            Name = name;
            Country = country;
            ProductionYear = year;
            GunCaliber = caliber;
        }
        public virtual string GetData()
        {
            string output = $"Tank description:\nName: {Name}, country: {Country}, production year: {ProductionYear}, gun caliber: {GunCaliber} mm, ";
            Console.Write(output);
            return output;
        }
    }
}
