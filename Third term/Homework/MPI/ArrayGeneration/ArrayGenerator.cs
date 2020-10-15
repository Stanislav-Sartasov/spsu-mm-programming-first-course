using System;
using System.Collections.Generic;
using System.Text;

namespace ArrayGeneration
{
    public static class ArrayGenerator
    {
        public static List<int> GenerateArray(int capacity)
        {
            Random r = new Random(DateTime.Now.Millisecond);
            List<int> lst = new List<int>(capacity);

            for (int i = 0; i < capacity; i++)
            {
                lst.Add((i % 2 == 0 ? 1 : -1) * r.Next(1000001));
            }

            return lst;
        }
    }
}
