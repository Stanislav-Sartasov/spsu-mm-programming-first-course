using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Future
{
    public class CascadeModel : IVectorLengthComputer
    {
        public double ComputeLength(int[] a)
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
            while (true)
            {
                var resultsPairs = tasks.Select((s, i) => new { Value = s, Index = i })
                    .GroupBy(x => x.Index / 2)
                    .Select(grp => grp.Select(x => x.Value).ToList())
                    .ToList();
                tasks.Clear();
                int count = resultsPairs.Count();
                if (count == 1)
                    return Math.Sqrt(resultsPairs.Sum(pair => pair.Sum(task => task.Result)));
                else if (resultsPairs.Last().Count % 2 == 1)
                {
                    tasks.AddRange(resultsPairs.Last());
                    resultsPairs.RemoveAt(count - 1);
                }
                tasks.AddRange(resultsPairs.Select(pair => 
                Task.Factory.StartNew(() => Sum(pair[0].Result, pair[1].Result))));
            }
        }

        private int Sum(int x, int y) => x + y;

        private int Square(int x) => x * x;
    }
}
