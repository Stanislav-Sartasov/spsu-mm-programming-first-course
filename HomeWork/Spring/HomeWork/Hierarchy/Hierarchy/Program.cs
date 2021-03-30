using System;
using Tanks;
namespace Hierarchy
{
    class Program
    {
        static void Main(string[] args)
        {
            Panter panter = new Panter();
            panter.Print(Choice.Color);
            panter.Print(Choice.All);
        }
    }
}
