using AbstractLibrary;

namespace VarietiesOfTanks
{
    public class PantherTank : AbstractTank
    {
        public int FuelCapacity { get; set; }
        public PantherTank(string name, string type, string place, float mass, float length, float height, int crew, int fuelCapacity) 
            : base(name, type, place, mass, length, height, crew)
        {
            FuelCapacity = fuelCapacity;
        }
        public override string GetInfo()
        {
            string s = base.GetInfo() + $"\nFuel capacity: {FuelCapacity}";
            return s;
        }
    }
}
