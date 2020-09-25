using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bash
{
    public class Vari : Message
    {
        public readonly string vari;
        internal Vari(string start, Status myStatus)
        {
            interup = Interup.queue;
            vari = start;
            st = myStatus;
        }
    }
}
