using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bash
{
    public class Command : Message
    {
        public Command(string start)
        {
            interup = Interup.Queue;
            Cmd = start;
            st = Status.Cmd;
        }
        internal Command(string start, Interup myInterup)
        {
            interup = myInterup;
            Cmd = start;
            st = Status.Cmd;
        }
    }
}
