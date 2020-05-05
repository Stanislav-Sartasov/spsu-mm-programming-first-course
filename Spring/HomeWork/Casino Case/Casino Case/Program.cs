using System;
using System.Collections.Generic;
using System.Text;
namespace Casino_Case
{
    class Program
    {
        enum Color : byte
        {
            red = 1,
            black = 2,
            blue = 3,
        }
        static void Main(string[] args)
        {
             Table tr = new Table();
            Game hz = new Game();


            Menu menu = new Menu();
            menu.mainMenu();



        }
    }  
}
