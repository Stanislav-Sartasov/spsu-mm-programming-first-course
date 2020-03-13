using System;
using System.Collections.Generic;
using System.Text;
using abstractTank;

namespace manyTanks
{
    public class t34 : anyTank
    {
        private int canGo;
        public t34()
        {
            base.contry = "USSR";
            base.title = "T34";
            base.gunNumber = 3;
            base.armor = 45;
            canGo = 250;
        }
        public override void getInfo()
        {
            base.getInfo();
            Console.WriteLine($" can go without refuel: {canGo}km");
        }
    }

    public class m4 : anyTank
    {
        private int sideArmor, towerArmor;
        public m4()
        {
            base.contry = "USA";
            base.title = "M4 Sharman";
            base.gunNumber = 4;
            base.armor = 51;
            sideArmor = 38;
            towerArmor = 76;
        }
        public override void getInfo()
        {
            base.getInfo();
            Console.WriteLine($" side armor: {sideArmor}\n tower armor: {towerArmor}");
        }
    }
}
