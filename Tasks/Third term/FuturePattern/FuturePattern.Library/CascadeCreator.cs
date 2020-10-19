using System;
using System.Collections.Generic;
using System.Text;

namespace FuturePattern.Library
{
    public class CascadeCreator : Creator
    {
        public override IVectorLengthComputer FactoryMethod()
        {
            return new Cascade();
        }
    }
}
