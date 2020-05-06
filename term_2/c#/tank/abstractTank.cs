using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tank
{
    public abstract class abstractTank
    {
        protected string title;
        protected string contry;
        protected int armor;
        protected int gunNumber;
        public virtual string getInfo()
        {
            return $" name: {title}\n contry: {contry}\n armor: {armor}mm\n number of guns: {gunNumber}";
        }
    }
 }
