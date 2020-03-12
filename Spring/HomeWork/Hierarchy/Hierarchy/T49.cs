using System;
using System.Collections.Generic;
using System.Text;

namespace Hierarchy
{
  public  class T49 : Tank
    {
        public T49() : base("Brown", "the USA", 65, 1300, 4)
        {
        }
        public void Print(string choice = "0")
        {
            base.Print(choice);
        }
    }
}
