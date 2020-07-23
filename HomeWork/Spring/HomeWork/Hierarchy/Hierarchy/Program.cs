using System;
using Tanks;
namespace Hierarchy
{
    class Program
    {
        static void Main(string[] args)
        {
            Panter panter = new Panter();
            panter.Print(Choose.Color);
            panter.Print(Choose.All);
        }
    }
}
