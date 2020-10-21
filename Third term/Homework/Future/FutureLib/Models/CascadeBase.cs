using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FutureLib.Models
{
    public abstract class CascadeBase
    {
        public double CascadeCalculate(List<Task<int>> tasks)
        {
            while (true)
            {
                var nextTasks = new List<Task<int>>();
                if (tasks.Count == 1)
                    return Math.Sqrt(tasks[0].Result);
                else if (tasks.Count % 2 == 1)
                    nextTasks.Add(tasks.Last());
                for (int i = 0; i + 1 <= tasks.Count - 1; i += 2)
                {
                    var nextTask = tasks[i + 1].Result;
                    nextTasks.Add(tasks[i].ContinueWith(task => Sum(task.Result, nextTask)));
                }
                tasks = nextTasks;
            }
        }

        private int Sum(int x, int y) => x + y;
    }
}
