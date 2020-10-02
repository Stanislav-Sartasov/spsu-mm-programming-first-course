using System;

namespace Fibers.Processes
{
    public static class Creator
    {
        public static IProcess Create(int type)
        {
            if (type == 1)
                return new Fibonacci();
            if (type == 2)
                return new DigitalRoot();
            return null;
        }
    }
}
