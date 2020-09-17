using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bash
{
    class MyBash
    {
        private readonly IInput myInput;
        private readonly IEngine myEngine;
        public MyBash(IInput startInput, IEngine startEngine)
        {
            myEngine = startEngine;
            myInput = startInput;
        }
        public MyBash()
        {
            myEngine = new Engine();
            myInput = new Input();
        }
        public void GoBash()
        {
            bool stop = false;
            while (!stop)
            {
                myEngine.InitInput(myInput.GetLine());
                stop = myEngine.StartCommand();
            }
        }
    }
}
