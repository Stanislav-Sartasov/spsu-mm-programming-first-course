using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfacePlugin;

namespace FirstPlugin
{
    public class FirstClass : IPlugin
    {
        public void StartAction()
        {
            Console.WriteLine("FirstClass");
        }
    }
}
