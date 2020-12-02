using System;
using Future.Library;

namespace Future
{
    class Future
    {
        static void Main(string[] args)
        {
            IVectorLengthComputer[] realizations = new IVectorLengthComputer[] { new Cascade(), new RegularAddition() };
            int[] a = new int[] { -5, 5, -20, 3, 4, 18, 0, -8, -9, -11, 6, 16, 14, -4, -3 };
            
            foreach (IVectorLengthComputer realization in realizations)
            {
                int result = realization.ComputeLength(a);
                Console.WriteLine(realization.GetType() + " " + result);
            }
        }
    }
}
