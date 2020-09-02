using System.Collections.Generic;
using AbstractPanzer;

namespace TankConfig
{
    public class M1Abrams : Panzer
    {
        public M1Abrams() : base("M1-Abrams", "USA", 55, new Dictionary<string, int> { { "Armor in millimeters", 50 }, { "Weapon caliber in millimeters", 120 }, { "Crew", 4 } })
        { }
    }
}