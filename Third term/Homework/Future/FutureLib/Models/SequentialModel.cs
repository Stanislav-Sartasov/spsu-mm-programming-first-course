using System;
using System.Linq;
using System.Threading.Tasks;

namespace Future
{
    public class SequentialModel : IVectorLengthComputer
    {
        public double ComputeLength(int[] a)
        {
            double result;
            try
            {
                result = Task.Factory.StartNew(() => Math.Sqrt(a.Sum(x => x * x))).Result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                result = double.NaN;
            }
            return result;
        }
    }
}
