using System.Collections.Generic;

namespace TankConfig
{
    public class M1Abrams : AbstractTank.AbstractTank
    {
        public M1Abrams() : base("M1-Abrams", "USA", 55, new Dictionary<string, int> { { "Armor in millimeters", 50 }, { "Weapon caliber in millimeters", 120 }, { "Crew", 4 } })
        { 
        }
    }
}