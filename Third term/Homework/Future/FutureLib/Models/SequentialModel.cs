using System;
using System.Linq;
using System.Threading.Tasks;

namespace Future
{
    public class SequentialModel : IVectorLengthComputer
    {
        public double ComputeLength(int[] a)
        {
            return Task.Factory.StartNew(() => Math.Sqrt(a.Sum(x => x * x))).Result;
        }
    }
}
