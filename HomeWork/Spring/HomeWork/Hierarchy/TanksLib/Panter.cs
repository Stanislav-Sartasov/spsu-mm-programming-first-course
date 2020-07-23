using System;
using System.Collections.Generic;
using System.Text;

namespace Tanks
{
    public class Panter : Tank
    {
        public Panter() : base("Grey", "German", 55, 1200, 5)
        {
        }
        public void Print(Choose choice)
        {
            base.Print(choice);
        }
    }
}