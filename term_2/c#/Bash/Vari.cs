using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bash
{
    public class Vari : Message
    {
        internal Vari(string start, Status myStatus)
        {
            interup = Interup.Queue;
            Cmd = start;
            st = myStatus;
        }
    }
}
