using System;
using System.Collections.Generic;
using System.Text;

namespace abstractTank
{
    public abstract class anyTank
    {
        protected string title;
        protected string contry;
        protected int armor;
        protected int gunNumber;
        public virtual void getInfo()
        {
            Console.WriteLine($" name: {title}\n contry: {contry}\n armor: {armor}mm\n number of guns: {gunNumber}");
        }
    }
}
