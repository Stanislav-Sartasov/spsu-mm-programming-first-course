﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bash
{
    public class Arg : Message
    {
        public readonly string arg;
        public Arg(string start)
        {
            interup = Interup.queue;
            arg = start;
            st = Status.arg;
        }
    }
}