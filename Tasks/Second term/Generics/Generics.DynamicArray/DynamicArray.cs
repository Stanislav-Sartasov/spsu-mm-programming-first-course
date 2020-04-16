using System;
using System.Collections.Generic;

namespace DynamicArray
{
    public class DynamicArray<T>
    {
        private T[] body;
        private int arraySize = 2;
        private int firstFreeIndex = 0;

        public DynamicArray()
        {
            body = new T[arraySize]; 
            for (int i = 0; i < arraySize; i++)
                body[i] = default(T);
        }

        public void SetAtIndex(T value, int index)
        {
            if (-1 < index && index < arraySize)
                body[index] = value;
        }

        public T GetAtIndex(int index)
        {
            if (index > -1 && index < arraySize)
                return body[index];
            return default(T);
        }

        public int Find(T value)
        {
            int index = -1;
            for (int i = 0; i < arraySize; i++)
            {
                if (body[i].Equals(value))
                {
                    index = i;
                    break;
                }
            }

            return index;
        }

        public void Add(T value)
        {
            if (firstFreeIndex == arraySize)
            {
                T[] newBody = new T[arraySize];
                arraySize *= 2;
                body.CopyTo(newBody, 0);
                body = new T[arraySize];
                newBody.CopyTo(body, 0);
            }
            body[firstFreeIndex] = value;
            firstFreeIndex++;
        }

        public void Delete(int index)
        {
            if (index > -1 && index < arraySize)
            {
                body[index] = default(T);
                for (int i = index; i < arraySize - 1; i++)
                    body[i] = body[i + 1];
                for (int i = firstFreeIndex; i < arraySize; i++)
                    body[i] = default(T);
                firstFreeIndex--;
            }
        }

        public static void PrintArray(DynamicArray<T> dynamicArray)
        {
            Console.WriteLine("****");
            for (int i = 0; i < dynamicArray.firstFreeIndex; i++)
            {
                Console.Write(dynamicArray.body[i].ToString() + " ");
            }
            Console.WriteLine("\n****\n");
        }
    }
}
