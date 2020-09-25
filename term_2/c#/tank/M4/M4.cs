using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AbstractTanks;

namespace TankM4
{
    public class M4 : AbstractTank
    {
        private readonly int sideArmor, towerArmor;
        public M4()
        {
            base.contry = "USA";
            base.title = "M4 Sharman";
            base.gunNumber = 4;
            base.armor = 51;
            sideArmor = 38;
            towerArmor = 76;
        }
        public override string getInfo()
        {
            return base.getInfo() + $" side armor: {sideArmor}\n tower armor: {towerArmor}";
        }
    }
}
