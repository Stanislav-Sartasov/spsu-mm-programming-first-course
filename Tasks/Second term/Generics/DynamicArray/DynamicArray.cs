using System;
using System.Collections.Generic;

namespace DynamicArray
{
    public class DynamicArray<T>
    {
        private T[] Body;
        private int ArraySize = 2;
        private int FirstFreeIndex = 0;
        public DynamicArray()
        {
            Body = new T[ArraySize]; 
            for (int i = 0; i < ArraySize; i++)
                Body[i] = default(T);
        }
        private void SetAtIndex(int index, T value)
        {
            Body[index] = value;
            FirstFreeIndex++;
        }
        public T GetAtIndex(int index)
        {
            if (index > -1 && index < FirstFreeIndex)
                return Body[index];
            return default(T);
        }
        public int Find(T value)
        {
            int index = -1;
            for (int i = 0; i < ArraySize; i++)
            {
                if (Body[i].Equals(value))
                {
                    index = i;
                    break;
                }
            }

            return index;
        }

        public void Add(T value)
        {
            if (FirstFreeIndex == ArraySize)
            {
                T[] newBody = new T[ArraySize];
                ArraySize *= 2;
                Body.CopyTo(newBody, 0);
                Body = new T[ArraySize];
                newBody.CopyTo(Body, 0);
            }
            SetAtIndex(FirstFreeIndex, value);
        }
        public void Delete(int index)
        {
            if (index > -1 && index < ArraySize)
            {
                Body[index] = default(T);
                for (int i = index; i < ArraySize - 1; i++)
                    Body[i] = Body[i + 1];
                for (int i = FirstFreeIndex; i < ArraySize; i++)
                    Body[i] = default(T);
                FirstFreeIndex--;
            }
        }
        public static void PrintArray(DynamicArray<T> dynamicArray)
        {
            Console.WriteLine("****");
            for (int i = 0; i < dynamicArray.FirstFreeIndex; i++)
            {
                Console.Write(dynamicArray.Body[i].ToString() + " ");
            }
            Console.WriteLine("\n****\n");
        }
    }
}
