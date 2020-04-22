using System;
using AbstractLibrary;

namespace VarietiesOfTanks
{
    public class TigerTank : AbstractTank
    {
        public float MaxSpeed { get; set; }
        public TigerTank(string name, string type, string place, float mass, float length, float height, int crew, float maxSpeed) 
            : base(name, type, place, mass, length, height, crew)
        {
            MaxSpeed = maxSpeed;
        }
        public override string Info()
        {
            string s = base.Info() + $"\nMaximum speed: {MaxSpeed}";
            return s;
        }
    }
}
