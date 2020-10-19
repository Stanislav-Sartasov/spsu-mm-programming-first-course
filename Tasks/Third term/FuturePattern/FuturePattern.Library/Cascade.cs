using System;
using System.Linq;
using System.Threading.Tasks;

namespace FuturePattern.Library
{
    class Cascade : IVectorLengthComputer
    {
        public int ComputeLength(int[] a)
        {
            if (a.Length % 2 == 1)
                a.Append(0);

            int amountOfIterations = (int)Math.Round(Math.Log(a.Length) + 0.5) + 1;
            Task<int>[][] arrayOfResults = new Task<int>[amountOfIterations][];
            arrayOfResults[0] = new Task<int>[(a.Length + 1) / 2];
            for (int i = 1; i < amountOfIterations; i++)
            {
                int temp = (arrayOfResults[i - 1].Length + 1) / 2;
                arrayOfResults[i] = new Task<int>[temp];
            }
            int j = 0;
            for (int i = 0; i < a.Length; i += 2)
            {
                int x = a[i];
                int y = 0;
                if (i + 1 < a.Length)
                    y = a[i + 1];

                arrayOfResults[0][j] = Task.Factory.StartNew<int>(() => SumSquare(x, y));
                j++;
            }
            for (int i = 1; i < amountOfIterations; i++)
            {
                j = 0;
                for (int k = 0; k < arrayOfResults[i - 1].Length; k += 2)
                {
                    int x = arrayOfResults[i - 1][k].Result;
                    int y = 0;
                    if (k + 1 < arrayOfResults[i - 1].Length)
                        y = arrayOfResults[i - 1][k + 1].Result;
                    arrayOfResults[i][j] = Task.Factory.StartNew<int>(
                                                        () => Sum(x, y));
                    j++;
                }
            }
            return (int)Math.Sqrt(arrayOfResults[arrayOfResults.Length - 1][0].Result);
        }
        private int SumSquare(int a, int b)
        {
            return a * a + b * b;
        }

        private int Sum(int a, int b)
        {
            return a + b;
        }

        public string Name()
        {
            return "Cascade";
        }
    }
}
