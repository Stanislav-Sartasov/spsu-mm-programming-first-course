using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiberLib
{
    internal class FibersHolder
    {
        private Random random = new Random();
        private List<FiberData>[] fibersPrior;
        private List<FiberData> fibersLine;
        private FiberData tempHolder;
        private bool isHolded;
        public  int CountFibers { get; private set; }
        public  int LastFiberID { get; private set; }
        private  readonly int maxPriority = 10;

        public FibersHolder()
        {
            isHolded = false;
            CountFibers = 0;
            LastFiberID = 0;
            fibersLine = new List<FiberData>();
            fibersPrior = new List<FiberData>[maxPriority];
            for (int i = 0; i < 10; ++i)
                fibersPrior[i] = new List<FiberData>();
        }
        public void Add(Process proc)
        {
            LastFiberID += 1;
            CountFibers += 1;
            FiberData temp = new FiberData
            {
                fiber = new Fiber(proc.Run),
                priority = proc.Priority,
                num = LastFiberID
            };
            fibersPrior[proc.Priority % maxPriority].Add(temp);
            fibersLine.Add(temp);
        }
        public void Remove (FiberData current)
        {
            fibersLine.Remove(current);
            fibersPrior[current.priority % maxPriority].Remove(current);
            CountFibers -= 1;
            isHolded = false;
        }
        private void Backin()
        {
            if (isHolded)
            {
                fibersLine.Add(tempHolder);
                fibersPrior[tempHolder.priority % maxPriority].Add(tempHolder);
                isHolded = false;
            }
        }
        private void Takeout(FiberData fiber)
        {
            if (isHolded)
                Backin();
            if (!isHolded)
            {
                fibersLine.Remove(fiber);
                fibersPrior[fiber.priority % maxPriority].Remove(fiber);
                tempHolder = fiber;
                isHolded = true;
            }
        }
        public bool GetNextFiber(SchedulerPriority priority, ref FiberData data)
        {

            if (CountFibers == 1)
                Backin();
            switch (priority)
            {
                case SchedulerPriority.NonePriority:
                    data = fibersLine.First();
                    Backin();
                    Takeout(data);
                    return true;
                case SchedulerPriority.PriorityLevel:
                    int i = random.Next(2 * maxPriority) + 1;
                    if (i > maxPriority)
                        i = maxPriority;
                    int tempHolder = i;
                    --i;
                    do
                    {
                        if (fibersPrior[i].Count !=0)
                        {
                            data = fibersPrior[i].First();
                            Backin();
                            Takeout(data);
                            return true;
                        }
                        ++i;
                    } while (fibersPrior.Length > i);
                    i = tempHolder;
                    do
                    {
                        --i;
                        if (fibersPrior[i].Count != 0)
                        {
                            data = fibersPrior[i].First();
                            Backin();
                            Takeout(data);
                            return true;
                        }
                    } while (i > 0);
                    break;
            }
            return false;
        }
        public void Dispose()
        {
        }
    }
}
