using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Models
{
    public class CascadeModel : AbstractCascadeTasks, IVectorLengthComputer
    {
        public int ComputeLength(int[] a)
        {
            List<int> vector = a.ToList();
            if (vector.Count % 2 == 1)
                vector.Add(0);

            List<Task<int>> tasks = new List<Task<int>>();
            for (int i = 0; i < vector.Count - 1; i += 2)
            {
                int x = vector[i];
                int y = vector[i + 1];
                tasks.Add(Task.Factory.StartNew(() => (x * x + y * y)));

            }
            return SolveTasks(tasks);
        }
    }
}
