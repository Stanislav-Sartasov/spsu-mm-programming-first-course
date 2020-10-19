using System;
using System.Collections.Generic;
using System.Text;

namespace FuturePattern.Library
{
    public abstract class Creator
    {
        public abstract IVectorLengthComputer FactoryMethod();
    }
}
