using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DynamicArray
{
    public class WeakDynamicArray<T> where T : class
    {
        private int arraySize = 3;
        private int firstFreeIndex = 0;
        public int Lifetime;

        private WeakReference<T>[] body;
        public WeakDynamicArray(int time)
        {
            body = new WeakReference<T>[arraySize];
            Lifetime = time;
        }
        public int Find(T value)
        {
            int index = -1;
            for (int i = 0; i < arraySize; i++)
            {
                T o;
                if (body[i] != null)
                {
                    body[i].TryGetTarget(out o);
                    if (value.Equals(o))
                    {
                        index = i;
                        break;
                    }
                }
            }
            return index;
        }

        public T GetByIndex(int index)
        {
            T res = default(T);
            if (-1 < index && index < firstFreeIndex)
                if (body[index].TryGetTarget(out res))
                    return res;
            return res;
        }

        public void SetAtIndex(T value, int index)
        {
            if (-1 < index && index < firstFreeIndex)
            {
                WeakReference<T> weakRef = new WeakReference<T>(value);
                body[index] = (weakRef);
            }
        }
        public async void AddToEnd(T value)
        {
            if (firstFreeIndex == arraySize)
            {
                WeakReference<T>[] temp = new WeakReference<T>[arraySize];
                body.CopyTo(temp, 0);
                arraySize *= 2;
                body = new WeakReference<T>[arraySize];
                temp.CopyTo(body, 0);
            }
            WeakReference<T> weakRef = new WeakReference<T>(value);
            body[firstFreeIndex] = (weakRef);
            firstFreeIndex++;
            await Task.Delay(Lifetime);
        }
        public void Delete(int index)
        {
            if (-1 < index && index < arraySize)
                body[index] = null;
        }
        public void PrintArray()
        {
            for (int i = 0; i < firstFreeIndex; i++)
            {
                T o;
                if (body[i] != null)
                    if (body[i].TryGetTarget(out o))
                        Console.WriteLine(o.ToString());
                    else
                    {
                        body[i] = default(WeakReference<T>);
                        Console.WriteLine("Deleted");
                    }
                else
                    Console.WriteLine("Deleted");
            }
        }
    }
}
