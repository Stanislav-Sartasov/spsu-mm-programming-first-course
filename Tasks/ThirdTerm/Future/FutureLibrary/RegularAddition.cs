using System;
using System.Threading.Tasks;

namespace Future.Library
{
    public class RegularAddition : IVectorLengthComputer
    {
        public int ComputeLength(int[] a)
        {
            int processors = Environment.ProcessorCount;
            int sizeOfBlock = a.Length / processors;
            if (a.Length % processors != 0)
                sizeOfBlock++;

            int[,] blocks = new int[processors, sizeOfBlock];
            Task<int>[] tasks = new Task<int>[processors + 1];

            for (int i = 0; i < a.Length; i++)
                blocks[i / sizeOfBlock, i % sizeOfBlock] = a[i];

            for (int i = 0; i < processors; i++)
            {
                int[] result = new int[sizeOfBlock];
                for (int j = 0; j < sizeOfBlock; j++)
                    result[j] = blocks[i, j];
                tasks[i] = Task.Factory.StartNew<int>(() => SumOfSquaresOfAnArray(result));
            }

            int[] intermediateResults = new int[processors];

            for (int i = 0; i < processors; i++)
                intermediateResults[i] = tasks[i].Result;

            tasks[processors] = Task.Factory.StartNew<int>(() => SumOfArray(intermediateResults));

            return (int)Math.Sqrt(tasks[processors].Result);
        }


        private int SumOfSquaresOfAnArray(int[] a)
        {
            int result = 0;
            for (int i = 0; i < a.Length; i++)
                result += a[i] * a[i];
            return result;
        }

        private int SumOfArray(int[] a)
        {
            int result = 0;
            for (int i = 0; i < a.Length; i++)
                result += a[i];
            return result;
        }

    }
}
