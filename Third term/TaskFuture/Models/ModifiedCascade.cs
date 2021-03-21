using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ModifiedCascade : AbstractCascadeTasks, IVectorLengthComputer
    {
        public int ComputeLength(int[] a)
        {
            int n = Environment.ProcessorCount;
            List<Task<int>> tasks = new List<Task<int>>();
            for (int i = 0; i < n; i++)
            {
                var block = new List<int>();
                if (i < n - 1)
                {
                    for (int j = 0; j < a.Length / n; j++)
                        block.Add(a[a.Length / n * i + j]);
                }
                else
                {
                    for (int j = a.Length / n * i; j < a.Length; j++)
                        block.Add(a[j]);
                }
                tasks.Add(Task.Factory.StartNew(() => block.Select(x => x * x).ToList().Sum()));
            }
            return SolveTasks(tasks);
        }
    }
}
