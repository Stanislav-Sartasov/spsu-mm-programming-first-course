using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public abstract class AbstractCascadeTasks
    {
        protected int SolveTasks(List<Task<int>> tasks)
        {
            if (tasks.Count == 0)
                return 0;

            while (tasks.Count > 1)
            {
                var newTasks = new List<Task<int>>();
                if (tasks.Count % 2 == 1)
                    newTasks.Add(tasks[tasks.Count - 1]);

                for (int i = 0; i < tasks.Count - 1; i += 2)
                {
                    int result = tasks[i + 1].Result;
                    newTasks.Add(tasks[i].ContinueWith(task => (task.Result + result)));
                }
                tasks = newTasks;
            }
            return (int)Math.Sqrt(tasks[0].Result);
        }
    }
}
