using System;
using System.Collections.Generic;
using System.Text;

namespace FuturePattern.Library
{
    public interface IVectorLengthComputer
    {
        int ComputeLength(int[] a);
        string Name();
    }
}
