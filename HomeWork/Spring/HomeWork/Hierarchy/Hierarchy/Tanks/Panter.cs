using System;
using System.Collections.Generic;
using System.Text;

namespace Hierarchy
{
    public class Panter : Tank
    {
        public Panter() : base("Grey", "German", 55, 1200, 5)
        {
        }
        public void Print(string choice = "0")
        {
            base.Print(choice);
        }
    }
}