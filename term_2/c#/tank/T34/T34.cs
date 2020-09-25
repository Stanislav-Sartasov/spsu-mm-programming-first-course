using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AbstractTanks;

namespace TankT34
{
    public class T34 : AbstractTank
    {
        private readonly int canGo;
        public T34()
        {
            base.contry = "USSR";
            base.title = "T34";
            base.gunNumber = 3;
            base.armor = 45;
            canGo = 250;
        }
        public override string getInfo()
        {
            return base.getInfo() + $" can go without refuel: {canGo}km";
        }
    }
}
