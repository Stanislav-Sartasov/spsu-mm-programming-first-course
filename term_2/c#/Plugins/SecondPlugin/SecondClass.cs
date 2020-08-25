using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfacePlugin;

namespace SecondPlugin
{
    public class SecondClass : IPlugin
    {
        public void StartAction()
        {
            Console.WriteLine("SecondClass");
        }
    }
}
