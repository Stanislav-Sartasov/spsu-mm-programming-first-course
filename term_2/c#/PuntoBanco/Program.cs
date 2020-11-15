using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuntoBanco
{
    class Program
    {
        static void Main(string[] args)
        {
            UserInterface userGame = new UserInterface();
            userGame.GoGame();
            Console.ReadKey();
        }
    }
}
