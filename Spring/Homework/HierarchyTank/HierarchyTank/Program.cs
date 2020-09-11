using TankConfig;

namespace HierarchyTank
{
    class Program
    {
        static void Main(string[] args)
        {
            M1Abrams m1Abrams = new M1Abrams();
            T90 t90 = new T90();

            m1Abrams.PrintInfo();
            t90.PrintInfo();
        }
    }
}