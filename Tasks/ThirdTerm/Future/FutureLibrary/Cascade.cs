using System;
using System.Threading.Tasks;

namespace Future.Library
{
    public class Cascade : IVectorLengthComputer
    {
        public int ComputeLength(int[] a)
        {
            int closestPowerOfTwo = ClosestPowerOfTwo(a.Length);
            Task<int>[] computationTree = new Task<int>[2 * closestPowerOfTwo];

            for (int i = 2 * closestPowerOfTwo - 2; i >= 0; i--)
            {
                if (i < closestPowerOfTwo - 1)
                {
                    int resultOne = computationTree[i * 2 + 1].Result, resultTwo = computationTree[i * 2 + 2].Result;
                    computationTree[i] = Task.Factory.StartNew<int>(() => Sum(resultOne, resultTwo));
                }
                else if (i - closestPowerOfTwo + 1 >= a.Length)
                {
                    computationTree[i] = Task.Factory.StartNew<int>(() => 0);
                }
                else
                {
                    int result = a[i - closestPowerOfTwo + 1];
                    computationTree[i] = Task.Factory.StartNew<int>(() => Square(result));
                }    
            }

            return (int)Math.Sqrt(computationTree[0].Result);
        }

        private int Square(int a)
        {
            return a * a;
        }

        private int Sum(int a, int b)
        {
            return a + b;
        }

        private int ClosestPowerOfTwo(int x)
        {
            x -= 1;
            x |= (x >> 1);
            x |= (x >> 2);
            x |= (x >> 4);
            x |= (x >> 8);
            x |= (x >> 16);
            return x + 1;
        }
    }
}
