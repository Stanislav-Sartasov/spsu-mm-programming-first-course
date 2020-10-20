using System;

namespace Future
{
    public static class ArrayGenerator
    {
        public static int[] GenerateArray(int capacity)
        {
            Random r = new Random(DateTime.Now.Millisecond);
            int[] array = new int[capacity];

            for (int i = 0; i < capacity; i++)
            {
                array[i] = (i % 2 == 0 ? 1 : -1) * r.Next(10);
            }

            return array;
        }
    }
}
