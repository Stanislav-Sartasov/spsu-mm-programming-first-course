using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FuturePattern.Library
{
    class ModifiedCascade : IVectorLengthComputer
    {
        public int ComputeLength(int[] a)
        {
            int processors = Environment.ProcessorCount;
            int sizeOfBlock = a.Length / processors + 1;
            int[][] blocks = new int[processors][];
            Task<int>[] tasks = new Task<int>[processors];
            Task<int> result;

            for (int i = 0; i < processors; i++)
                blocks[i] = new int[sizeOfBlock];
            for (int i = 0; i < sizeOfBlock * processors; i++)
            {
                blocks[i / sizeOfBlock][i % sizeOfBlock] = 0;
                if (i < a.Length)
                    blocks[i / sizeOfBlock][i % sizeOfBlock] = a[i];
            }    
            for (int i = 0; i < processors; i++)
            {
                int[] temp = blocks[i];
                tasks[i] = Task.Factory.StartNew<int>(() => SumSquare(temp));
            }
            int[] intermediateResults = new int[processors];
            for (int i = 0; i < processors; i++)
                intermediateResults[i] = tasks[i].Result;
            result = Task.Factory.StartNew<int>(() => Sum(intermediateResults));

            return (int)Math.Sqrt(result.Result);
        }


        private int SumSquare(int[] a)
        {
            int result = 0;
            for (int i = 0; i < a.Length; i++)
                result += a[i] * a[i];
            return result;
        }
        private int Sum(int[] a)
        {
            int result = 0;
            for (int i = 0; i < a.Length; i++)
                result += a[i];
            return result;
        }

        public string Name()
        {
            return "Modified cascade";
        }
    }
}
