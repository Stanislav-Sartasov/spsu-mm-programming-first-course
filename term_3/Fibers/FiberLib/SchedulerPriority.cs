using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiberLib
{
    public enum SchedulerPriority
    {
        NonePriority, // first in list is first to execute
        PriorityLevel // priority by 3 levels high, medium, low
    }
}
