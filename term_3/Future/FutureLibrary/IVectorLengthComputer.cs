using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutureLibrary
{
    public interface IVectorLengthComputer
    {
        double ComputeLength(int[] coordinates);
    }
}
