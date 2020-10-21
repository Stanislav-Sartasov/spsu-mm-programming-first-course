using FutureLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Future
{
    public class CascadeModel : CascadeBase, IVectorLengthComputer 
    {
        public double ComputeLength(int[] a)
        {
            double result;
            try
            {
                var vector = a.ToList();
                if (vector.Count % 2 == 1)
                    vector.Add(0);
                var tasks = new List<Task<int>>();
                for (int i = 0; i < vector.Count; i += 2)
                {
                    int x = vector[i];
                    int y = vector[i + 1];
                    tasks.Add(Task.Factory.StartNew(() => Sum(Square(x), Square(y))));
                }
                result = CascadeCalculate(tasks);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                result = double.NaN;
            }
            return result;
        }

        private int Sum(int x, int y) => x + y;

        private int Square(int x) => x * x;
    }
}
