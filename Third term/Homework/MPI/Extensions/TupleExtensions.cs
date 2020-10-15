using System;
using System.Collections.Generic;

namespace Extensions
{
    public static class TupleExtensions
    {
        public static T Max<T>(this Tuple<T, T> tuple)
            where T : IComparable
        {
            return tuple.Item1.CompareTo(tuple.Item2) > 0 ? tuple.Item1 : tuple.Item2;
        }

        public static T Min<T>(this Tuple<T, T> tuple)
            where T : IComparable
        {
            return tuple.Item1.CompareTo(tuple.Item2) > 0 ? tuple.Item2 : tuple.Item1;
        }

    }
}
