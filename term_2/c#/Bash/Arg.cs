using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bash
{
    public class Arg : Message
    {
        public Arg(string start)
        {
            interup = Interup.Queue;
            Cmd = start;
            st = Status.Arg;
        }
    }
}
