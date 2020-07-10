using System;
using System.Collections.Generic;
using System.Text;

namespace Hierarchy
{
    public class T34 : Tank
    {
        public T34() : base("Black", "Ussr", 56, 400, 4)
        {
        }
        public void Print(string choice = "0")
        {
            base.Print(choice);
        }
    }
}