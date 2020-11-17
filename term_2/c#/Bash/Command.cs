using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bash
{
    public class Command : Message
    {
        public bool NeedArg;
        public Command(string start, bool myNeedArg)
        {
            NeedArg = myNeedArg;
            interup = Interup.Queue;
            Cmd = start;
            st = Status.Cmd;
        }
        internal Command(string start, Interup myInterup, bool myNeedArg)
        {
            NeedArg = myNeedArg;
            interup = myInterup;
            Cmd = start;
            st = Status.Cmd;
        }
    }
}
