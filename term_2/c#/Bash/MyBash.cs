using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bash
{
    public class MyBash
    {
        private readonly IInput myInput;
        private readonly IEngine myEngine;
        public MyBash(IInteraction startInput, IEngine startEngine)
        {
            myEngine = startEngine;
            myInput = new Input(startInput);
        }
        public MyBash(IInteraction startInput)
        {
            myEngine = new Engine();
            myInput = new Input(startInput);
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
