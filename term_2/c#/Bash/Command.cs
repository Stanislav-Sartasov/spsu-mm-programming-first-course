using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bash
{
    public class Command : Message
    {
        public readonly string cmd;
        public Command(string start)
        {
            interup = Interup.Queue;
            cmd = start;
            st = Status.Cmd;
        }
        internal Command(string start, Interup myInterup)
        {
            interup = myInterup;
            cmd = start;
            st = Status.Cmd;
        }
    }
}
