using System.Collections.Generic;
using AbstractPanzer;

namespace TankConfig
{
    public class T90 : Panzer
    {
        public T90() : base("T-90", "Russia", 46, new Dictionary<string, int> { { "Armor in millimeters", 65 }, { "Weapon caliber in millimeters", 125 }, { "Crew", 3 } })
        { }
    }
}